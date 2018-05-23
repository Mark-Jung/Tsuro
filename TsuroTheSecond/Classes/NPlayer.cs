using System;
using System.Xml;

namespace TsuroTheSecond
{
    public class NPlayer
    {
        Player player;
        Parser parser = new Parser();
        Wrapper wrapper = new Wrapper();
        public NPlayer()
        {
            
        }

        public XmlNode Identifier(XmlNode node)
        {
            /*
             * Accepts 
             */

            string command = parser.GetCommand(node);
            switch (command)
            {
                case "get-name":
                    return wrapper.GetName(player);
                //case "initialize":
                //    return parser.InitializeXML(xmlDocument);
                //case "place-pawn":
                //    return parser.PlacePawnXML(xmlDocument);
                //case "play-turn":
                //    return parser.PlayTurnXML(xmlDocument);
                //case "end-game":
                    //return parser.EndGameXML(xmlDocument);
                default:
                    throw new ArgumentException("Invalid Command Received");
            }
        }
    }
}
