using System;
using System.Xml;

namespace TsuroTheSecond
{
    public class NPlayer
    {
        public NPlayer()
        {
            
        }

        public object ReceiveCommand(XmlDocument xmlDocument)
        {
            Parser parser = new Parser();
            string command = parser.GetCommand(xmlDocument);
            switch (command)
            {
                case "get-name":
                    return command;
                case "initialize":
                    return parser.InitializeXML(xmlDocument);
                case "place-pawn":
                    return parser.PlacePawnXML(xmlDocument);
                case "play-turn":
                    return parser.PlayTurnXML(xmlDocument);
                case "end-game":
                    return parser.EndGameXML(xmlDocument);
                default:
                    throw new ArgumentException("Invalid Command Received");
            }
        }
    }
}
