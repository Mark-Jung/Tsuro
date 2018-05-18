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
        public void TestParserGetName(){
            XElement get_name = XElement.Parse("<get-name> </get-name>");
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(get_name.ToString());
            Parser parser = new Parser();
            (string command, object output) = parser.FromXML(xmlDocument);
            Assert.AreEqual("get-name", command);
            Assert.IsNull(output);
        }
        //[TestMethod]
        //public void TestParserInitialize()
        //{
        //    XElement get_name = XElement.Parse("<get-name> </get-name>");
        //    XmlDocument xmlDocument = new XmlDocument();
        //    xmlDocument.LoadXml(get_name.ToString());
        //    Parser parser = new Parser();
        //    (string command, object output) = parser.FromXML(xmlDocument);
        //    Assert.AreEqual("get-name", command);
        //    Assert.IsNull(output);
        //}

    }
}
