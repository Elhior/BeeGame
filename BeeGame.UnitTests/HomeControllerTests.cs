using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bee_game.Controllers;
using NUnit.Framework;
using System.Web.Mvc;

namespace BeeGame.UnitTests
{
    [TestFixture]
    class HomeControllerTests
    {
        private HomeController controller;
        private ViewResult result;

        [OneTimeSetUp]
        public void SetupContext()
        {
            controller = new HomeController();
            result = controller.Index() as ViewResult;
        }

        [Test]
        public void IndexViewResultNotNull()
        {
            Assert.IsNotNull(result);
        }
        [Test]
        public void IndexViewEqualIndexCshtml()
        {
           Assert.AreEqual("Index", result.ViewName);
        }
        [Test]
        public void IndexStringInViewbag()
        {
            Assert.AreEqual("Home Page", result.ViewBag.Title);
        }
    }
}
