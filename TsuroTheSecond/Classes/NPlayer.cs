using System;
using System.Xml;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;



namespace TsuroTheSecond
{
    public class NPlayer : IPlayer
    {
        public Parser parser = new Parser();
        public Maker maker = new Maker();
        public Wrapper wrapper = new Wrapper();
        public NetworkRelay relay;

        public Player player;
        protected string name;
        public string color;
        protected List<string> player_colors;
        public enum State { start, initialized, loop, end };
        public State playerState;

        public NPlayer(string _name, Socket socket)
        {
            this.name = _name;
            this.relay = new NetworkRelay(socket);
            this.playerState = State.start;
        }

        public void Initialize(string _color, List<string> all_colors)
        {
            if (playerState != State.start)
            {
                throw new Exception("Player should be in start state");
            }
            // make initialize xml
            XmlElement initializeXML = maker.ToXmlElement(maker.InitializeXML(_color, all_colors));

            XmlNode answer = relay.SingleRelay(initializeXML.OuterXml);
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
            XmlElement placepawnXML = maker.ToXmlElement(maker.PlacePawnXML(board));

            XmlNode answer = relay.SingleRelay(placepawnXML.OuterXml);

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
            XmlElement PlayTurnXML = maker.ToXmlElement(maker.PlayTurnXML(board, hand, deck));

            XmlNode answer = relay.SingleRelay(PlayTurnXML.OuterXml);
            Tile tile = parser.TileXML(answer);
            return tile;
        }

        public void EndGame(Board board, List<string> colors)
        {
            if (playerState != State.loop)
            {
                throw new Exception("Player is in wrong state");
            }

            XmlElement EndGameXML = maker.ToXmlElement(maker.EndGameXML(board, colors));

            XmlNode answer = relay.SingleRelay(EndGameXML.OuterXml);

            if(parser.GetCommand(answer) != "void")
            {
                throw new Exception("Expected a response xml of void tag.");
            }

            playerState = State.end;
            relay.CloseMe();
        }

        public String GetName()
        {
            // make the get-name xml call
            XmlElement GetNameXml = maker.ToXmlElement(maker.GetNameXML());
            XmlNode answer = relay.SingleRelay(GetNameXml.OuterXml);
            string _name = parser.PlayerNameXML(answer);
            return _name;
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