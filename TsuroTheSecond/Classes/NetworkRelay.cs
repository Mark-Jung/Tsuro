using System;
using TsuroTheSecond;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Net.Sockets;



namespace TsuroTheSecond
{
    public class NetworkRelay
    {
        public StreamWriter writer;
        public StreamReader reader;
        public NetworkStream networkStream;
        public NetworkRelay(Socket socket)
        {
            networkStream = new NetworkStream(socket);
            writer = new StreamWriter(networkStream);
            reader = new StreamReader(networkStream);

        }
        public XmlNode SingleRelay (string output)
        {
            writer.WriteLine(output);
            writer.Flush();
            string answer = reader.ReadLine();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(answer);
            XmlNode newNode = doc.DocumentElement;
            return newNode;
        }

        public void CloseMe()
        {
            writer.Close();
            networkStream.Close();
            reader.Close();
        }
    }
}