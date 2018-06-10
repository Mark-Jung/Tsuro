using System;
using System.Collections.Generic;
using System.Xml;
using System.Net;
using System.Net.Sockets;
using System.IO;


namespace TsuroTheSecond
{
    public class PlayerProxy
    {

        Parser parser = new Parser();
        Wrapper wrapper = new Wrapper();
        public readonly string Color;
        public List<Tile> Hand;
        public IPlayer iplayer;
        public NetworkRelay networkRelay;
        public bool GameFinished;
        public Socket sender;
        public NetworkStream networkStream;

        public PlayerProxy(IPlayer player, string c, StreamWriter writer, StreamReader reader)
        {
            if (!Constants.colors.Contains(c))
            {
                throw new ArgumentException("Color not allowed");
            }
            Hand = new List<Tile>();
            iplayer = player;
            Color = c;

            networkRelay = new NetworkRelay(writer, reader);
            Console.WriteLine("Got the streams set up for proxy player");
            GameFinished = false;
        }

        public void Play()
        {
            XmlNode command = networkRelay.ListenForMe();
            XmlNode answer = Identifier(command);
            networkRelay.WriteForMe(answer.OuterXml);
            //if(this.GameFinished){
            //    networkRelay.CloseMe();
            //}
        }

        public XmlNode Identifier(XmlNode node)         {
            string command = parser.GetCommand(node);             switch (command)             {                 case "get-name":                     return wrapper.GetName(this);                 case "initialize":                     return wrapper.Initialize(this, node);                 case "place-pawn":                     return wrapper.PlacePawn(this, node);
                case "play-turn":
                    //Console.WriteLine("Going to play turn!");
                    return wrapper.PlayTurn(this, node);
                case "end-game":
                    this.GameFinished = true;
                    return wrapper.EndGame(this, node);

                default:                     throw new ArgumentException("Invalid Command Received");             }         } 
        public void AddTiletoHand(Tile tile)
        {
            if (this.Hand.Count >= 3)
            {
                // this should only be called by server ... and should break in server
                throw new Exception("Player hand is already full!");
            }
            this.Hand.Add(tile);
        }

        public void RemoveTilefromHand(Tile tile)
        {
            // this should only be called by server
            if (this.Hand.Count <= 0)
            {
                throw new Exception("Player hand is already empty!");
            }
            int hand_cnt = this.Hand.Count;
            for (int i = 0; i < this.Hand.Count; i++)
            {
                if (this.Hand[i].id == tile.id)
                {
                    this.Hand.Remove(this.Hand[i]);
                }
            }
            if (hand_cnt == this.Hand.Count)
            {
                throw new Exception("Remove Tile from hand was not effective");
            }
        }

        public bool TileinHand(Tile tile)
        {
            if (this.Hand.Find(each => each.id == tile.id) == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void ReplaceIPlayer(IPlayer player)
        {
            this.iplayer = player;
        }
    }

}
