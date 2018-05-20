using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using TsuroTheSecond;

namespace TsuroTheSecondTests
{
    [TestClass]
    public class TileTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "There must be 8 unique ports specified")]
        public void TestConstructorTooManyPorts()
        {
            Tile testTile1 = new Tile(1, new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "There must be 8 unique ports specified")]
        public void TestConstructorTooFewPorts()
        {
            Tile testTile1 = new Tile(1, new List<int> { 0, 1, 2, 3, 4, 5, 6 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Ports must be in range 0 to 7")]
        public void TestConstructorPathValuesTooGreat()
        {
            Tile testTile1 = new Tile(1, new List<int> { 0, 1, 2, 3, 4, 5, 6, 9});
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Ports must be in range 0 to 7")]
        public void TestConstructorPathValuesTooSmall()
        {
            Tile testTile1 = new Tile(1, new List<int> { 0, 1, 2, 3, 4, 5, 6, -7 });
        }

        [TestMethod]
        public void TestFindEndofPath()
        {
            List<int> paths = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 };
            Tile testTile1 = new Tile(1, paths);

            for (int i = 0; i < 8; i+=2) {
                int endFirst = testTile1.FindEndofPath(paths[i]);
                int endSecond = testTile1.FindEndofPath(paths[i + 1]);
                Assert.AreEqual(paths[i + 1], endFirst);
                Assert.AreEqual(paths[i], endSecond);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Start is out of range")]
        public void TestFindEndofPathStartTooBig()
        {
            Tile testTile1 = new Tile(1, new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 });
            int end = testTile1.FindEndofPath(9);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Start is out of range")]
        public void TestFindEndofPathStartTooSmall()
        {
            Tile testTile1 = new Tile(1, new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 });
            int end = testTile1.FindEndofPath(-1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Port doesn't exist in tile")]
        public void TestFindEndofPathPortDoesntExist()
        {
            Tile testTile1 = new Tile(1, new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 });
            testTile1.paths[0][0] = 2;
            int end = testTile1.FindEndofPath(0);
            Console.WriteLine(end);
        }

        [TestMethod]
        public void TestRotate()
        {
            Tile testTile1 = new Tile(1, new List<int>{0, 1, 2, 3, 4, 5, 6, 7});
            testTile1.Rotate();
            Tile testTile2 = new Tile(2, new List<int>{2, 3, 4, 5, 6, 7, 0, 1});
            for (int i = 0; i < 4; i++) {
                int tile1_ent_1 = testTile1.paths[i][0];
                Assert.AreEqual(testTile1.FindEndofPath(tile1_ent_1), testTile2.FindEndofPath(tile1_ent_1));
                int tile1_ent_2 = testTile1.paths[i][1];
                Assert.AreEqual(testTile1.FindEndofPath(tile1_ent_2), testTile2.FindEndofPath(tile1_ent_2));
            }
        }

        [TestMethod]
        public void TestJudgeSymmetric1()
        {
            Tile testTile1 = new Tile(1, new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 });
            Tile testTile2 = new Tile(2, new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 });

            testTile1.JudgeSymmetric();
            Assert.AreEqual(1, testTile1.symmetricity);
            Assert.AreEqual(testTile1.PathMap(), testTile2.PathMap());
        }

        [TestMethod]
        public void TestJudgeSymmetric4()
        {
            Tile testTile1 = new Tile(1, new List<int> { 0, 5, 1, 3, 2, 6, 4, 7 });
            Tile testTile2 = new Tile(2, new List<int> { 0, 5, 1, 3, 2, 6, 4, 7 });

            testTile1.JudgeSymmetric();
            Assert.AreEqual(4, testTile1.symmetricity);
            Assert.AreEqual(testTile1.PathMap(), testTile2.PathMap());

        }

        [TestMethod]
        public void TestCompareByPath_t()
        {
            Tile testTile1 = new Tile(1, new List<int> { 0, 5, 1, 3, 2, 6, 4, 7 });
            Tile testTile2 = new Tile(2, new List<int> { 1, 3, 2, 6, 4, 7, 0, 5 });

            Assert.IsTrue(testTile1.CompareByPath(testTile2));
            Assert.IsTrue(testTile2.CompareByPath(testTile1));

        }

        [TestMethod]
        public void TestCompareByPath_f()
        {
            Tile testTile1 = new Tile(1, new List<int> { 0, 5, 1, 3, 2, 6, 4, 7 });
            Tile testTile2 = new Tile(2, new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 });
            Assert.IsFalse(testTile1.CompareByPath(testTile2));
            Assert.IsFalse(testTile2.CompareByPath(testTile1));
        }
        [TestMethod]
        public void TestIsDifferent()
        {
            Tile testTile1 = new Tile(1, new List<int> { 0, 4, 1, 5, 2, 7, 3, 6 });
            Tile testTile2 = new Tile(2, new List<int> { 2, 6, 3, 7, 4, 1, 5, 0 });
            Assert.IsFalse(testTile1.IsDifferent(testTile2));
        }
    }
}
