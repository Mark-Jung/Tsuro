using System;
using System.Xml;
using System.Collections.Generic;

namespace TsuroTheSecond
{
    public class NPlayer : IPlayer
    {
        public Parser parser = new Parser();
        public Maker maker = new Maker();
        public Wrapper wrapper = new Wrapper();
        public Player player;
        public NetworkRelay relay = new NetworkRelay();

        // need to fix  Player  to IPlayer. NPlayer should implement IPlayer.

        protected string name;
        public string color;
        protected List<string> player_colors;
        protected enum State { start, initialized, loop, end };
        protected State playerState;

        public NPlayer(string _name, string _color)
        {
            this.name = _name;
            this.playerState = State.start;
        }

        public void Initialize(string _color, List<string> all_colors)
        {
            if (playerState != State.start)
            {
                throw new Exception("Player should be in start state");
            }
            // make initialize xml
            XmlNode initializeXML = maker.ToXmlNode(maker.InitializeXML(_color, all_colors));

            XmlNode answer = relay.SingleRelay(initializeXML);
            if(parser.GetCommand(answer) == "void")
            {
                // change state to initialized
                playerState = State.initialized;
            }
            else 
            {
                throw new Exception("Got a non-void reponse when expecting void response.");
            }
        }

        public Position PlacePawn(Board board)
        {
            if (playerState != State.initialized)
            {
                throw new Exception("Player should be in initialized state");
            }
            XmlNode placepawnXML = maker.ToXmlNode(maker.PlacePawnXML(board));

            XmlNode answer = relay.SingleRelay(placepawnXML);

            // parse response and turn the game object
            (Position position1, Position position2) = parser.PawnLocXML(answer);
            Position position;
            try{
                position = new Position(position1.x, position1.y, position1.port);
            }
            catch {
                position = new Position(position2);
            }

            // change state
            playerState = State.loop;
            return position;
        }

        public Tile PlayTurn(Board board, List<Tile> hand, int deck)
        {
            if (playerState != State.loop)
            {
                throw new Exception("Player should be in loop state");
            }
            XmlNode PlayTurnXML = maker.ToXmlNode(maker.PlayTurnXML(board, hand, deck));

            XmlNode answer = relay.SingleRelay(PlayTurnXML);
            Tile tile = parser.TileXML(answer);
            return tile;
        }

        public void EndGame(Board board, List<string> colors)
        {
            if (playerState != State.loop)
            {
                throw new Exception("Player is in wrong state");
            }

            XmlNode EndGameXML = maker.ToXmlNode(maker.EndGameXML(board, colors));

            XmlNode answer = relay.SingleRelay(EndGameXML);

            if(parser.GetCommand(answer) != "void")
            {
                throw new Exception("Expected a response xml of void tag.");
            }

            playerState = State.end;
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

        public String GetName()
        {
            // make the get-name xml call
            XmlNode GetNameXml = maker.ToXmlNode(maker.GetNameXML());
            XmlNode answer = relay.SingleRelay(GetNameXml);
            return parser.PlayerNameXML(answer);
        }

        public String GetColor()
        {
            return color;
        }

        public List<string> GetOtherColors()
        {
            return player_colors;
        }
    }
}