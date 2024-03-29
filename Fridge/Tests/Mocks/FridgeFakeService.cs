﻿using Fridge.Services.Abstracts;
using Moq;

namespace Fridge.Tests.Mocks
{
    public class FridgeFakeService
    {
        public Mock<IFridgeService> Mock;
        public IFridgeService Service;

        public FridgeFakeService()
        {
            Mock = new Mock<IFridgeService>();
            Service = Mock.Object;
        }
    }
}
