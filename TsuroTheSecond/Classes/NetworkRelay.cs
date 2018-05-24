using System;
using TsuroTheSecond;
using System.Xml;

namespace TsuroTheSecond
{
    public class NetworkRelay
    {
        public NetworkRelay()
        {
        }

        public XmlNode Receiver (string s)
        {
            int i = 0;
            string line = "";
            while (i < s.Length && s[i] != '\n')
            {
                if (s[i] != ' ')
                    line += s[i];
                i++;
            }
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(line.ToString());
            XmlNode newNode = xmlDocument.DocumentElement;
            return newNode;
        }

    }
}
