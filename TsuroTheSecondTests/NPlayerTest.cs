using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml;
using TsuroTheSecond;
using System.Linq;


namespace TsuroTheSecondTests
{
    [TestClass]
    public class NPlayerTest
    {
        Wrapper wrapper = new Wrapper();
        Maker maker = new Maker();
        Parser parser = new Parser();
        Server server;
        RandomPlayer randomPlayer;
        LeastSymmetricPlayer leastSymmetricPlayer;
        MostSymmetricPlayer mostSymmetricPlayer;
        List<Tile> tiles = new List<Tile>();
        Player randBlue;
        Player mostsymRed;
        Player leastsymRed;
        NPlayer nPlayer;

        [TestInitialize]
        public void Initialize()
        {
            server = new Server();
            randomPlayer = new RandomPlayer("jim");
            leastSymmetricPlayer = new LeastSymmetricPlayer("reggie");
            mostSymmetricPlayer = new MostSymmetricPlayer("michael");

            randBlue = new Player(randomPlayer, "blue");
            mostsymRed = new Player(mostSymmetricPlayer, "red");
            leastsymRed = new Player(leastSymmetricPlayer, "red");


            tiles = new List<Tile>{
                new Tile(1, new List<int>{0, 1, 2, 3, 4, 5, 6, 7}),
                new Tile(2, new List<int>{0, 1, 2, 4, 3, 6, 5, 7}),
                new Tile(3, new List<int>{0, 6, 1, 5, 2, 4, 3, 7}),
                new Tile(4, new List<int>{0, 5, 1, 4, 2, 7, 3, 6}),
                new Tile(5, new List<int>{0, 2, 1, 4, 3, 7, 5, 6}),
            };
            //nPlayer = new NPlayer("Jim", );
        }

        //[TestMethod]
        //public void TestGetName()
        //{
        //    XElement playernameXML = maker.PlayerNameXML("Mark");
        //    string playername = playernameXML.ToString();
        //    Console.SetIn(new StringReader(playername));
        //    string received = nPlayer.GetName();
        //    Assert.AreEqual("Mark", received);
        //}

        //[TestMethod]
        //public void TestInitialize()
        //{
        //    Console.SetIn(new StringReader("<void></void>"));
        //    nPlayer.Initialize("blue", new List<string> { "blue", "green" });
        //}

        //[TestMethod]
        //public void TestPlacePawn()
        //{
        //    nPlayer.playerState = NPlayer.State.initialized;
        //    XmlElement pawnloc = maker.ToXmlElement(maker.PawnLocXML(new Position(0, -1, 4)));
        //    Console.SetIn(new StringReader(pawnloc.OuterXml));
        //    Position recv = nPlayer.PlacePawn(new Board(6));
        //    Assert.AreEqual(new Position(0, -1, 4), recv);
        //}

        //[TestMethod]
        //public void TestPlayTurn()
        //{
        //    nPlayer.playerState = NPlayer.State.loop;
        //    XmlElement tile = maker.ToXmlElement(maker.TileXML(tiles[1]));
        //    Console.SetIn(new StringReader(tile.OuterXml));
        //    Tile recv = nPlayer.PlayTurn(new Board(6), tiles, 23);
        //    Assert.IsTrue(!recv.IsDifferent(tiles[1]));
        //}

        //[TestMethod]
        //public void TestEndGame()
        //{
        //    nPlayer.playerState = NPlayer.State.loop;
        //    Console.SetIn(new StringReader("<void></void>"));
        //    nPlayer.EndGame(new Board(6), new List<string> { "blue", "green" });
        //}
    }
}


