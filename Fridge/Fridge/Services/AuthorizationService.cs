using Fridge.Data.Models;
using Fridge.Data.Repositories.Interfaces;
using Fridge.Models;
using Fridge.Models.Requests;
using Fridge.Services.Abstracts;
using System.Security.Cryptography;


namespace Fridge.Services
{
    public class AuthorizationService : IAuthService
    {
        private readonly IRepositoryManager repository;
        private TokenInfo? tokenInfo;

        public AuthorizationService(IConfiguration config, IHttpContextAccessor httpContextAccessor, IRepositoryManager repository)
        {
            this.repository = repository;

            tokenInfo = new TokenInfo(repository, httpContextAccessor, config);
        }
        
        public async Task<string> RegisterUser(AddUserModel addUserModel)
        {
            var role = addUserModel.IsOwner ? UserRoles.Owner : UserRoles.Renter;

            if (repository.User.FindBy(u => u.Email == addUserModel.Email && u.Role == role) is not null)
            {
                throw new ArgumentException($"{role} with the same email has been registered.");
            }

            CreatePasswordHash(addUserModel.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var renter = new User
            {
                Id = Guid.NewGuid(),
                Email = addUserModel.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Role = role
            };

            var token = tokenInfo?.CreateToken(renter);
           
            repository.User.AddUser(renter);

            await repository.SaveAsync();

            var user = repository.User.FindBy(u => u.Email == addUserModel.Email);

            tokenInfo?.SetRefreshToken(user);
            repository.User.UpdateUser(user);

            await repository.SaveAsync();

            return token!;
        }

        public async Task<string> RegisterThroughSocialMedia(AddUserSocialAuth addUserSocialAuthModel)
        {
            var user = new User()
            {
                Role = addUserSocialAuthModel.IsOwner ? UserRoles.Owner : UserRoles.Renter,
                SocialId = addUserSocialAuthModel.SocialId,
                AuthVia = addUserSocialAuthModel.AuthVia,
            };

            if (repository.User.FindBy(u => u.SocialId == addUserSocialAuthModel.SocialId &&
                u.Role == user.Role) is not null)
            {
                return tokenInfo.CreateToken(user);
            }
            else
            {
                user.Id = Guid.NewGuid();
                repository.User.AddUser(user);
                await repository.SaveAsync();

                return tokenInfo.CreateToken(user);
            }
        }

        public async Task<string> LoginUser(LoginModel loginModel)
        {
            var user = repository.User.FindBy(u => u.Email == loginModel.Email);

            if (user is null)
            {
                return await RegisterUser(new AddUserModel
                {
                    Email = loginModel.Email,
                    Password = loginModel.Password,
                    IsOwner = false,
                });
            }
            else
            {
                VerifyData(user, loginModel);

                var token = tokenInfo?.CreateToken(user);
                tokenInfo?.SetRefreshToken(user);

                repository.User.UpdateUser(user);
                await repository.SaveAsync();

                return token;
            }
        }

        public async Task<string> GetRefreshToken()
        {
            var user = await tokenInfo.GetUser();

            return await tokenInfo.UpdateRefreshToken(user);
        }

        private void VerifyData(User user, LoginModel loginModel)
        {
            if (user!.Email != loginModel.Email)
            {
                throw new ArgumentException("Not found.");
            }

            if (!VerifyPasswordHash(loginModel.Password, user.PasswordHash!, user.PasswordSalt!))
            {
                throw new ArgumentException("Wrong password");
            }
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }
    }
}
