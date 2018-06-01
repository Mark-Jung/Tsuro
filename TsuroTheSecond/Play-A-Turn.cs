using System;
using System.Collections.Generic;
using System.Xml;


namespace TsuroTheSecond
{
    public class Play_A_Turn
    {
        public Play_A_Turn()
        {
        }

        public static XmlNode ReadAsXML()
        {
            string input = Console.ReadLine();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(input);
            return doc.DocumentElement;
        }

        static void Main(string[] args)
        {
            Parser parser = new Parser();
            Maker maker = new Maker();
            Wrapper wrapper = new Wrapper();
            while(true){
                Server server = new Server();

                server.gameState = Server.State.loop;
                // deck, alive, dead, board, and the tile to play
                XmlNode tiles = ReadAsXML();
                XmlNode list_of_splayer1 = ReadAsXML();
                XmlNode list_of_splayer2 = ReadAsXML();
                XmlNode in_board = ReadAsXML();
                XmlNode _tile = ReadAsXML();


                // prepare for play-a-turn
                // 1. turn to semi game object
                // 2. fully fledged game object
                List<Tile> deck = parser.ListofTilesXML(tiles);
                Dictionary<string, (List<Tile>, Boolean)> alive_incomplete = parser.ListSPlayerXML(list_of_splayer1);
                Dictionary<string, (List<Tile>, Boolean)> dead_incomplete = parser.ListSPlayerXML(list_of_splayer2);
                (Dictionary<(int, int), Tile> TilesTobePlaced, Dictionary<string, (Position, Position)> TokenPositions) = parser.BoardXML(in_board);
                Tile tile_toplay = parser.TileXML(_tile);

                foreach(KeyValuePair<string, (List<Tile>, Boolean)> entry in alive_incomplete){
                    Player player = new Player(new MostSymmetricPlayer(entry.Key), entry.Key);
                    player.Hand = entry.Value.Item1;
                    if(entry.Value.Item2){
                        server.dragonQueue.Add(player);
                    }
                    server.alive.Add(player);
                }

                // alive
                foreach (KeyValuePair<string, (List<Tile>, Boolean)> entry in dead_incomplete)
                {
                    Player player = new Player(new MostSymmetricPlayer(entry.Key), entry.Key);
                    player.Hand = entry.Value.Item1;
                    server.dead.Add(player);
                }

                // dead
                foreach (KeyValuePair<(int, int), Tile> entry in TilesTobePlaced)
                {
                    server.board.PlaceTile(entry.Value, entry.Key.Item1, entry.Key.Item2);
                }
                // board
                server.board = wrapper.BoardBuilder(TilesTobePlaced, TokenPositions);
                server.deck = deck;


                // run play-a-turn
                (List<Tile>_deck, List<Player>_alive, List<Player>_dead, Board _board, Boolean GG, List<Player> victors) = server.PlayATurn(deck, server.alive, server.dead, server.board, tile_toplay);

                List<(Player, Boolean)> whether_dragon_holder_alive = new List<(Player, bool)>();
                List<(Player, Boolean)> whether_dragon_holder_dead = new List<(Player, bool)>();
                List<(Player, Boolean)> dragon_victors = new List<(Player, bool)>();

                if (server.dragonQueue.Count > 0)
                {
                    string dragon_color = server.dragonQueue[0].Color;
                    foreach (Player each in _alive)
                    {
                        whether_dragon_holder_alive.Add((each, (each.Color == dragon_color)));
                    }
                    foreach (Player each in victors)
                    {
                        dragon_victors.Add((each, (each.Color == dragon_color)));
                    }
                } else {
                    foreach (Player each in _alive)
                    {
                        whether_dragon_holder_alive.Add((each, false));
                    }
                    foreach (Player each in victors)
                    {
                        dragon_victors.Add((each, false));
                    }
                }
                foreach (Player each in _dead)
                {
                    whether_dragon_holder_dead.Add((each, false));
                }
                       
                // change output object to xml
                // writes deck
                Console.WriteLine(maker.ToXmlNode(maker.ListofTilesXML(_deck)).OuterXml);
                // writes alive
                Console.WriteLine(maker.ToXmlNode(maker.ListofSPlayersXML(whether_dragon_holder_alive)).OuterXml);
                // writes dead
                Console.WriteLine(maker.ToXmlNode(maker.ListofSPlayersXML(whether_dragon_holder_dead)).OuterXml);
                // writes board
                Console.WriteLine(maker.ToXmlNode(maker.BoardXML(_board)).OuterXml);
                if(GG){
                    Console.WriteLine("<false></false>");
                } else {
                    Console.WriteLine(maker.ToXmlNode(maker.ListofSPlayersXML(dragon_victors)).OuterXml);
                }

            }
        }
    }
}
