using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml;
using TsuroTheSecond;


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
        public void TestParserGetCommand()
        {
            XElement get_name = XElement.Parse("<get-name> </get-name>");
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(get_name.ToString());
            Parser parser = new Parser();
            string command = parser.GetCommand(xmlDocument);
            Assert.AreEqual("get-name", command);

        }

        [TestMethod]
        public void TestParserInitialize()
        {
            XElement initialize = XElement.Parse("<initialize> <color> blue </color> <list> <color> red </color> <color> blue </color> <color> green </color> </list> </initialize>");
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(initialize.ToString());
            Parser parser = new Parser();
            (string own_color, List<string> other_colors) = parser.InitializeXML(xmlDocument);
            Assert.AreEqual("blue", own_color);
            CollectionAssert.AreEqual(new List<string> { "red", "blue", "green" }, other_colors);
        }

        [TestMethod]
        public void TestParserPlacePawn()
        {
            // place-pawn contains a board so it should output a board object.

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
        public void TestParserConnect(){
            string xmlContent = "<connect><n>0</n><n></n></connect>";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlContent);
            XmlNode newNode = doc.DocumentElement;
            Parser parser = new Parser();


        }
    }
}
