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
    public class NetworkRelayTest
    {
        NetworkRelay nr = new NetworkRelay();

        [TestMethod]
        public void TestNetworkRelayReceive()
        {
            string s1 = "<get-name> </get-name> \n <initialize> <color> blue </color> <list> <color> red </color> <color> blue </color> <color> green </color> </list> </initialize>";
            XmlNode result1 = nr.Receiver(s1);
            Assert.AreEqual(result1.Name, "get-name");

            string s2 = "<initialize> <color> blue </color> <list> <color> red </color> <color> blue </color> <color> green </color> </list> </initialize>";
            XmlNode result2 = nr.Receiver(s2);
            Assert.AreEqual(result2.Name, "initialize");
            Assert.AreEqual(result2.FirstChild.Name, "color");
            Assert.AreEqual(result2.FirstChild.InnerText, "blue");

        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Invalid XML")]
        public void TestNetworkRelayReceiveInvalid()
        {
            string s = "invalid";
            nr.Receiver(s);
        }
    }
}