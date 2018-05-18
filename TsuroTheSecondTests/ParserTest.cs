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
        public void TestParserPawnLoc()
        {
            string xmlContent = "<pawn-loc><h></h><n>3</n><n>4</n></pawn-loc>";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlContent);
            XmlNode newNode = doc.DocumentElement;
            Parser parser = new Parser();
            (string horv, int x, int y) = parser.PawnLocXML(newNode);
            Assert.AreEqual("h", horv);
            Assert.AreEqual(3, x);
            Assert.AreEqual(4, y);
        }

        [TestMethod]
        public void TestParserPawns()
        {
            string xmlContent = "<map><ent><color>blue</color><pawn-loc><h></h><n>3</n><n>4</n></pawn-loc></ent> <ent><color>red</color><pawn-loc><v></v><n>4</n><n>3</n></pawn-loc></ent></map>";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlContent);
            XmlNode newNode = doc.DocumentElement;
            Parser parser = new Parser();
            // IN PROGRESS
            //
        }
    }
}
