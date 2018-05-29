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

        public XmlNode Receiver(string s)
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
            try
            {
                xmlDocument.LoadXml(line.ToString());
            }
            catch (Exception e)
            {
                throw new ArgumentException("Invalid XML");
            }
            XmlNode newNode = xmlDocument.DocumentElement;
            return newNode;
        }
        public XmlNode SingleRelay (XmlNode output)
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