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
        public Socket handler;
        public NetworkRelay(StreamWriter _writer, StreamReader _reader)
        {
            writer = _writer;
            reader = _reader;

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
            handler.Shutdown(SocketShutdown.Both);  
            handler.Close();  
        }

        public XmlNode ListenForMe()
        {
            string answer = reader.ReadLine();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(answer);
            XmlNode newNode = doc.DocumentElement;
            return newNode;
        }
        public void WriteForMe(string output)
        {
            writer.WriteLine(output);
            writer.Flush();
        }
    }
}