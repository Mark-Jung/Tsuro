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
            string input = null;
            while(input is null){
                input = Console.ReadLine();
            }
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(input);
            return doc.DocumentElement;
        }

        public void play_a_turn()
        {
            Parser parser = new Parser();
            Maker maker = new Maker();
            Wrapper wrapper = new Wrapper();
            while(true){
                Server server = new Server();

                server.gameState = Server.State.safe;
                //while(server.gameState != Server.State.end){
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

                int dragon_index = -1;
                int cnt = 0;
                // alive
                foreach (KeyValuePair<string, (List<Tile>, Boolean)> entry in alive_incomplete)
                {
                    Player player = new Player(new MostSymmetricPlayer(entry.Key), entry.Key);
                    player.Hand = entry.Value.Item1;
                    if (entry.Value.Item2)
                    {
                        dragon_index = cnt;
                    }
                    server.alive.Add(player);
                    cnt++;
                }
                // populate server dragon queue
                // 1. add less than 3 cards holders AFTER the dragon holder
                // 2. add less than 3 cards holders BEFORE the dragon holder
                // edge case: 
                // the first player is the dragon holder. -> Divide it into 
                // 3 big cases.
                /* 1. from the found dragon holder(including) to the end. 
                 *    include underpopulated hands
                 * 2. check if the first hand is underpopulated(2). 
                 *    special since it is expected to have 2, not 3. However,
                 *    this first element cannot be the dragon holder(double count with 1.)
                 * 3. from index 1 to the dragon holder index.
                 */      
                if(dragon_index >= 0)
                {
                    for (int i = dragon_index; i < server.alive.Count; i++)
                    {
                        if (server.alive[i].Hand.Count < 3)
                        {
                            server.dragonQueue.Add(server.alive[i]);
                        }
                    }

                    if(server.alive[0].Hand.Count < 2 && dragon_index != 0)
                    {
                        server.dragonQueue.Add(server.alive[0]);
                    }

                    for (int i = 1; i < dragon_index; i++)
                    {
                        if (server.alive[i].Hand.Count < 3)
                        {
                            server.dragonQueue.Add(server.alive[i]);
                        }
                    }
                }


                // dead
                foreach (KeyValuePair<string, (List<Tile>, Boolean)> entry in dead_incomplete)
                {
                    Player player = new Player(new MostSymmetricPlayer(entry.Key), entry.Key);
                    player.Hand = entry.Value.Item1;
                    server.dead.Add(player);
                }

                foreach (KeyValuePair<(int, int), Tile> entry in TilesTobePlaced)
                {
                    server.board.PlaceTile(entry.Value, entry.Key.Item1, entry.Key.Item2);
                }
                // board
                server.board = wrapper.BoardBuilder(TilesTobePlaced, TokenPositions);
                server.deck = deck;
                //server.alive[0].RemoveTilefromHand(tile_toplay);



                // run play-a-turn
                (List<Tile> _deck, List<Player> _alive, List<Player> _dead, Board _board, Boolean GG, List<Player> victors) = server.PlayATurn(server.deck, server.alive, server.dead, server.board, tile_toplay);

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
                }
                else
                {
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
                if (GG)
                {
                    Console.WriteLine(maker.ToXmlNode(maker.ListofSPlayersXML(dragon_victors)).OuterXml);
                }
                else
                {
                    Console.WriteLine("<false></false>");
                }
            }

        }
    }
}
