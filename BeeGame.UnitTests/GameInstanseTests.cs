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
    public class GameInstanseTests
    {
        //ЧтоТестируем_СостояниеТеста_ОжидаемоеПоведение 
        [Test]
        public static void GetNewGameId_Expected0()
        {
            int expected = 0;
            int result = GameInstance.GetNewGameId();

            Assert.AreEqual(expected, expected);
        }
        [Test]
        public static void Test()
        {
            int expected = 2;
            int result = GameInstance.GetNewGameId();

            Assert.AreEqual(result, expected);
        }
    }
}
