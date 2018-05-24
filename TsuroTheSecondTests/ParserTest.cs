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
    public class ParserTest
    {

        //[TestMethod]
        //public void TestParserBoardXMLEmptyBoard()
        //{
        //    XElement board = new XElement("board", "hi");
        //    XmlDocument xmlDocument = new XmlDocument();
        //    xmlDocument.LoadXml(board.ToString());
        //    Parser parser = new Parser();

        //    Board result = parser.BoardXML(xmlDocument);

        //    //Assert.AreEqual("board", result);

        //    //foreach (Tile each in result.tiles){
        //    //    Assert.IsNull(each);
        //    //}
        //}
        [TestMethod]
        public void TestParserGetNameCommand()
        {
            XElement get_name = XElement.Parse("<get-name> </get-name>");
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(get_name.ToString());
            XmlNode newNode = xmlDocument.DocumentElement;
            Parser parser = new Parser();
            string command = parser.GetCommand(newNode);
            Assert.AreEqual("get-name", command);
        }

        [TestMethod]
        public void TestParserInitialize()
        {
            string initialize = "<initialize> <color> blue </color> <list> <color> red </color> <color> blue </color> <color> green </color> </list> </initialize>";
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(initialize);
            XmlNode newNode = xmlDocument.DocumentElement;
            Parser parser = new Parser();
            (string own_color, List<string> other_colors) = parser.InitializeXML(newNode);
            Assert.AreEqual("blue", own_color);
            CollectionAssert.AreEqual(new List<string> { "red", "blue", "green" }, other_colors);
        }

        [TestMethod]
        public void TestParserN()
        {
            string xmlContent = "<n>5</n>";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlContent);
            XmlNode newNode = doc.DocumentElement;
            Parser parser = new Parser();
            int nat = parser.NXML(newNode);
            Assert.AreEqual(5, nat);
        }

        [TestMethod]
        public void TestParserHV_H()
        {
            string xmlContent = "<h></h>";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlContent);
            XmlNode newNode = doc.DocumentElement;
            Parser parser = new Parser();
            string horv = parser.HVXML(newNode);
            Assert.AreEqual("h", horv);
        }

        [TestMethod]
        public void TestParserHV_V()
        {
            string xmlContent = "<v></v>";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlContent);
            XmlNode newNode = doc.DocumentElement;
            Parser parser = new Parser();
            string horv = parser.HVXML(newNode);
            Assert.AreEqual("v", horv);
        }

        [TestMethod]
        public void TestParserPawnLocHorizontal()
        {
            string xmlContent = "<pawn-loc><h></h><n>3</n><n>4</n></pawn-loc>";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlContent);
            XmlNode newNode = doc.DocumentElement;
            Parser parser = new Parser();
            (Position result, Position result1) = parser.PawnLocXML(newNode);
            Assert.AreEqual(new Position(2, 2, 5, false), result);
            Assert.AreEqual(new Position(2, 3, 0, false), result1);

        }
        [TestMethod]
        public void TestParserPawnLocVert()
        {
            string xmlContent = "<pawn-loc><v></v><n>2</n><n>3</n></pawn-loc>";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlContent);
            XmlNode newNode = doc.DocumentElement;
            Parser parser = new Parser();
            (Position result, Position result1) = parser.PawnLocXML(newNode);
            Assert.AreEqual(new Position(1, 1, 3, false), result);
            Assert.AreEqual(new Position(2, 1, 6, false), result1);

        }

        [TestMethod]
        public void TestParserPawns()
        {
            /*
             * Each pawn location refers to a place on the edge of a tile and each has three components.
             * The first component indicates if the location is on a horizontal or a vertical edge. 
             * The second component indicates which edge it is, counting from 0 to 6. 
             * 0 means the upper or leftmost edge of the board and 6 means either the lower or rightmost edge.
             * If the pawn location is a horizontal location, a 1 means that it pawn location is on the bottom edge of the first row of tiles,
             * which is also the top edge of the second row tiles. 
             * Similarly, 1 in a vertical pawn location means either the right edge of the leftmost column of tiles, 
             * which is also the left edge of the second leftmost row of tiles. The last component refers to the spot along the tile. 
             * Each tile has two places a pawn might be, so those values range from 0 to 11.
             */
            string xmlContent = "<map><ent><color>blue</color><pawn-loc><h></h><n>3</n><n>4</n></pawn-loc></ent> <ent><color>red</color><pawn-loc><v></v><n>4</n><n>3</n></pawn-loc></ent></map>";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlContent);
            XmlNode newNode = doc.DocumentElement;
            Parser parser = new Parser();
            // will return a dictionary of color : Position
            Dictionary<string, (Position, Position)> result = parser.PawnsXML(newNode);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(new Position(2, 2, 5, false), result["blue"].Item1);
            Assert.AreEqual(new Position(2, 3, 0, false), result["blue"].Item2);
            Assert.AreEqual(new Position(3, 1, 3, false), result["red"].Item1);
            Assert.AreEqual(new Position(4, 1, 6, false), result["red"].Item2);
        }
        [TestMethod]
        public void TestParserConnect()
        {
            string xmlContent = "<connect><n>0</n><n>1</n></connect>";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlContent);
            XmlNode newNode = doc.DocumentElement;
            Parser parser = new Parser();
            // will return two integers.(ports that will be connected)
            List<int> result = parser.ConnectXML(newNode);
            CollectionAssert.AreEqual(new List<int> { 0, 1 }, result);
        }
        [TestMethod]
        public void TestParserTile()
        {
            string xmlContent = "<tile><connect><n>0</n><n>1</n></connect><connect><n>2</n><n>3</n></connect><connect><n>4</n><n>5</n></connect><connect><n>6</n><n>7</n></connect></tile>";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlContent);
            XmlNode newNode = doc.DocumentElement;
            Parser parser = new Parser();
            // will return a tile with the paths and the id.
            Tile tile = parser.TileXML(newNode);
            Tile answer_tile = new Tile(0, new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 });
            Assert.IsTrue(answer_tile.CompareByPath(tile));
        }
        //[TestMethod]
        //public void TestParserBadTile()
        //{
        //    string xmlContent = "<tile><connect><n>0</n><n>7</n></connect><connect><n>1</n><n>2</n></connect><connect><n>4</n><n>6</n></connect><connect><n>5</n><n>3</n></connect></tile>";
        //    XmlDocument doc = new XmlDocument();
        //    doc.LoadXml(xmlContent);
        //    XmlNode newNode = doc.DocumentElement;
        //    Parser parser = new Parser();
        //    // will return a tile with the paths and the id.
        //    // if the tile with that path doesn't exit, the id should be -1
        //    Tile tile = parser.TileXML(newNode);
        //    Assert.AreEqual(-1, tile.id);
        //}
        [TestMethod]
        public void TestParserXY(){
            string xmlContent = "<xy><x>3</x><y>4</y></xy>";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlContent);
            XmlNode newNode = doc.DocumentElement;
            Parser parser = new Parser();
            // need to return two integers: x and y(3, 4)
            (int x, int y) = parser.XYXML(newNode);
            Assert.AreEqual(3, x);
            Assert.AreEqual(4, y);
        }
        [TestMethod]
        public void TestParserTilesOne(){
            string xmlContent = "<map><ent><xy><x><n>3</n></x><y><n>4</n></y></xy><tile><connect><n>0</n><n>1</n></connect><connect><n>2</n><n>3</n></connect><connect><n>4</n><n>5</n></connect><connect><n>6</n><n>7</n></connect></tile></ent></map>";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlContent);
            XmlNode newNode = doc.DocumentElement;
            Parser parser = new Parser();
            // 3, 4, (0, 1, 2, 3 ,4 ,5 , 6, 7)
            Dictionary<(int, int), Tile> result = parser.TilesXML(newNode);
            Tile ans_tile = new Tile(1, new List<int>{ 0, 1, 2, 3, 4, 5, 6, 7 });
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(1, result[(3, 4)].id);
            Assert.IsTrue(ans_tile.CompareByPath(result[(3, 4)]));
        }
        [TestMethod]
        public void TestParserTilesMulti()
        {
            string xmlContent = "<map><ent><xy><x><n>3</n></x><y><n>4</n></y></xy><tile><connect><n>0</n><n>1</n></connect><connect><n>2</n><n>3</n></connect><connect><n>4</n><n>5</n></connect><connect><n>6</n><n>7</n></connect></tile></ent><ent><xy><x><n>4</n></x><y><n>4</n></y></xy><tile><connect><n>0</n><n>4</n></connect><connect><n>1</n><n>5</n></connect><connect><n>2</n><n>6</n></connect><connect><n>3</n><n>7</n></connect></tile></ent></map>";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlContent);
            XmlNode newNode = doc.DocumentElement;
            Parser parser = new Parser();
            // 3, 4 (0, 1, 2, 3, 4, 5, 6, 7), id 1
            // 4, 4 (0, 4, 1, 5, 2, 6, 3, 7), id 9
            Dictionary<(int, int), Tile> result = parser.TilesXML(newNode);
            Tile ans_tile = new Tile(1, new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 });
            Tile ans_tile2 = new Tile(9, new List<int> { 0, 4, 1, 5, 2, 6, 3, 7 });
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(1, result[(3, 4)].id);
            Assert.AreEqual(9, result[(4, 4)].id);
            Assert.IsTrue(ans_tile.CompareByPath(result[(3, 4)]));
            Assert.IsTrue(ans_tile2.CompareByPath(result[(4, 4)]));
        }
        [TestMethod]
        public void TestParserBoard()
        {
            string xmlContent = "<board>";
            string Tiles = "<map><ent><xy><x>3</x><y>4</y></xy><tile><connect><n>0</n><n>1</n></connect><connect><n>2</n><n>3</n></connect><connect><n>4</n><n>5</n></connect><connect><n>6</n><n>7</n></connect></tile></ent><ent><xy><x><n>4</n></x><y><n>4</n></y></xy><tile><connect><n>0</n><n>4</n></connect><connect><n>1</n><n>5</n></connect><connect><n>2</n><n>6</n></connect><connect><n>3</n><n>7</n></connect></tile></ent></map>";
            string Pawns = "<map><ent><color>blue</color><pawn-loc><h></h><n>3</n><n>4</n></pawn-loc></ent> <ent><color>red</color><pawn-loc><v></v><n>4</n><n>3</n></pawn-loc></ent></map>";
            xmlContent += Tiles;
            xmlContent += Pawns;
            xmlContent += "</board>";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlContent);
            XmlNode newNode = doc.DocumentElement;
            Parser parser = new Parser();
            // returns tiles to be placed on the board : Dictionary key of position(int x, int y) and value of tiles
            // returns tokens position: Dictionary key of color(string) and value of (Position, Position)
            (Dictionary<(int, int), Tile> TilesTobePlaced, Dictionary<string, (Position, Position)> tokenPosition) = parser.BoardXML(newNode);
            Tile ans_tile = new Tile(1, new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 });
            Tile ans_tile2 = new Tile(9, new List<int> { 0, 4, 1, 5, 2, 6, 3, 7 });
            Assert.AreEqual(2, TilesTobePlaced.Count);
            Assert.AreEqual(1, TilesTobePlaced[(3, 4)].id);
            Assert.AreEqual(9, TilesTobePlaced[(4, 4)].id);
            Assert.IsTrue(ans_tile.CompareByPath(TilesTobePlaced[(3, 4)]));
            Assert.IsTrue(ans_tile2.CompareByPath(TilesTobePlaced[(4, 4)]));
            Assert.AreEqual(2, tokenPosition.Count);
            Assert.AreEqual(new Position(2, 2, 5, false), tokenPosition["blue"].Item1);
            Assert.AreEqual(new Position(2, 3, 0, false), tokenPosition["blue"].Item2);
            Assert.AreEqual(new Position(3, 1, 3, false), tokenPosition["red"].Item1);
            Assert.AreEqual(new Position(4, 1, 6, false), tokenPosition["red"].Item2);
        }
        [TestMethod]
        public void TestParserPlacePawn()
        {
            string xmlContent = "<place-pawn><board>";
            string Tiles = "<map><ent><xy><x><n>3</n></x><y><n>4</n></y></xy><tile><connect><n>0</n><n>1</n></connect><connect><n>2</n><n>3</n></connect><connect><n>4</n><n>5</n></connect><connect><n>6</n><n>7</n></connect></tile></ent><ent><xy><x><n>4</n></x><y><n>4</n></y></xy><tile><connect><n>0</n><n>4</n></connect><connect><n>1</n><n>5</n></connect><connect><n>2</n><n>6</n></connect><connect><n>3</n><n>7</n></connect></tile></ent></map>";
            string Pawns = "<map><ent><color>blue</color><pawn-loc><h></h><n>3</n><n>4</n></pawn-loc></ent> <ent><color>red</color><pawn-loc><v></v><n>4</n><n>3</n></pawn-loc></ent></map>";
            xmlContent += Tiles;
            xmlContent += Pawns;
            xmlContent += "</board></place-pawn>";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlContent);
            XmlNode newNode = doc.DocumentElement;
            Parser parser = new Parser();
            // returns tiles to be placed on the board : Dictionary key of position(int x, int y) and value of tiles
            // returns tokens position: Dictionary key of color(string) and value of (Position, Position)
            (Dictionary<(int, int), Tile> TilesTobePlaced, Dictionary<string, (Position, Position)> tokenPosition) = parser.PlacePawnXML(newNode);
            Tile ans_tile = new Tile(1, new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 });
            Tile ans_tile2 = new Tile(9, new List<int> { 0, 4, 1, 5, 2, 6, 3, 7 });
            Assert.AreEqual(2, TilesTobePlaced.Count);
            Assert.AreEqual(1, TilesTobePlaced[(3, 4)].id);
            Assert.AreEqual(9, TilesTobePlaced[(4, 4)].id);
            Assert.IsTrue(ans_tile.CompareByPath(TilesTobePlaced[(3, 4)]));
            Assert.IsTrue(ans_tile2.CompareByPath(TilesTobePlaced[(4, 4)]));
            Assert.AreEqual(2, tokenPosition.Count);
            Assert.AreEqual(new Position(2, 2, 5, false), tokenPosition["blue"].Item1);
            Assert.AreEqual(new Position(2, 3, 0, false), tokenPosition["blue"].Item2);
            Assert.AreEqual(new Position(3, 1, 3, false), tokenPosition["red"].Item1);
            Assert.AreEqual(new Position(4, 1, 6, false), tokenPosition["red"].Item2);
        }
        [TestMethod]
        public void TestParserSetofTile(){
            // (0, 1, 2, 3, 4, 5, 6, 7), id 1
            // (0, 4, 1, 5, 2, 6, 3, 7), id 9
            string tiles = "<tile><connect><n>0</n><n>1</n></connect><connect><n>2</n><n>3</n></connect><connect><n>4</n><n>5</n></connect><connect><n>6</n><n>7</n></connect></tile><tile><connect><n>0</n><n>4</n></connect><connect><n>1</n><n>5</n></connect><connect><n>2</n><n>6</n></connect><connect><n>3</n><n>7</n></connect></tile>";
            string setoftile = "<set>" + tiles + "</set>";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(setoftile);
            XmlNode newNode = doc.DocumentElement;
            Parser parser = new Parser();
            HashSet<Tile> result = parser.SetofTilesXML(newNode);
            Tile ans_tile = new Tile(1, new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 });
            Tile ans_tile2 = new Tile(9, new List<int> { 0, 4, 1, 5, 2, 6, 3, 7 });
            List<Tile> ans_tiles = new List<Tile>();
            ans_tiles.Add(ans_tile);
            ans_tiles.Add(ans_tile2);
            Assert.AreEqual(2, result.Count);
            List<Tile> result_list = result.ToList();
            Boolean good = false;
            for (int j = 0; j < ans_tiles.Count; j++)
            {
                good = false;
                for (int i = 0; i < result_list.Count; i++)
                {
                    if(!result_list[i].IsDifferent(ans_tiles[j])){
                        good = true;
                        break;
                    }
                }
                Assert.IsTrue(good);
            }
        }

        [TestMethod]
        public void TestParserListofTile()
        {
            // (0, 1, 2, 3, 4, 5, 6, 7), id 1
            // (0, 4, 1, 5, 2, 6, 3, 7), id 9
            string tiles = "<tile><connect><n>0</n><n>1</n></connect><connect><n>2</n><n>3</n></connect><connect><n>4</n><n>5</n></connect><connect><n>6</n><n>7</n></connect></tile><tile><connect><n>0</n><n>4</n></connect><connect><n>1</n><n>5</n></connect><connect><n>2</n><n>6</n></connect><connect><n>3</n><n>7</n></connect></tile>";
            string setoftile = "<list>" + tiles + "</list>";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(setoftile);
            XmlNode newNode = doc.DocumentElement;
            Parser parser = new Parser();
            List<Tile> result = parser.ListofTilesXML(newNode);
            Tile ans_tile = new Tile(1, new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 });
            Tile ans_tile2 = new Tile(9, new List<int> { 0, 4, 1, 5, 2, 6, 3, 7 });
            List<Tile> ans_tiles = new List<Tile>();
            ans_tiles.Add(ans_tile);
            ans_tiles.Add(ans_tile2);
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(!ans_tile.IsDifferent(result[0]));
            Assert.IsTrue(!ans_tile2.IsDifferent(result[1]));
        }

        [TestMethod]
        public void TestParserSetofTilesDupli()
        {
            // (0, 1, 2, 3, 4, 5, 6, 7), id 1
            // (0, 4, 1, 5, 2, 6, 3, 7), id 9
            string tiles = "<tile><connect><n>0</n><n>1</n></connect><connect><n>2</n><n>3</n></connect><connect><n>4</n><n>5</n></connect><connect><n>6</n><n>7</n></connect></tile><tile><connect><n>0</n><n>4</n></connect><connect><n>1</n><n>5</n></connect><connect><n>2</n><n>6</n></connect><connect><n>3</n><n>7</n></connect></tile><tile><connect><n>0</n><n>4</n></connect><connect><n>1</n><n>5</n></connect><connect><n>2</n><n>6</n></connect><connect><n>3</n><n>7</n></connect></tile>";
            string setoftile = "<set>" + tiles + "</set>";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(setoftile);
            XmlNode newNode = doc.DocumentElement;
            Parser parser = new Parser();
            HashSet<Tile> result = parser.SetofTilesXML(newNode);
            Assert.AreEqual(2, result.Count);

        }

        [TestMethod]
        public void TestParserPlayTurn4Num()
        {
            string playturn = "<play-turn>";
            // board
            string board = "<board>";
            string Tiles = "<map><ent><xy><x><n>3</n></x><y><n>4</n></y></xy><tile><connect><n>0</n><n>1</n></connect><connect><n>2</n><n>3</n></connect><connect><n>4</n><n>5</n></connect><connect><n>6</n><n>7</n></connect></tile></ent><ent><xy><x><n>4</n></x><y><n>4</n></y></xy><tile><connect><n>0</n><n>4</n></connect><connect><n>1</n><n>5</n></connect><connect><n>2</n><n>6</n></connect><connect><n>3</n><n>7</n></connect></tile></ent></map>";
            string Pawns = "<map><ent><color>blue</color><pawn-loc><h></h><n>3</n><n>4</n></pawn-loc></ent> <ent><color>red</color><pawn-loc><v></v><n>4</n><n>3</n></pawn-loc></ent></map>";
            board += Tiles;
            board += Pawns;
            board += "</board>";
            playturn += board;
            // set of tile
            // (0, 1, 2, 3, 4, 5, 6, 7), id 1
            // (0, 4, 1, 5, 2, 6, 3, 7), id 9
            string tiles = "<tile><connect><n>0</n><n>1</n></connect><connect><n>2</n><n>3</n></connect><connect><n>4</n><n>5</n></connect><connect><n>6</n><n>7</n></connect></tile><tile><connect><n>0</n><n>4</n></connect><connect><n>1</n><n>5</n></connect><connect><n>2</n><n>6</n></connect><connect><n>3</n><n>7</n></connect></tile>";
            string setoftile = "<set>" + tiles + "</set>";
            playturn += setoftile;
            // n
            string n1 = "<n>1</n>";
            string n2 = "<n>2</n>";
            string n3 = "<n>3</n>";
            string n4 = "<n>4</n>";
            playturn += n1;
            playturn += n2;
            playturn += n3;
            playturn += n4;
            playturn += "</play-turn>";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(playturn);
            XmlNode newNode = doc.DocumentElement;
            Parser parser = new Parser();

            (Dictionary<(int, int), Tile> TilesTobePlaced, Dictionary<string, (Position, Position)> tokenPosition, HashSet<Tile> SetofTiles, List<int> nums) = parser.PlayTurnXML(newNode);

            Tile ans_tile = new Tile(1, new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 });
            Tile ans_tile2 = new Tile(9, new List<int> { 0, 4, 1, 5, 2, 6, 3, 7 });
            // board check
            Assert.AreEqual(2, TilesTobePlaced.Count);
            Assert.AreEqual(1, TilesTobePlaced[(3, 4)].id);
            Assert.AreEqual(9, TilesTobePlaced[(4, 4)].id);
            Assert.IsTrue(ans_tile.CompareByPath(TilesTobePlaced[(3, 4)]));
            Assert.IsTrue(ans_tile2.CompareByPath(TilesTobePlaced[(4, 4)]));
            Assert.AreEqual(2, tokenPosition.Count);
            Assert.AreEqual(new Position(2, 2, 5, false), tokenPosition["blue"].Item1);
            Assert.AreEqual(new Position(2, 3, 0, false), tokenPosition["blue"].Item2);
            Assert.AreEqual(new Position(3, 1, 3, false), tokenPosition["red"].Item1);
            Assert.AreEqual(new Position(4, 1, 6, false), tokenPosition["red"].Item2);
            // set of tiles check
            List<Tile> ans_tiles = new List<Tile>();
            ans_tiles.Add(ans_tile);
            ans_tiles.Add(ans_tile2);
            Assert.AreEqual(2, SetofTiles.Count);
            List<Tile> result_list = SetofTiles.ToList();
            Boolean good = false;
            for (int j = 0; j < ans_tiles.Count; j++)
            {
                good = false;
                for (int i = 0; i < result_list.Count; i++)
                {
                    if (!result_list[i].IsDifferent(ans_tiles[j]))
                    {
                        good = true;
                        break;
                    }
                }
                Assert.IsTrue(good);
            }
            // num check
            Assert.AreEqual(4, nums.Count);
            for (int i = 1; i < 5; i++)
            {
                Assert.AreEqual(i, nums[i - 1]);
            }
        }


        [TestMethod]
        public void TestParserPlayTurn2Num()
        {
            string playturn = "<play-turn>";
            // board
            string board = "<board>";
            string Tiles = "<map><ent><xy><x><n>3</n></x><y><n>4</n></y></xy><tile><connect><n>0</n><n>1</n></connect><connect><n>2</n><n>3</n></connect><connect><n>4</n><n>5</n></connect><connect><n>6</n><n>7</n></connect></tile></ent><ent><xy><x><n>4</n></x><y><n>4</n></y></xy><tile><connect><n>0</n><n>4</n></connect><connect><n>1</n><n>5</n></connect><connect><n>2</n><n>6</n></connect><connect><n>3</n><n>7</n></connect></tile></ent></map>";
            string Pawns = "<map><ent><color>blue</color><pawn-loc><h></h><n>3</n><n>4</n></pawn-loc></ent> <ent><color>red</color><pawn-loc><v></v><n>4</n><n>3</n></pawn-loc></ent></map>";
            board += Tiles;
            board += Pawns;
            board += "</board>";
            playturn += board;
            // set of tile
            // (0, 1, 2, 3, 4, 5, 6, 7), id 1
            // (0, 4, 1, 5, 2, 6, 3, 7), id 9
            string tiles = "<tile><connect><n>0</n><n>1</n></connect><connect><n>2</n><n>3</n></connect><connect><n>4</n><n>5</n></connect><connect><n>6</n><n>7</n></connect></tile><tile><connect><n>0</n><n>4</n></connect><connect><n>1</n><n>5</n></connect><connect><n>2</n><n>6</n></connect><connect><n>3</n><n>7</n></connect></tile>";
            string setoftile = "<set>" + tiles + "</set>";
            playturn += setoftile;
            // n
            string n = "<n>3</n>";
            string n1 = "<n>2</n>";
            playturn += n;
            playturn += n1;
            playturn += "</play-turn>";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(playturn);
            XmlNode newNode = doc.DocumentElement;
            Parser parser = new Parser();
            (Dictionary<(int, int), Tile> TilesTobePlaced, Dictionary<string, (Position, Position)> tokenPosition, HashSet<Tile> SetofTiles, List<int> nums) = parser.PlayTurnXML(newNode);
            Tile ans_tile = new Tile(1, new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 });
            Tile ans_tile2 = new Tile(9, new List<int> { 0, 4, 1, 5, 2, 6, 3, 7 });
            // board check
            Assert.AreEqual(2, TilesTobePlaced.Count);
            Assert.AreEqual(1, TilesTobePlaced[(3, 4)].id);
            Assert.AreEqual(9, TilesTobePlaced[(4, 4)].id);
            Assert.IsTrue(ans_tile.CompareByPath(TilesTobePlaced[(3, 4)]));
            Assert.IsTrue(ans_tile2.CompareByPath(TilesTobePlaced[(4, 4)]));
            Assert.AreEqual(2, tokenPosition.Count);
            Assert.AreEqual(new Position(2, 2, 5, false), tokenPosition["blue"].Item1);
            Assert.AreEqual(new Position(2, 3, 0, false), tokenPosition["blue"].Item2);
            Assert.AreEqual(new Position(3, 1, 3, false), tokenPosition["red"].Item1);
            Assert.AreEqual(new Position(4, 1, 6, false), tokenPosition["red"].Item2);
            // set of tiles check
            List<Tile> ans_tiles = new List<Tile>();
            ans_tiles.Add(ans_tile);
            ans_tiles.Add(ans_tile2);
            Assert.AreEqual(2, SetofTiles.Count);
            List<Tile> result_list = SetofTiles.ToList();
            Boolean good = false;
            for (int j = 0; j < ans_tiles.Count; j++)
            {
                good = false;
                for (int i = 0; i < result_list.Count; i++)
                {
                    if (!result_list[i].IsDifferent(ans_tiles[j]))
                    {
                        good = true;
                        break;
                    }
                }
                Assert.IsTrue(good);
            }
            // num check
            Assert.AreEqual(2, nums.Count);
            Assert.AreEqual(3, nums[0]);
            Assert.AreEqual(2, nums[1]);
        }
        [TestMethod]
        public void TestParserSPlayerDragon()
        {
            string splayer = "<splayer-dragon>";
            string color = "<color>blue</color>";
            // (0, 1, 2, 3, 4, 5, 6, 7), id 1
            // (0, 4, 1, 5, 2, 6, 3, 7), id 9
            // (0, 3, 1, 5, 2, 7, 4, 6), id 12
            string tiles = "<tile><connect><n>0</n><n>3</n></connect><connect><n>1</n><n>5</n></connect><connect><n>2</n><n>7</n></connect><connect><n>4</n><n>6</n></connect></tile><tile><connect><n>0</n><n>1</n></connect><connect><n>2</n><n>3</n></connect><connect><n>4</n><n>5</n></connect><connect><n>6</n><n>7</n></connect></tile><tile><connect><n>0</n><n>4</n></connect><connect><n>1</n><n>5</n></connect><connect><n>2</n><n>6</n></connect><connect><n>3</n><n>7</n></connect></tile>";
            string setoftile = "<set>" + tiles + "</set>";
            splayer += color;
            splayer += setoftile;
            splayer += "</splayer-dragon>";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(splayer);
            XmlNode newNode = doc.DocumentElement;
            Parser parser = new Parser();

            (string playercolor, List<Tile> hand, Boolean IsDragon) = parser.SPlayerXML(newNode);

            Assert.AreEqual("blue", playercolor);
            Assert.IsTrue(IsDragon);
            Tile ans_tile0 = new Tile(12, new List<int> { 0, 3, 1, 5, 2, 7, 4, 6});
            Tile ans_tile1 = new Tile(1, new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 });
            Tile ans_tile2 = new Tile(9, new List<int> { 0, 4, 1, 5, 2, 6, 3, 7 });
        }

        [TestMethod]
        public void TestParserSPlayerNoDragon()
        {
            string splayer = "<splayer-nodragon>";
            string color = "<color>blue</color>";
            // (0, 1, 2, 3, 4, 5, 6, 7), id 1
            // (0, 4, 1, 5, 2, 6, 3, 7), id 9
            // (0, 3, 1, 5, 2, 7, 4, 6), id 12
            string tiles = "<tile><connect><n>0</n><n>3</n></connect><connect><n>1</n><n>5</n></connect><connect><n>2</n><n>7</n></connect><connect><n>4</n><n>6</n></connect></tile><tile><connect><n>0</n><n>1</n></connect><connect><n>2</n><n>3</n></connect><connect><n>4</n><n>5</n></connect><connect><n>6</n><n>7</n></connect></tile><tile><connect><n>0</n><n>4</n></connect><connect><n>1</n><n>5</n></connect><connect><n>2</n><n>6</n></connect><connect><n>3</n><n>7</n></connect></tile>";
            string setoftile = "<set>" + tiles + "</set>";
            splayer += color;
            splayer += setoftile;
            splayer += "</splayer-nodragon>";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(splayer);
            XmlNode newNode = doc.DocumentElement;
            Parser parser = new Parser();

            (string playercolor, List<Tile> hand, Boolean IsDragon) = parser.SPlayerXML(newNode);

            Assert.AreEqual("blue", playercolor);
            Assert.IsFalse(IsDragon);
            Tile ans_tile0 = new Tile(12, new List<int> { 0, 3, 1, 5, 2, 7, 4, 6 });
            Tile ans_tile1 = new Tile(1, new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 });
            Tile ans_tile2 = new Tile(9, new List<int> { 0, 4, 1, 5, 2, 6, 3, 7 });
        }

        [TestMethod]
        public void TestParserSPlayerList()
        {
            string listofsplayer = "<list>";
            string splayer = "<splayer-nodragon>";
            string color = "<color>blue</color>";
            // (0, 1, 2, 3, 4, 5, 6, 7), id 1
            // (0, 4, 1, 5, 2, 6, 3, 7), id 9
            // (0, 3, 1, 5, 2, 7, 4, 6), id 12
            string tiles = "<tile><connect><n>0</n><n>3</n></connect><connect><n>1</n><n>5</n></connect><connect><n>2</n><n>7</n></connect><connect><n>4</n><n>6</n></connect></tile><tile><connect><n>0</n><n>1</n></connect><connect><n>2</n><n>3</n></connect><connect><n>4</n><n>5</n></connect><connect><n>6</n><n>7</n></connect></tile><tile><connect><n>0</n><n>4</n></connect><connect><n>1</n><n>5</n></connect><connect><n>2</n><n>6</n></connect><connect><n>3</n><n>7</n></connect></tile>";
            string setoftile = "<set>" + tiles + "</set>";
            splayer += color;
            splayer += setoftile;
            splayer += "</splayer-nodragon>";
            string splayer_drag = "<splayer-dragon>";
            string color_red = "<color>red</color>";
            // (0, 1, 2, 3, 4, 5, 6, 7), id 1
            // (0, 4, 1, 5, 2, 6, 3, 7), id 9
            // (0, 3, 1, 5, 2, 7, 4, 6), id 12
            string tiles_drag = "<tile><connect><n>0</n><n>3</n></connect><connect><n>1</n><n>5</n></connect><connect><n>2</n><n>7</n></connect><connect><n>4</n><n>6</n></connect></tile><tile><connect><n>0</n><n>1</n></connect><connect><n>2</n><n>3</n></connect><connect><n>4</n><n>5</n></connect><connect><n>6</n><n>7</n></connect></tile><tile><connect><n>0</n><n>4</n></connect><connect><n>1</n><n>5</n></connect><connect><n>2</n><n>6</n></connect><connect><n>3</n><n>7</n></connect></tile>";
            string setoftile_drag = "<set>" + tiles_drag + "</set>";
            splayer_drag += color_red;
            splayer_drag += setoftile_drag;
            splayer_drag += "</splayer-dragon>";
            listofsplayer += splayer;
            listofsplayer += splayer_drag;
            listofsplayer += "</list>";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(listofsplayer);
            XmlNode newNode = doc.DocumentElement;
            Parser parser = new Parser();
            Tile ans_tile0 = new Tile(12, new List<int> { 0, 3, 1, 5, 2, 7, 4, 6 });
            Tile ans_tile1 = new Tile(1, new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 });
            Tile ans_tile2 = new Tile(9, new List<int> { 0, 4, 1, 5, 2, 6, 3, 7 });
            List<Tile> ans_tiles = new List<Tile>();
            ans_tiles.Add(ans_tile0);
            ans_tiles.Add(ans_tile1);
            ans_tiles.Add(ans_tile2);

            Dictionary<string, (List<Tile>, Boolean)> result = parser.ListSPlayerXML(newNode);

            (List<Tile> red_hand, Boolean red_isDrag) = result["red"];
            (List<Tile> blue_hand, Boolean blue_isDrag) = result["blue"];
            Assert.IsTrue(red_isDrag);
            Assert.IsFalse(blue_isDrag);
            for (int i = 0; i < 3; i++)
            {
                Assert.IsTrue(!ans_tiles[i].IsDifferent(blue_hand[i]));
                Assert.IsTrue(!ans_tiles[i].IsDifferent(red_hand[i]));
            }
        }

        [TestMethod]
        public void TestParserEndGame()
        {
            string endgame = "<end-game>";
            // board
            string board = "<board>";
            string Tiles = "<map><ent><xy><x><n>3</n></x><y><n>4</n></y></xy><tile><connect><n>0</n><n>1</n></connect><connect><n>2</n><n>3</n></connect><connect><n>4</n><n>5</n></connect><connect><n>6</n><n>7</n></connect></tile></ent><ent><xy><x><n>4</n></x><y><n>4</n></y></xy><tile><connect><n>0</n><n>4</n></connect><connect><n>1</n><n>5</n></connect><connect><n>2</n><n>6</n></connect><connect><n>3</n><n>7</n></connect></tile></ent></map>";
            string Pawns = "<map><ent><color>blue</color><pawn-loc><h></h><n>3</n><n>4</n></pawn-loc></ent></map>";
            board += Tiles;
            board += Pawns;
            board += "</board>";
            endgame += board;
            //list of colors
            string listofcolors = "<list> <color> blue </color> </list>";
            endgame += listofcolors;
            endgame += "</end-game>";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(endgame);
            XmlNode newNode = doc.DocumentElement;
            Parser parser = new Parser();

            (Dictionary<(int, int), Tile> TilesTobePlaced, Dictionary<string, (Position, Position)> tokenPosition, List<string> winnercolors) = parser.EndGameXML(newNode);
            Tile ans_tile = new Tile(1, new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 });
            Tile ans_tile2 = new Tile(9, new List<int> { 0, 4, 1, 5, 2, 6, 3, 7 });
            
            //board check
            Assert.AreEqual(2, TilesTobePlaced.Count);
            Assert.AreEqual(1, TilesTobePlaced[(3, 4)].id);
            Assert.AreEqual(9, TilesTobePlaced[(4, 4)].id);
            Assert.IsTrue(ans_tile.CompareByPath(TilesTobePlaced[(3, 4)]));
            Assert.IsTrue(ans_tile2.CompareByPath(TilesTobePlaced[(4, 4)]));
            Assert.AreEqual(1, tokenPosition.Count);
            Assert.AreEqual(new Position(2, 2, 5, false), tokenPosition["blue"].Item1);
            Assert.AreEqual(new Position(2, 3, 0, false), tokenPosition["blue"].Item2);

            //color list check
            Assert.AreEqual(1, winnercolors.Count);
            Assert.AreEqual("blue", winnercolors[0]);
        }
    }
}
