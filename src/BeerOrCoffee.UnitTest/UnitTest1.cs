using System;
using System.IO;
using BeerOrCoffee.Models;
using BeerOrCoffee.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BeerOrCoffee.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestEmotionApi()
        {
            var service = new EmotionCognitiveSevice();
            var imageBytes = File.ReadAllBytes("W:\\Bitbucket\\BeerOrCoffee\\test_Image.jpg");
            var response = service.SendImage(imageBytes);
            Assert.IsTrue(response.Result != null && response.Result.Length > 0);
        }
    }
}
