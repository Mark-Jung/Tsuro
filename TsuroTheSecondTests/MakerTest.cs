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
    public class MakerTest
    {
        
        Maker maker = new Maker();

        [TestMethod]
        public void TestMakerN()
        {
            string xmlContent = "<n>3</n>";
            XElement ToNXML = maker.NXML(3);
            Assert.AreEqual(xmlContent, ToNXML.ToString());
        }

        [TestMethod]
        public void TestMakerHV_H()
        {
            string xmlContent = "<h></h>";
            XElement ToHVXML = maker.HVXML("h");
            Assert.AreEqual(xmlContent, ToHVXML.ToString());
        }

        [TestMethod]
        public void TestMakerHV_V()
        {
            string xmlContent = "<v></v>";
            XElement ToHVXML = maker.HVXML("v");
            Assert.AreEqual(xmlContent, ToHVXML.ToString());
        }

        [TestMethod]
        public void TestMakerConnect()
        {
            // expected
            string xmlContent = "<connect>" +
                "<n>3</n>" +
                "<n>4</n>" +
                "</connect>";

            XDocument expected_doc = XDocument.Parse(xmlContent);

            // testing
            XElement connectxml = maker.ConnectXML(3, 4);
            Assert.IsTrue(XNode.DeepEquals(expected_doc.FirstNode, connectxml)); 
        }

        [TestMethod]
        public void TestMakerTile()
        {
            // expected
            string xmlconnect1 = "<connect><n>1</n><n>2</n></connect>";
            string xmlconnect2 = "<connect><n>3</n><n>4</n></connect>";
            string xmlconnect3 = "<connect><n>5</n><n>6</n></connect>";
            string xmlconnect4 = "<connect><n>0</n><n>7</n></connect>";
            string xmltile = "<tile>";
            xmltile += xmlconnect1;
            xmltile += xmlconnect2;
            xmltile += xmlconnect3;
            xmltile += xmlconnect4;
            xmltile += "</tile>";

            XDocument expected_doc = XDocument.Parse(xmltile);

            Tile test_tile = new Tile(1, new List<int> { 1, 2, 3, 4, 5, 6, 0, 7 });

            // testing
            XElement tilexml = maker.TileXML(test_tile);
            Assert.IsTrue(XNode.DeepEquals(expected_doc.FirstNode, tilexml)); 
        }

        [TestMethod]
        public void TestMakerColor(){
            // expected
            string colorxml = "<color>blue</color>";
            XDocument expected_doc = XDocument.Parse(colorxml);
            string color = "blue";
            Assert.IsTrue(XNode.DeepEquals(expected_doc.FirstNode, maker.ColorXML(color)));
        }

        [TestMethod]
        public void TestMakerXY()
        {
            // expected
            string xy = "<xy><x>2</x><y>4</y></xy>";
            XDocument expected_doc = XDocument.Parse(xy);
            Assert.IsTrue(XNode.DeepEquals(expected_doc.FirstNode, maker.XYXML(2, 4)));
        }

        [TestMethod]
        public void TestMakerListofTile()
        {
            //expected
            // (0, 1, 2, 3, 4, 5, 6, 7), id 1
            // (0, 4, 1, 5, 2, 6, 3, 7), id 9
            string tiles = "<tile><connect><n>0</n><n>1</n></connect><connect><n>2</n><n>3</n></connect><connect><n>4</n><n>5</n></connect><connect><n>6</n><n>7</n></connect></tile><tile><connect><n>0</n><n>4</n></connect><connect><n>1</n><n>5</n></connect><connect><n>2</n><n>6</n></connect><connect><n>3</n><n>7</n></connect></tile>";
            string listoftiles = "<list>" + tiles + "</list>";
            XDocument expected_doc = XDocument.Parse(listoftiles);
            List<Tile> ListofTiles = new List<Tile>();
            ListofTiles.Add(new Tile(1, new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 }));
            ListofTiles.Add(new Tile(9, new List<int> { 0, 4, 1, 5, 2, 6, 3, 7 }));
            Assert.IsTrue(XNode.DeepEquals(expected_doc.FirstNode, maker.ListofTilesXML(ListofTiles)));
        }

        [TestMethod]
        public void TestMakerSetofTile()
        {
            //expected
            // (0, 1, 2, 3, 4, 5, 6, 7), id 1
            // (0, 4, 1, 5, 2, 6, 3, 7), id 9
            string tiles = "<tile><connect><n>0</n><n>1</n></connect><connect><n>2</n><n>3</n></connect><connect><n>4</n><n>5</n></connect><connect><n>6</n><n>7</n></connect></tile><tile><connect><n>0</n><n>4</n></connect><connect><n>1</n><n>5</n></connect><connect><n>2</n><n>6</n></connect><connect><n>3</n><n>7</n></connect></tile>";
            string listoftiles = "<set>" + tiles + "</set>";
            XDocument expected_doc = XDocument.Parse(listoftiles);
            List<Tile> ListofTiles = new List<Tile>();
            ListofTiles.Add(new Tile(1, new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 }));
            ListofTiles.Add(new Tile(9, new List<int> { 0, 4, 1, 5, 2, 6, 3, 7 }));
            Assert.IsTrue(XNode.DeepEquals(expected_doc.FirstNode, maker.SetofTilesXML(ListofTiles)));
        }

        [TestMethod]
        public void TestMakerSplayerinDragonQueue()
        {
            //expected
            string DragonSPlayer = "<splayer-dragon>";
            string colorxml = "<color>blue</color>";
            DragonSPlayer += colorxml;
            // (0, 1, 2, 3, 4, 5, 6, 7), id 1
            // (0, 4, 1, 5, 2, 6, 3, 7), id 9
            string tiles = "<tile><connect><n>0</n><n>1</n></connect><connect><n>2</n><n>3</n></connect><connect><n>4</n><n>5</n></connect><connect><n>6</n><n>7</n></connect></tile><tile><connect><n>0</n><n>4</n></connect><connect><n>1</n><n>5</n></connect><connect><n>2</n><n>6</n></connect><connect><n>3</n><n>7</n></connect></tile>";
            string setoftiles = "<set>" + tiles + "</set>";
            DragonSPlayer += setoftiles;
            DragonSPlayer += "</splayer-dragon>";
            XDocument expected_doc = XDocument.Parse(DragonSPlayer);

            RandomPlayer jim = new RandomPlayer("jim");
            Player randBlue = new Player(jim, "blue");
            randBlue.AddTiletoHand(new Tile(1, new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 }));
            randBlue.AddTiletoHand(new Tile(9, new List<int> { 0, 4, 1, 5, 2, 6, 3, 7 }));

            Assert.IsTrue(XNode.DeepEquals(expected_doc.FirstNode, maker.DragonSPlayerXML(randBlue)));
        }

        [TestMethod]
        public void TestMakerSplayerNotinDragonQueue()
        {
            //expected
            string SPlayer = "<splayer-nodragon>";
            string colorxml = "<color>blue</color>";
            SPlayer += colorxml;
            // (0, 1, 2, 3, 4, 5, 6, 7), id 1
            // (0, 4, 1, 5, 2, 6, 3, 7), id 9
            string tiles = "<tile><connect><n>0</n><n>1</n></connect><connect><n>2</n><n>3</n></connect><connect><n>4</n><n>5</n></connect><connect><n>6</n><n>7</n></connect></tile><tile><connect><n>0</n><n>4</n></connect><connect><n>1</n><n>5</n></connect><connect><n>2</n><n>6</n></connect><connect><n>3</n><n>7</n></connect></tile>";
            string setoftiles = "<set>" + tiles + "</set>";
            SPlayer += setoftiles;
            SPlayer += "</splayer-nodragon>";
            XDocument expected_doc = XDocument.Parse(SPlayer);

            RandomPlayer jim = new RandomPlayer("jim");
            Player randBlue = new Player(jim, "blue");
            randBlue.AddTiletoHand(new Tile(1, new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 }));
            randBlue.AddTiletoHand(new Tile(9, new List<int> { 0, 4, 1, 5, 2, 6, 3, 7 }));

            Assert.IsTrue(XNode.DeepEquals(expected_doc.FirstNode, maker.SPlayerXML(randBlue)));
        }

        [TestMethod]
        public void TestMakerListofSplayer()
        {
            string SPlayer = "<splayer-nodragon>";
            string colorxml = "<color>blue</color>";
            SPlayer += colorxml;
            // (0, 1, 2, 3, 4, 5, 6, 7), id 1
            // (0, 4, 1, 5, 2, 6, 3, 7), id 9
            string tiles = "<tile><connect><n>0</n><n>1</n></connect><connect><n>2</n><n>3</n></connect><connect><n>4</n><n>5</n></connect><connect><n>6</n><n>7</n></connect></tile><tile><connect><n>0</n><n>4</n></connect><connect><n>1</n><n>5</n></connect><connect><n>2</n><n>6</n></connect><connect><n>3</n><n>7</n></connect></tile>";
            string setoftiles = "<set>" + tiles + "</set>";
            SPlayer += setoftiles;
            SPlayer += "</splayer-nodragon>";

            string DragonSPlayer = "<splayer-dragon>";
            string dragcolorxml = "<color>red</color>";
            DragonSPlayer += dragcolorxml;
            // (0, 1, 2, 3, 4, 5, 6, 7), id 1
            // (0, 4, 1, 5, 2, 6, 3, 7), id 9
            string dragtiles = "<tile><connect><n>0</n><n>1</n></connect><connect><n>2</n><n>3</n></connect><connect><n>4</n><n>5</n></connect><connect><n>6</n><n>7</n></connect></tile><tile><connect><n>0</n><n>4</n></connect><connect><n>1</n><n>5</n></connect><connect><n>2</n><n>6</n></connect><connect><n>3</n><n>7</n></connect></tile>";
            string dragsetoftiles = "<set>" + dragtiles + "</set>";
            DragonSPlayer += dragsetoftiles;
            DragonSPlayer += "</splayer-dragon>";

            string ListofSPlayer = "<list>" + SPlayer + DragonSPlayer + "</list>";
            XDocument expected_doc = XDocument.Parse(ListofSPlayer);

            // make list of splayers
            RandomPlayer jim = new RandomPlayer("jim");
            Player randBlue = new Player(jim, "blue");
            randBlue.AddTiletoHand(new Tile(1, new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 }));
            randBlue.AddTiletoHand(new Tile(9, new List<int> { 0, 4, 1, 5, 2, 6, 3, 7 }));
            RandomPlayer bob = new RandomPlayer("bob");
            Player randred = new Player(jim, "red");
            randred.AddTiletoHand(new Tile(1, new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 }));
            randred.AddTiletoHand(new Tile(9, new List<int> { 0, 4, 1, 5, 2, 6, 3, 7 }));

            List<(Player, Boolean)> listofSPlayer = new List<(Player, Boolean)> { (randBlue, false), (randred, true)};


            Assert.IsTrue(XNode.DeepEquals(expected_doc.FirstNode, maker.ListofSPlayersXML(listofSPlayer)));
        }

        [TestMethod]
        public void TestMakerFalse(){
            string isFalse = "<false></false>";
            XDocument expected_doc = XDocument.Parse(isFalse);
            Assert.IsTrue(XNode.DeepEquals(expected_doc.FirstNode, maker.FalseXML()));
        }

        [TestMethod]
        public void TestMakerTilesSingle(){
            // xy
            string xy = "<xy><x>2</x><y>4</y></xy>";
            // tile
            string xmlconnect1 = "<connect><n>1</n><n>2</n></connect>";
            string xmlconnect2 = "<connect><n>3</n><n>4</n></connect>";
            string xmlconnect3 = "<connect><n>5</n><n>6</n></connect>";
            string xmlconnect4 = "<connect><n>0</n><n>7</n></connect>";
            string xmltile = "<tile>";
            xmltile += xmlconnect1;
            xmltile += xmlconnect2;
            xmltile += xmlconnect3;
            xmltile += xmlconnect4;
            xmltile += "</tile>";
            // tiles
            string xmltiles = "<map><ent>" + xy + xmltile + "</ent></map>";
            XDocument expected_doc = XDocument.Parse(xmltiles);

            Tile tile1 = new Tile(1, new List<int> { 1, 2, 3, 4, 5, 6, 0, 7 });
            List<Tile> tiles = new List<Tile>();
            tiles.Add(tile1);
            List<(int, int)> locs = new List<(int, int)>();
            locs.Add((2, 4));
            Assert.IsTrue(XNode.DeepEquals(expected_doc.FirstNode, maker.TilesXML( locs, tiles)));
        }

        [TestMethod]
        public void TestMakerPawnLoc(){
            // horv
            string horv = "<h></h>";
            // n, n
            string n1 = "<n>3</n>";
            string n2 = "<n>3</n>";
            string pawnloc = "<pawn-loc>" + horv + n1 + n2 + "</pawn-loc>";
            XDocument expected_doc = XDocument.Parse(pawnloc);

            Position position1 = new Position(1, 3, 1, false);
            Position position2 = new Position(1, 2, 4, false);

            Assert.IsTrue(XNode.DeepEquals(expected_doc.FirstNode, maker.PawnLocXML(position1)));
            Assert.IsTrue(XNode.DeepEquals(expected_doc.FirstNode, maker.PawnLocXML(position2)));

        }

        [TestMethod]
        public void TestMakerPawns() {
            // color
            string color = "<color>red</color>";
            string blue = "<color>blue</color>";
            // horv
            string horv = "<h></h>";
            // n, n
            string n1 = "<n>3</n>";
            string n2 = "<n>3</n>";
            string pawnloc = "<pawn-loc>" + horv + n1 + n2 + "</pawn-loc>";
            string ent = "<ent>" + color + pawnloc + "</ent>";
            string ent1 = "<ent>" + blue + pawnloc + "</ent>";
            string pawns = "<map>" + ent + ent1 + "</map>";

            Position position1 = new Position(1, 3, 1, false);
            Position position2 = new Position(1, 2, 4, false);

            Dictionary<string, Position> tokenPositions = new Dictionary<string, Position>();
            tokenPositions["red"] = position1;
            tokenPositions["blue"] = position2;

            XDocument expected_doc = XDocument.Parse(pawns);
            Assert.IsTrue(XNode.DeepEquals(expected_doc.FirstNode, maker.PawnsXML(tokenPositions)));
        }

        [TestMethod]
        public void TestMakerBoard() {
            // pawns setup
            // color
            string color = "<color>red</color>";
            string blue = "<color>blue</color>";
            // horv
            string horv = "<h></h>";
            // n, n
            string n1 = "<n>3</n>";
            string n2 = "<n>3</n>";
            string pawnloc = "<pawn-loc>" + horv + n1 + n2 + "</pawn-loc>";
            string ent = "<ent>" + color + pawnloc + "</ent>";
            string ent1 = "<ent>" + blue + pawnloc + "</ent>";
            string pawns = "<map>" + ent + ent1 + "</map>";

            Position position1 = new Position(1, 3, 1, false);
            Position position2 = new Position(1, 2, 4, false);

            //tiles setup
            // xy
            string xy = "<xy><x>2</x><y>4</y></xy>";
            // tile
            string xmlconnect1 = "<connect><n>1</n><n>2</n></connect>";
            string xmlconnect2 = "<connect><n>3</n><n>4</n></connect>";
            string xmlconnect3 = "<connect><n>5</n><n>6</n></connect>";
            string xmlconnect4 = "<connect><n>0</n><n>7</n></connect>";
            string xmltile = "<tile>";
            xmltile += xmlconnect1;
            xmltile += xmlconnect2;
            xmltile += xmlconnect3;
            xmltile += xmlconnect4;
            xmltile += "</tile>";
            // tiles
            string xmltiles = "<map><ent>" + xy + xmltile + "</ent></map>";

            string board_str = "<board>" + xmltiles + pawns + "</board>";

            Board board = new Board(6);
            Tile tile1 = new Tile(1, new List<int> { 1, 2, 3, 4, 5, 6, 0, 7 });
            board.PlaceTile(tile1, 2, 4);
            board.AddPlayerToken("red", position1);
            board.AddPlayerToken("blue", position2);

            XDocument expected_doc = XDocument.Parse(board_str);

            Assert.IsTrue(XNode.DeepEquals(expected_doc.FirstNode, maker.BoardXML(board)));
        }

        [TestMethod]
        public void TestMakerToXmlNode(){
            XElement xyXelement = maker.XYXML(3, 4);
            XmlNode newNode = maker.ToXmlNode(xyXelement);
            Parser parser = new Parser();
            // need to return two integers: x and y(3, 4)
            (int x, int y) = parser.XYXML(newNode);
            Assert.AreEqual(3, x);
            Assert.AreEqual(4, y);
        }
    }
}
