using System;
using TsuroTheSecond;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.Collections.Generic;

namespace TsuroTheSecond
{
    public class Wrapper
    {
        Maker maker = new Maker();
        Parser parser = new Parser();
        public Wrapper()
        {
        }

        public XmlNode GetName(PlayerProxy player)
        {
            string name = player.iplayer.GetName();
            XElement nameXelement = maker.PlayerNameXML(name);
            return maker.ToXmlNode(nameXelement);
        }

        public XmlNode Initialize(PlayerProxy player, XmlNode node)
        {
            (string color, List<string> list_of_color) = parser.InitializeXML(node);
            if(player.Color != color){
                throw new ArgumentException("Color is inconsistent");
            }
            player.iplayer.Initialize(color, list_of_color);
            return maker.ToXmlNode(maker.VoidXML());
        }

        public XmlNode PlacePawn(PlayerProxy player, XmlNode node)
        {
            (Dictionary<(int, int), Tile> TilesTobePlaced, Dictionary<string, (Position, Position)> TokenPositions) = this.parser.PlacePawnXML(node);
            Board board = this.initBoardBuilder(TilesTobePlaced, TokenPositions);
            Position pawn_loc = player.iplayer.PlacePawn(board);
            return maker.ToXmlNode(maker.PawnLocXML(pawn_loc));
        }

        public Board initBoardBuilder(Dictionary<(int, int), Tile> TilesTobePlaced, Dictionary<string, (Position, Position)> TokenPositions)
        {
            Board board = new Board(6);
            // places the tiles
            foreach (KeyValuePair<(int, int), Tile> entry in TilesTobePlaced)
            {
                board.PlaceTile(entry.Value, entry.Key.Item1, entry.Key.Item2);
            }

            Position init;
            // figures out which position is valid for starting the game
            foreach (KeyValuePair<string, (Position, Position)> entry in TokenPositions)
            {
                try
                {
                    init = new Position(entry.Value.Item1.x, entry.Value.Item1.y, entry.Value.Item1.port);
                }
                catch (ArgumentException)
                {
                    try
                    {
                        init = new Position(entry.Value.Item2.x, entry.Value.Item2.y, entry.Value.Item2.port);
                    }
                    catch (ArgumentException)
                    {
                        throw new ArgumentException("Not an initial spot at the start of game, place-pawn");
                    }

                }
                board.tokenPositions[entry.Key] = init;
            }
            return board;
        }

        public Board BoardBuilder(Dictionary<(int, int), Tile> TilesTobePlaced, Dictionary<string, (Position, Position)> TokenPositions)
        {
            Board board = new Board(6);
            Console.WriteLine("Instantiated new board!");
            // places the tiles
            foreach (KeyValuePair<(int, int), Tile> entry in TilesTobePlaced)
            {
                board.PlaceTile(entry.Value, entry.Key.Item1, entry.Key.Item2);
            }
            Console.WriteLine("Placed the tiles!");

            // figures out which position is valid for starting the game
            foreach (KeyValuePair<string, (Position, Position)> entry in TokenPositions)
            {
                if(entry.Value.Item1.IsDead()){
                    Console.WriteLine("Juk el GAK!!");
                    entry.Value.Item1.PrintMe();
                    board.tokenPositions[entry.Key] = entry.Value.Item1;
                    continue;
                } else if(entry.Value.Item2.IsDead()){
                    Console.WriteLine("Juk el GAK!!");
                    entry.Value.Item2.PrintMe();
                    board.tokenPositions[entry.Key] = entry.Value.Item2;
                    continue;
                }


                if(board.tiles[entry.Value.Item1.x][entry.Value.Item1.y] is null){
                    entry.Value.Item2.PrintMe();
                    board.tokenPositions[entry.Key] = entry.Value.Item2;
                } else {
                    entry.Value.Item1.PrintMe();
                    board.tokenPositions[entry.Key] = entry.Value.Item1;
                }
            }
            return board;
        }


        public XmlNode PlayTurn(PlayerProxy player, XmlNode node){
            (Dictionary<(int, int), Tile>TilesTobePlaced, Dictionary<string, (Position, Position)> TokenPositions, HashSet<Tile> hand, List<int> n) = parser.PlayTurnXML(node);
            Board board = this.BoardBuilder(TilesTobePlaced, TokenPositions);

            List<Tile> Hand = hand.ToList();

            Tile tile = player.iplayer.PlayTurn(board, Hand, n[0]);
            return maker.ToXmlNode(maker.TileXML(tile));
        }

        public XmlNode EndGame(PlayerProxy player, XmlNode node)
        {
            (Dictionary<(int, int), Tile> TilesTobePlaced, Dictionary<string, (Position, Position)> TokenPositions, List<string> list_of_colors) = parser.EndGameXML(node);
            Board board = BoardBuilder(TilesTobePlaced, TokenPositions);
            player.iplayer.EndGame(board, list_of_colors);
            return maker.ToXmlNode(maker.VoidXML());
        }

    }
}
