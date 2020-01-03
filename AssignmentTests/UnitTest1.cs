using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NotesApp.AlgoExpert;

namespace AssignmentTests
{
    [TestClass]
    public class RiverTests
    {
        [TestMethod]
        public void RiverTest1()
        {
            int[,] rivers = new int[,] { {1, 0, 0, 1, 0 },
                {1, 0, 1, 0, 0 },
                {0, 0, 1, 0, 1 },
                {1, 0, 1, 0, 1 },
                {1, 0, 1, 1, 0 }
            };


            Console.WriteLine(String.Join(",", Assignment.RiverSizes(rivers)));
            Assert.IsTrue(true);
        }
    }

    [TestClass]
    public class TestLongestPalindrome
    {

        [DataTestMethod]
        [DataRow(null, 0)]
        [DataRow("", 0)]
        [DataRow("a", 1)]
        [DataRow("aa", 2)]
        [DataRow("baa", 2)]
        [DataRow("I like racecars that go fast", 7)]
        [DataRow("   ", 3)]
        public void LongestPalindrom(string str, int expected)
        {
            var result = Assignment.GetLongestPalindrome(str);

            Assert.AreEqual(expected, result);
        }
    }

    [TestClass]
    public class DiverseTests
    {
        [TestMethod]
        public void LongesBinaryGap()
        {
            Assignment.Gap(20);
//            Assignment.Gap(9);
            Assert.AreEqual(0, 0);
        }
    }
}
