using System;
using System.Xml;
using System.Collections.Generic;

namespace TsuroTheSecond
{
    public class NPlayer
    {
        public Parser parser = new Parser();
        public Wrapper wrapper = new Wrapper();
        public Player player;

        // need to fix  Player  to IPlayer. NPlayer should implement IPlayer.

        public NPlayer(IPlayer _player, string c)
        {
            this.player = new Player(_player, c);
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
                    return wrapper.GetName(this.player);
                case "initialize":
                    return wrapper.Initialize(this.player, node);
                case "place-pawn":
                    return wrapper.PlacePawn(this.player, node);
                //case "play-turn":
                    //return wrapper.PlayTurn(this.player, node);
                //case "end-game":
                    //return wrapper.EndGame(player, node);

                default:
                    throw new ArgumentException("Invalid Command Received");
            }
        }
    }
}