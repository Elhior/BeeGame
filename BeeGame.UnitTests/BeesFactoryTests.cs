using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bee_game.Models;
using NUnit.Framework;

namespace BeeGame.UnitTests
{
    [TestFixture]
    public class BeesFactoryTests
    {
        [Test]
        public static void CreateBeeNotNull()
        {
            IBee result = BeesFactory.CreateBee("Queen",20,4);

            Assert.IsNotNull(result);
        }

        [Test]
        public static void CreateBee_ExpectedQueen()
        {
            IBee result = BeesFactory.CreateBee("Queen", 20, 4);

            IBee expected = new Queen("Queen", 20, 4);

            Assert.AreEqual(result.GetType(), expected.GetType());
        }

        [Test]
        public static void CreateBee_ExpectedDrone()
        {
            IBee result = BeesFactory.CreateBee("Drone", 20, 4);

            IBee expected = new Drone("Drone", 20, 4);

            Assert.AreEqual(result.GetType(), expected.GetType());
        }
    }
}
