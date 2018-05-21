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
        public void TestMakerPawnLocH()
        {
            // expected
            string xmlContent = "<pawn-loc>" +
                "<h></h>" +
                "<n>3</n>" +
                "<n>4</n>" +
                "</pawn-loc>";

            XDocument expected_doc = XDocument.Parse(xmlContent);

            // testing
            XElement ToPawnLocH = maker.PawnLocXML("h", 3, 4);
            Assert.IsTrue(XNode.DeepEquals(expected_doc.FirstNode, ToPawnLocH));
        }

        [TestMethod]
        public void TestMakerPawnLocV()
        {
            // expected
            string xmlContent = "<pawn-loc>" +
                "<v></v>" +
                "<n>3</n>" +
                "<n>4</n>" +
                "</pawn-loc>";

            XDocument expected_doc = XDocument.Parse(xmlContent);

            // testing
            XElement ToPawnLocV = maker.PawnLocXML("v", 3, 4);
            Assert.IsTrue(XNode.DeepEquals(expected_doc.FirstNode, ToPawnLocV));
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

            // testing
            XElement tilexml = maker.TileXML(1, 2, 3, 4, 5, 6, 0, 7);
            Assert.IsTrue(XNode.DeepEquals(expected_doc.FirstNode, tilexml)); 
        }


        ////[TestMethod]
        ////public void TestParserBadTile()
        ////{
        ////    string xmlContent = "<tile><connect><n>0</n><n>7</n></connect><connect><n>1</n><n>2</n></connect><connect><n>4</n><n>6</n></connect><connect><n>5</n><n>3</n></connect></tile>";
        ////    XmlDocument doc = new XmlDocument();
        ////    doc.LoadXml(xmlContent);
        ////    XmlNode newNode = doc.DocumentElement;
        ////    Parser parser = new Parser();
        ////    // will return a tile with the paths and the id.
        ////    // if the tile with that path doesn't exit, the id should be -1
        ////    Tile tile = parser.TileXML(newNode);
        ////    Assert.AreEqual(-1, tile.id);
        ////}
        //[TestMethod]
        //public void TestParserXY()
        //{
        //    string xmlContent = "<xy><x><n>3</n></x><y><n>4</n></y></xy>";
        //    XmlDocument doc = new XmlDocument();
        //    doc.LoadXml(xmlContent);
        //    XmlNode newNode = doc.DocumentElement;
        //    Parser parser = new Parser();
        //    // need to return two integers: x and y(3, 4)
        //    (int x, int y) = parser.XYXML(newNode);
        //    Assert.AreEqual(3, x);
        //    Assert.AreEqual(4, y);
        //}
        //[TestMethod]
        //public void TestParserTilesOne()
        //{
        //    string xmlContent = "<map><ent><xy><x><n>3</n></x><y><n>4</n></y></xy><tile><connect><n>0</n><n>1</n></connect><connect><n>2</n><n>3</n></connect><connect><n>4</n><n>5</n></connect><connect><n>6</n><n>7</n></connect></tile></ent></map>";
        //    XmlDocument doc = new XmlDocument();
        //    doc.LoadXml(xmlContent);
        //    XmlNode newNode = doc.DocumentElement;
        //    Parser parser = new Parser();
        //    // 3, 4, (0, 1, 2, 3 ,4 ,5 , 6, 7)
        //    Dictionary<(int, int), Tile> result = parser.TilesXML(newNode);
        //    Tile ans_tile = new Tile(1, new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 });
        //    Assert.AreEqual(1, result.Count);
        //    Assert.AreEqual(1, result[(3, 4)].id);
        //    Assert.IsTrue(ans_tile.CompareByPath(result[(3, 4)]));
        //}
        //[TestMethod]
        //public void TestParserTilesMulti()
        //{
        //    string xmlContent = "<map><ent><xy><x><n>3</n></x><y><n>4</n></y></xy><tile><connect><n>0</n><n>1</n></connect><connect><n>2</n><n>3</n></connect><connect><n>4</n><n>5</n></connect><connect><n>6</n><n>7</n></connect></tile></ent><ent><xy><x><n>4</n></x><y><n>4</n></y></xy><tile><connect><n>0</n><n>4</n></connect><connect><n>1</n><n>5</n></connect><connect><n>2</n><n>6</n></connect><connect><n>3</n><n>7</n></connect></tile></ent></map>";
        //    XmlDocument doc = new XmlDocument();
        //    doc.LoadXml(xmlContent);
        //    XmlNode newNode = doc.DocumentElement;
        //    Parser parser = new Parser();
        //    // 3, 4 (0, 1, 2, 3, 4, 5, 6, 7), id 1
        //    // 4, 4 (0, 4, 1, 5, 2, 6, 3, 7), id 9
        //    Dictionary<(int, int), Tile> result = parser.TilesXML(newNode);
        //    Tile ans_tile = new Tile(1, new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 });
        //    Tile ans_tile2 = new Tile(9, new List<int> { 0, 4, 1, 5, 2, 6, 3, 7 });
        //    Assert.AreEqual(2, result.Count);
        //    Assert.AreEqual(1, result[(3, 4)].id);
        //    Assert.AreEqual(9, result[(4, 4)].id);
        //    Assert.IsTrue(ans_tile.CompareByPath(result[(3, 4)]));
        //    Assert.IsTrue(ans_tile2.CompareByPath(result[(4, 4)]));
        //}
        //[TestMethod]
        //public void TestParserBoard()
        //{
        //    string xmlContent = "<board>";
        //    string Tiles = "<map><ent><xy><x><n>3</n></x><y><n>4</n></y></xy><tile><connect><n>0</n><n>1</n></connect><connect><n>2</n><n>3</n></connect><connect><n>4</n><n>5</n></connect><connect><n>6</n><n>7</n></connect></tile></ent><ent><xy><x><n>4</n></x><y><n>4</n></y></xy><tile><connect><n>0</n><n>4</n></connect><connect><n>1</n><n>5</n></connect><connect><n>2</n><n>6</n></connect><connect><n>3</n><n>7</n></connect></tile></ent></map>";
        //    string Pawns = "<map><ent><color>blue</color><pawn-loc><h></h><n>3</n><n>4</n></pawn-loc></ent> <ent><color>red</color><pawn-loc><v></v><n>4</n><n>3</n></pawn-loc></ent></map>";
        //    xmlContent += Tiles;
        //    xmlContent += Pawns;
        //    xmlContent += "</board>";
        //    XmlDocument doc = new XmlDocument();
        //    doc.LoadXml(xmlContent);
        //    XmlNode newNode = doc.DocumentElement;
        //    Parser parser = new Parser();
        //    // returns tiles to be placed on the board : Dictionary key of position(int x, int y) and value of tiles
        //    // returns tokens position: Dictionary key of color(string) and value of (Position, Position)
        //    (Dictionary<(int, int), Tile> TilesTobePlaced, Dictionary<string, (Position, Position)> tokenPosition) = parser.BoardXML(newNode);
        //    Tile ans_tile = new Tile(1, new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 });
        //    Tile ans_tile2 = new Tile(9, new List<int> { 0, 4, 1, 5, 2, 6, 3, 7 });
        //    Assert.AreEqual(2, TilesTobePlaced.Count);
        //    Assert.AreEqual(1, TilesTobePlaced[(3, 4)].id);
        //    Assert.AreEqual(9, TilesTobePlaced[(4, 4)].id);
        //    Assert.IsTrue(ans_tile.CompareByPath(TilesTobePlaced[(3, 4)]));
        //    Assert.IsTrue(ans_tile2.CompareByPath(TilesTobePlaced[(4, 4)]));
        //    Assert.AreEqual(2, tokenPosition.Count);
        //    Assert.AreEqual(new Position(2, 2, 5, false), tokenPosition["blue"].Item1);
        //    Assert.AreEqual(new Position(2, 3, 0, false), tokenPosition["blue"].Item2);
        //    Assert.AreEqual(new Position(3, 1, 3, false), tokenPosition["red"].Item1);
        //    Assert.AreEqual(new Position(4, 1, 6, false), tokenPosition["red"].Item2);
        //}
        //[TestMethod]
        //public void TestParserPlacePawn()
        //{
        //    string xmlContent = "<place-pawn><board>";
        //    string Tiles = "<map><ent><xy><x><n>3</n></x><y><n>4</n></y></xy><tile><connect><n>0</n><n>1</n></connect><connect><n>2</n><n>3</n></connect><connect><n>4</n><n>5</n></connect><connect><n>6</n><n>7</n></connect></tile></ent><ent><xy><x><n>4</n></x><y><n>4</n></y></xy><tile><connect><n>0</n><n>4</n></connect><connect><n>1</n><n>5</n></connect><connect><n>2</n><n>6</n></connect><connect><n>3</n><n>7</n></connect></tile></ent></map>";
        //    string Pawns = "<map><ent><color>blue</color><pawn-loc><h></h><n>3</n><n>4</n></pawn-loc></ent> <ent><color>red</color><pawn-loc><v></v><n>4</n><n>3</n></pawn-loc></ent></map>";
        //    xmlContent += Tiles;
        //    xmlContent += Pawns;
        //    xmlContent += "</board></place-pawn>";
        //    XmlDocument doc = new XmlDocument();
        //    doc.LoadXml(xmlContent);
        //    XmlNode newNode = doc.DocumentElement;
        //    Parser parser = new Parser();
        //    // returns tiles to be placed on the board : Dictionary key of position(int x, int y) and value of tiles
        //    // returns tokens position: Dictionary key of color(string) and value of (Position, Position)
        //    (Dictionary<(int, int), Tile> TilesTobePlaced, Dictionary<string, (Position, Position)> tokenPosition) = parser.PlacePawnXML(newNode);
        //    Tile ans_tile = new Tile(1, new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 });
        //    Tile ans_tile2 = new Tile(9, new List<int> { 0, 4, 1, 5, 2, 6, 3, 7 });
        //    Assert.AreEqual(2, TilesTobePlaced.Count);
        //    Assert.AreEqual(1, TilesTobePlaced[(3, 4)].id);
        //    Assert.AreEqual(9, TilesTobePlaced[(4, 4)].id);
        //    Assert.IsTrue(ans_tile.CompareByPath(TilesTobePlaced[(3, 4)]));
        //    Assert.IsTrue(ans_tile2.CompareByPath(TilesTobePlaced[(4, 4)]));
        //    Assert.AreEqual(2, tokenPosition.Count);
        //    Assert.AreEqual(new Position(2, 2, 5, false), tokenPosition["blue"].Item1);
        //    Assert.AreEqual(new Position(2, 3, 0, false), tokenPosition["blue"].Item2);
        //    Assert.AreEqual(new Position(3, 1, 3, false), tokenPosition["red"].Item1);
        //    Assert.AreEqual(new Position(4, 1, 6, false), tokenPosition["red"].Item2);
        //}
        //[TestMethod]
        //public void TestParserSetofTile()
        //{
        //    // (0, 1, 2, 3, 4, 5, 6, 7), id 1
        //    // (0, 4, 1, 5, 2, 6, 3, 7), id 9
        //    string tiles = "<tile><connect><n>0</n><n>1</n></connect><connect><n>2</n><n>3</n></connect><connect><n>4</n><n>5</n></connect><connect><n>6</n><n>7</n></connect></tile><tile><connect><n>0</n><n>4</n></connect><connect><n>1</n><n>5</n></connect><connect><n>2</n><n>6</n></connect><connect><n>3</n><n>7</n></connect></tile>";
        //    string setoftile = "<set>" + tiles + "</set>";
        //    XmlDocument doc = new XmlDocument();
        //    doc.LoadXml(setoftile);
        //    XmlNode newNode = doc.DocumentElement;
        //    Parser parser = new Parser();
        //    HashSet<Tile> result = parser.SetofTilesXML(newNode);
        //    Tile ans_tile = new Tile(1, new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 });
        //    Tile ans_tile2 = new Tile(9, new List<int> { 0, 4, 1, 5, 2, 6, 3, 7 });
        //    List<Tile> ans_tiles = new List<Tile>();
        //    ans_tiles.Add(ans_tile);
        //    ans_tiles.Add(ans_tile2);
        //    Assert.AreEqual(2, result.Count);
        //    List<Tile> result_list = result.ToList();
        //    Boolean good = false;
        //    for (int j = 0; j < ans_tiles.Count; j++)
        //    {
        //        good = false;
        //        for (int i = 0; i < result_list.Count; i++)
        //        {
        //            if (!result_list[i].IsDifferent(ans_tiles[j]))
        //            {
        //                good = true;
        //                break;
        //            }
        //        }
        //        Assert.IsTrue(good);
        //    }
        //}
        //[TestMethod]
        //public void TestParserSetofTilesDupli()
        //{
        //    // (0, 1, 2, 3, 4, 5, 6, 7), id 1
        //    // (0, 4, 1, 5, 2, 6, 3, 7), id 9
        //    string tiles = "<tile><connect><n>0</n><n>1</n></connect><connect><n>2</n><n>3</n></connect><connect><n>4</n><n>5</n></connect><connect><n>6</n><n>7</n></connect></tile><tile><connect><n>0</n><n>4</n></connect><connect><n>1</n><n>5</n></connect><connect><n>2</n><n>6</n></connect><connect><n>3</n><n>7</n></connect></tile><tile><connect><n>0</n><n>4</n></connect><connect><n>1</n><n>5</n></connect><connect><n>2</n><n>6</n></connect><connect><n>3</n><n>7</n></connect></tile>";
        //    string setoftile = "<set>" + tiles + "</set>";
        //    XmlDocument doc = new XmlDocument();
        //    doc.LoadXml(setoftile);
        //    XmlNode newNode = doc.DocumentElement;
        //    Parser parser = new Parser();
        //    HashSet<Tile> result = parser.SetofTilesXML(newNode);
        //    Assert.AreEqual(2, result.Count);

        //}

        //[TestMethod]
        //public void TestParserPlayTurn4Num()
        //{
        //    string playturn = "<play-turn>";
        //    // board
        //    string board = "<board>";
        //    string Tiles = "<map><ent><xy><x><n>3</n></x><y><n>4</n></y></xy><tile><connect><n>0</n><n>1</n></connect><connect><n>2</n><n>3</n></connect><connect><n>4</n><n>5</n></connect><connect><n>6</n><n>7</n></connect></tile></ent><ent><xy><x><n>4</n></x><y><n>4</n></y></xy><tile><connect><n>0</n><n>4</n></connect><connect><n>1</n><n>5</n></connect><connect><n>2</n><n>6</n></connect><connect><n>3</n><n>7</n></connect></tile></ent></map>";
        //    string Pawns = "<map><ent><color>blue</color><pawn-loc><h></h><n>3</n><n>4</n></pawn-loc></ent> <ent><color>red</color><pawn-loc><v></v><n>4</n><n>3</n></pawn-loc></ent></map>";
        //    board += Tiles;
        //    board += Pawns;
        //    board += "</board>";
        //    playturn += board;
        //    // set of tile
        //    // (0, 1, 2, 3, 4, 5, 6, 7), id 1
        //    // (0, 4, 1, 5, 2, 6, 3, 7), id 9
        //    string tiles = "<tile><connect><n>0</n><n>1</n></connect><connect><n>2</n><n>3</n></connect><connect><n>4</n><n>5</n></connect><connect><n>6</n><n>7</n></connect></tile><tile><connect><n>0</n><n>4</n></connect><connect><n>1</n><n>5</n></connect><connect><n>2</n><n>6</n></connect><connect><n>3</n><n>7</n></connect></tile>";
        //    string setoftile = "<set>" + tiles + "</set>";
        //    playturn += setoftile;
        //    // n
        //    string n1 = "<n>1</n>";
        //    string n2 = "<n>2</n>";
        //    string n3 = "<n>3</n>";
        //    string n4 = "<n>4</n>";
        //    playturn += n1;
        //    playturn += n2;
        //    playturn += n3;
        //    playturn += n4;
        //    playturn += "</play-turn>";
        //    XmlDocument doc = new XmlDocument();
        //    doc.LoadXml(playturn);
        //    XmlNode newNode = doc.DocumentElement;
        //    Parser parser = new Parser();

        //    (Dictionary<(int, int), Tile> TilesTobePlaced, Dictionary<string, (Position, Position)> tokenPosition, HashSet<Tile> SetofTiles, List<int> nums) = parser.PlayTurnXML(newNode);

        //    Tile ans_tile = new Tile(1, new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 });
        //    Tile ans_tile2 = new Tile(9, new List<int> { 0, 4, 1, 5, 2, 6, 3, 7 });
        //    // board check
        //    Assert.AreEqual(2, TilesTobePlaced.Count);
        //    Assert.AreEqual(1, TilesTobePlaced[(3, 4)].id);
        //    Assert.AreEqual(9, TilesTobePlaced[(4, 4)].id);
        //    Assert.IsTrue(ans_tile.CompareByPath(TilesTobePlaced[(3, 4)]));
        //    Assert.IsTrue(ans_tile2.CompareByPath(TilesTobePlaced[(4, 4)]));
        //    Assert.AreEqual(2, tokenPosition.Count);
        //    Assert.AreEqual(new Position(2, 2, 5, false), tokenPosition["blue"].Item1);
        //    Assert.AreEqual(new Position(2, 3, 0, false), tokenPosition["blue"].Item2);
        //    Assert.AreEqual(new Position(3, 1, 3, false), tokenPosition["red"].Item1);
        //    Assert.AreEqual(new Position(4, 1, 6, false), tokenPosition["red"].Item2);
        //    // set of tiles check
        //    List<Tile> ans_tiles = new List<Tile>();
        //    ans_tiles.Add(ans_tile);
        //    ans_tiles.Add(ans_tile2);
        //    Assert.AreEqual(2, SetofTiles.Count);
        //    List<Tile> result_list = SetofTiles.ToList();
        //    Boolean good = false;
        //    for (int j = 0; j < ans_tiles.Count; j++)
        //    {
        //        good = false;
        //        for (int i = 0; i < result_list.Count; i++)
        //        {
        //            if (!result_list[i].IsDifferent(ans_tiles[j]))
        //            {
        //                good = true;
        //                break;
        //            }
        //        }
        //        Assert.IsTrue(good);
        //    }
        //    // num check
        //    Assert.AreEqual(4, nums.Count);
        //    for (int i = 1; i < 5; i++)
        //    {
        //        Assert.AreEqual(i, nums[i - 1]);
        //    }
        //}


        //[TestMethod]
        //public void TestParserPlayTurn2Num()
        //{
        //    string playturn = "<play-turn>";
        //    // board
        //    string board = "<board>";
        //    string Tiles = "<map><ent><xy><x><n>3</n></x><y><n>4</n></y></xy><tile><connect><n>0</n><n>1</n></connect><connect><n>2</n><n>3</n></connect><connect><n>4</n><n>5</n></connect><connect><n>6</n><n>7</n></connect></tile></ent><ent><xy><x><n>4</n></x><y><n>4</n></y></xy><tile><connect><n>0</n><n>4</n></connect><connect><n>1</n><n>5</n></connect><connect><n>2</n><n>6</n></connect><connect><n>3</n><n>7</n></connect></tile></ent></map>";
        //    string Pawns = "<map><ent><color>blue</color><pawn-loc><h></h><n>3</n><n>4</n></pawn-loc></ent> <ent><color>red</color><pawn-loc><v></v><n>4</n><n>3</n></pawn-loc></ent></map>";
        //    board += Tiles;
        //    board += Pawns;
        //    board += "</board>";
        //    playturn += board;
        //    // set of tile
        //    // (0, 1, 2, 3, 4, 5, 6, 7), id 1
        //    // (0, 4, 1, 5, 2, 6, 3, 7), id 9
        //    string tiles = "<tile><connect><n>0</n><n>1</n></connect><connect><n>2</n><n>3</n></connect><connect><n>4</n><n>5</n></connect><connect><n>6</n><n>7</n></connect></tile><tile><connect><n>0</n><n>4</n></connect><connect><n>1</n><n>5</n></connect><connect><n>2</n><n>6</n></connect><connect><n>3</n><n>7</n></connect></tile>";
        //    string setoftile = "<set>" + tiles + "</set>";
        //    playturn += setoftile;
        //    // n
        //    string n = "<n>3</n>";
        //    string n1 = "<n>2</n>";
        //    playturn += n;
        //    playturn += n1;
        //    playturn += "</play-turn>";
        //    XmlDocument doc = new XmlDocument();
        //    doc.LoadXml(playturn);
        //    XmlNode newNode = doc.DocumentElement;
        //    Parser parser = new Parser();
        //    (Dictionary<(int, int), Tile> TilesTobePlaced, Dictionary<string, (Position, Position)> tokenPosition, HashSet<Tile> SetofTiles, List<int> nums) = parser.PlayTurnXML(newNode);
        //    Tile ans_tile = new Tile(1, new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 });
        //    Tile ans_tile2 = new Tile(9, new List<int> { 0, 4, 1, 5, 2, 6, 3, 7 });
        //    // board check
        //    Assert.AreEqual(2, TilesTobePlaced.Count);
        //    Assert.AreEqual(1, TilesTobePlaced[(3, 4)].id);
        //    Assert.AreEqual(9, TilesTobePlaced[(4, 4)].id);
        //    Assert.IsTrue(ans_tile.CompareByPath(TilesTobePlaced[(3, 4)]));
        //    Assert.IsTrue(ans_tile2.CompareByPath(TilesTobePlaced[(4, 4)]));
        //    Assert.AreEqual(2, tokenPosition.Count);
        //    Assert.AreEqual(new Position(2, 2, 5, false), tokenPosition["blue"].Item1);
        //    Assert.AreEqual(new Position(2, 3, 0, false), tokenPosition["blue"].Item2);
        //    Assert.AreEqual(new Position(3, 1, 3, false), tokenPosition["red"].Item1);
        //    Assert.AreEqual(new Position(4, 1, 6, false), tokenPosition["red"].Item2);
        //    // set of tiles check
        //    List<Tile> ans_tiles = new List<Tile>();
        //    ans_tiles.Add(ans_tile);
        //    ans_tiles.Add(ans_tile2);
        //    Assert.AreEqual(2, SetofTiles.Count);
        //    List<Tile> result_list = SetofTiles.ToList();
        //    Boolean good = false;
        //    for (int j = 0; j < ans_tiles.Count; j++)
        //    {
        //        good = false;
        //        for (int i = 0; i < result_list.Count; i++)
        //        {
        //            if (!result_list[i].IsDifferent(ans_tiles[j]))
        //            {
        //                good = true;
        //                break;
        //            }
        //        }
        //        Assert.IsTrue(good);
        //    }
        //    // num check
        //    Assert.AreEqual(2, nums.Count);
        //    Assert.AreEqual(3, nums[0]);
        //    Assert.AreEqual(2, nums[1]);
        //}
    }
}
