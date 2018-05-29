using System;
using TsuroTheSecond;
using System.Xml;
using System.Xml.Linq;


namespace TsuroTheSecond
{
    public class NetworkRelay
    {
        public NetworkRelay()
        {
        }
        public XmlNode SingleRelay (string output)
        {
            Console.WriteLine(output);
            string answer = Console.ReadLine();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(answer);
            XmlNode newNode = doc.DocumentElement;
            return newNode;
        }
    }
}