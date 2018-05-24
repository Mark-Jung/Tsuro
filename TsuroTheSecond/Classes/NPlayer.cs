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
                case "initialize":
                    return wrapper.Initialize(player);
                case "place-pawn":
                    return wrapper.PlacePawn(player);
                case "play-turn":
                    return wrapper.PlayTurn(player);
                case "end-game":
                    return wrapper.EndGame(player);
                default:
                    throw new ArgumentException("Invalid Command Received");
            }
        }
    }
}