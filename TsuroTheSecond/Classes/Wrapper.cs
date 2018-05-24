using System;
using TsuroTheSecond;
using System.Xml;
using System.Xml.Linq;
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

        public XmlNode GetName(Player player)
        {
            string name = player.iplayer.GetName();
            XElement nameXelement = maker.PlayerNameXML(name);
            return maker.ToXmlNode(nameXelement);
        }

        public XmlNode Initialize(Player player, XmlNode node)
        {
            (string color, List<string> list_of_color) = parser.InitializeXML(node);
            player.iplayer.Initialize(color, list_of_color);
            return maker.ToXmlNode(maker.VoidXML());
        }

        public XmlNode PlacePawn(Player player, XmlNode node)
        {
            (Dictionary<(int, int), Tile> TilesTobePlaced, Dictionary<string, (Position, Position)> TokenPositions) = this.parser.PlacePawnXML(node);
            Board board = new Board(6);
            // places the tiles
            foreach(KeyValuePair<(int, int),Tile> entry in TilesTobePlaced){
                board.PlaceTile(entry.Value, entry.Key.Item1, entry.Key.Item2);
            }

            Position init;
            // figures out which position is valid for starting the game
            foreach(KeyValuePair<string, (Position, Position)> entry in TokenPositions){
                try
                {
                    init = new Position(entry.Value.Item1.x, entry.Value.Item1.y, entry.Value.Item1.port);
                }
                catch (ArgumentException){
                    try{
                        init = new Position(entry.Value.Item2.x, entry.Value.Item2.y, entry.Value.Item2.port);
                    }
                    catch(ArgumentException){
                        throw new ArgumentException("Not an initial spot at the start of game, place-pawn");
                    }
                    
                }
                board.tokenPositions[entry.Key] = init;
            }
            Position pawn_loc = player.iplayer.PlacePawn(board);
            return maker.ToXmlNode(maker.PawnLocXML(pawn_loc));
        }


    }
}
