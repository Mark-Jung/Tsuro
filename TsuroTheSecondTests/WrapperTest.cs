using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml;
using TsuroTheSecond;
using System.Linq;


namespace TsuroTheSecondTests
{
    [TestClass]
    public class WrapperTest
    {
        Wrapper wrapper = new Wrapper();
        Server server;
        RandomPlayer randomPlayer;
        LeastSymmetricPlayer leastSymmetricPlayer;
        MostSymmetricPlayer mostSymmetricPlayer;
        List<Tile> tiles = new List<Tile>();
        Player randBlue;

        [TestInitialize]
        public void Initialize()
        {
            server = new Server();
            randomPlayer = new RandomPlayer("jim");
            leastSymmetricPlayer= new LeastSymmetricPlayer("reggie");
            mostSymmetricPlayer= new MostSymmetricPlayer("michael");

            randBlue = new Player(randomPlayer, "blue");
            tiles = new List<Tile>{
                new Tile(1, new List<int>{0, 1, 2, 3, 4, 5, 6, 7}),
                new Tile(2, new List<int>{0, 1, 2, 4, 3, 6, 5, 7}),
                new Tile(3, new List<int>{0, 6, 1, 5, 2, 4, 3, 7}),
                new Tile(4, new List<int>{0, 5, 1, 4, 2, 7, 3, 6}),
                new Tile(5, new List<int>{0, 2, 1, 4, 3, 7, 5, 6}),
            };
        }


        [TestMethod]
        public void TestGetNameWrapper()
        {
            string expected = "<player-name>jim</player-name>";
            XmlNode output_node = wrapper.GetName(randBlue);
            Assert.AreEqual(expected, output_node.OuterXml);
        }

    }
}

