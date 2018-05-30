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
        Maker maker = new Maker();
        Server server;
        RandomPlayer randomPlayer;
        LeastSymmetricPlayer leastSymmetricPlayer;
        MostSymmetricPlayer mostSymmetricPlayer;
        List<Tile> tiles = new List<Tile>();
        PlayerProxy randBlue;
        PlayerProxy mostsymRed;
        PlayerProxy leastsymRed;

        [TestInitialize]
        public void Initialize()
        {
            server = new Server();
            randomPlayer = new RandomPlayer("jim");
            leastSymmetricPlayer= new LeastSymmetricPlayer("reggie");
            mostSymmetricPlayer= new MostSymmetricPlayer("michael");

            randBlue = new PlayerProxy(randomPlayer, "blue", 9999);
            mostsymRed = new PlayerProxy(mostSymmetricPlayer, "red", 9999);
            leastsymRed = new PlayerProxy(leastSymmetricPlayer, "red", 9999);


            tiles = new List<Tile>{
                new Tile(1, new List<int>{0, 1, 2, 3, 4, 5, 6, 7}),
                new Tile(2, new List<int>{0, 1, 2, 4, 3, 6, 5, 7}),
                new Tile(3, new List<int>{0, 6, 1, 5, 2, 4, 3, 7}),
                new Tile(4, new List<int>{0, 5, 1, 4, 2, 7, 3, 6}),
                new Tile(5, new List<int>{0, 2, 1, 4, 3, 7, 5, 6}),
            };
        }

        //[TestMethod]
        //public void TestGetNameWrapper()
        //{
        //    string expected = "<player-name>jim</player-name>";
        //    XmlNode output_node = wrapper.GetName(randBlue);
        //    Assert.AreEqual(expected, output_node.OuterXml);
        //}

        //[TestMethod]
        //public void TestInitializeWrapper()
        //{
        //    string input = "<initialize><color>blue</color><list><color>red</color><color>blue</color><color>green</color></list></initialize>";
        //    string expected = "<void></void>";
        //    XmlDocument xmlDocument = new XmlDocument();
        //    xmlDocument.LoadXml(input);
        //    XmlNode newNode = xmlDocument.DocumentElement;
        //    XmlNode output_node = wrapper.Initialize(randBlue, newNode);
        //    Assert.AreEqual(expected, output_node.OuterXml);
        //    // check if it was effective
        //    Assert.AreEqual(randomPlayer.GetColor(), "blue");
        //    CollectionAssert.AreEqual(randomPlayer.GetOtherColors(), new List<string> { "red", "blue", "green" });
        //}

        //[TestMethod]
        //[ExpectedException(typeof(ArgumentException),
        //    "Color is inconsistent")]
        //public void TestInitializeWrapperException()
        //{
        //    string input = "<initialize><color>red</color><list><color>red</color><color>blue</color><color>green</color></list></initialize>";
        //    //string expected = "<void></void>";
        //    XmlDocument xmlDocument = new XmlDocument();
        //    xmlDocument.LoadXml(input);
        //    XmlNode newNode = xmlDocument.DocumentElement;
        //    XmlNode output_node = wrapper.Initialize(randBlue, newNode);
        //}

        //[TestMethod]
        //public void TestPlacePawn(){
        //    string xmlContent = "<place-pawn><board>";
        //    string Tiles = "<map><ent><xy><x>3</x><y>4</y></xy><tile><connect><n>0</n><n>1</n></connect><connect><n>2</n><n>3</n></connect><connect><n>4</n><n>5</n></connect><connect><n>6</n><n>7</n></connect></tile></ent><ent><xy><x><n>4</n></x><y><n>4</n></y></xy><tile><connect><n>0</n><n>4</n></connect><connect><n>1</n><n>5</n></connect><connect><n>2</n><n>6</n></connect><connect><n>3</n><n>7</n></connect></tile></ent></map>";
        //    string Pawns = "<map><ent><color>red</color><pawn-loc><h></h><n>0</n><n>6</n></pawn-loc></ent> <ent><color>green</color><pawn-loc><v></v><n>0</n><n>2</n></pawn-loc></ent></map>";
        //    xmlContent += Tiles;
        //    xmlContent += Pawns;
        //    xmlContent += "</board></place-pawn>";
        //    // input is place-pawn, which holds board
        //    string input = xmlContent;
        //    // expected is pawn-loc
        //    string expected = "<pawn-loc><h></h><n>0</n><n>0</n></pawn-loc>";
        //    XmlDocument xmlDocument = new XmlDocument();
        //    xmlDocument.LoadXml(input);
        //    XmlNode newNode = xmlDocument.DocumentElement;
        //    randomPlayer.Initialize(randBlue.Color, new List<string> { "red", "blue", "green" });
        //    XmlNode outputNode = wrapper.PlacePawn(randBlue, newNode);
        //    Assert.AreEqual(expected, outputNode.OuterXml);
        //}

        //[TestMethod]
        //public void TestPlayTurn()
        //{
        //    string playturn = "<play-turn>";
        //    // board
        //    string board = "<board>";
        //    string Tiles = "<map><ent><xy><x>0</x><y>0</y></xy><tile><connect><n>0</n><n>5</n></connect><connect><n>1</n><n>4</n></connect><connect><n>2</n><n>7</n></connect><connect><n>3</n><n>6</n></connect></tile></ent></map>";
        //    string Pawns = "<map><ent><color>blue</color><pawn-loc><h></h><n>1</n><n>0</n></pawn-loc></ent> <ent><color>red</color><pawn-loc><v></v><n>1</n><n>0</n></pawn-loc></ent></map>";
        //    board += Tiles;
        //    board += Pawns;
        //    board += "</board>";
        //    playturn += board;
        //    // set of tile
        //    // (0, 1, 2, 3, 4, 5, 6, 7), id 1
        //    // (0, 4, 1, 5, 2, 6, 3, 7), id 9
        //    string tiless = "<tile><connect><n>0</n><n>1</n></connect><connect><n>2</n><n>3</n></connect><connect><n>4</n><n>5</n></connect><connect><n>6</n><n>7</n></connect></tile><tile><connect><n>0</n><n>4</n></connect><connect><n>1</n><n>5</n></connect><connect><n>2</n><n>6</n></connect><connect><n>3</n><n>7</n></connect></tile>";
        //    string setoftile = "<set>" + tiless + "</set>";
        //    playturn += setoftile;
        //    // n
        //    string n1 = "<n>27</n>";
        //    playturn += n1;
        //    playturn += "</play-turn>";
        //    Tile ans_tile = new Tile(1, new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 });
        //    Tile ans_tile2 = new Tile(9, new List<int> { 0, 4, 1, 5, 2, 6, 3, 7 });
        //    // expected is tile
        //    string expected = maker.ToXmlNode(maker.TileXML(ans_tile2)).OuterXml;
        //    XmlDocument xmlDocument = new XmlDocument();
        //    xmlDocument.LoadXml(playturn);
        //    XmlNode newNode = xmlDocument.DocumentElement;
        //    leastSymmetricPlayer.playerState = LeastSymmetricPlayer.State.loop;
        //    leastSymmetricPlayer.color = "blue";
        //    XmlNode output = wrapper.PlayTurn(leastsymRed, newNode);
        //    Assert.AreEqual(expected, output.OuterXml);
        //}
        //[TestMethod]
        //public void TestEndGame()
        //{
        //    string endgame = "<end-game>";
        //    string board = "<board>";
        //    string Tiles = "<map><ent><xy><x>0</x><y>0</y></xy><tile><connect><n>0</n><n>5</n></connect><connect><n>1</n><n>4</n></connect><connect><n>2</n><n>7</n></connect><connect><n>3</n><n>6</n></connect></tile></ent></map>";
        //    string Pawns = "<map><ent><color>blue</color><pawn-loc><h></h><n>1</n><n>0</n></pawn-loc></ent><ent><color>red</color><pawn-loc><v></v><n>1</n><n>0</n></pawn-loc></ent></map>";
        //    board += Tiles;
        //    board += Pawns;
        //    board += "</board>";
        //    endgame += board;
        //    string colors = "<color>blue</color><color>green</color><color>red</color>";
        //    string setofcolors = "<set>" + colors + "</set>";
        //    endgame += setofcolors;
        //    endgame += "</end-game>";
        //    XmlDocument xmlDocument = new XmlDocument();
        //    xmlDocument.LoadXml(endgame);
        //    XmlNode newNode = xmlDocument.DocumentElement;
        //    randomPlayer.color = "blue";
        //    randomPlayer.playerState = RandomPlayer.State.loop;
        //    XmlNode output = randBlue.Identifier(newNode);
        //    Assert.AreEqual("<void></void>", output.OuterXml);
        //}
    }
}

