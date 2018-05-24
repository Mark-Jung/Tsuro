using System;
using System.Collections.Generic;
using System.Xml;

namespace TsuroTheSecond
{
    public class NServer
    {
        Parser parser = new Parser();
        Wrapper wrapper = new Wrapper();

        public NServer()
        {

        }

        public XmlNode Identifier(XmlNode node)
        {
            string response = parser.GetCommand(node);
            switch (response)
            {
                case "player-name":
                    break;
                case "pawn-loc":
                    break;
                case "tile":
                    break;
                case "void":
                    break;
                default:
                    throw new ArgumentException("Invalid Response Received");
            }
            throw new NotImplementedException();
        }
    }
}