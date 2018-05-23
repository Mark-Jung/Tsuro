using System;
using System.Xml.Linq;
using System.Linq;
using System.Xml;
using System.Collections.Generic;

namespace TsuroTheSecond
{
    public class Parser
    {
        public Parser()
        {

        }
        public string GetCommand(XmlDocument input)
        {
            string command = input.FirstChild.Name;
            // string and object in internal game structure
            return command;
        }

        public (string, List<string>) InitializeXML(XmlNode input)
        {
            /*
             * <initialize> color list-of-color </initialize>
             * returns "initialize and a list of string
             */
            string name = input.Name;
            if(input.Name != "initialize"){
                throw new ArgumentException("Expected initialize tag.");
            }
            XmlNode first = input.FirstChild;
            if(first.Name != "color"){
                throw new ArgumentException("Expected color as first tag");
            }
            string own_color = this.ColorXML(first);

            XmlNode listofcolorsXML = input.LastChild;
            if(listofcolorsXML.Name != "list") {
                throw new ArgumentException("Expected list of color as the second tag");
            }

            List<string> list_of_color = new List<string>();
            foreach (XmlNode each in listofcolorsXML)
            {
                list_of_color.Add(this.ColorXML(each));
            }

            return (own_color, list_of_color);
        }

        public string ColorXML(XmlNode colorXML)
        {
            if(colorXML.Name != "color"){
                throw new ArgumentException("Expected color tag.");
            }
            return colorXML.InnerText.Replace(" ", "");
        }

        public int NXML(XmlNode n)
        {
            if(n.Name != "n"){
                throw new ArgumentException("Expected n tag");
            }
            return int.Parse(n.InnerText);
        }

        public string HVXML(XmlNode hv)
        {
            if( !(hv.Name == "v" || hv.Name == "h")){
                throw new ArgumentException("Expected h or v tag");
            }
            return hv.Name;
        }

        public (Position, Position) PawnLocXML(XmlNode pawn_loc)
        {
            if (pawn_loc.Name != "pawn-loc") {
                throw new ArgumentException("Expected pawn-loc tag");
            }
            XmlNodeList pawn_child = pawn_loc.ChildNodes;
            string horv = this.HVXML(pawn_child.Item(0));
            int inp_0 = this.NXML(pawn_child.Item(1));
            int inp_1 = this.NXML(pawn_child.Item(2));
            if (horv is "h")
            {
                int common_x, y_up, y_down, p_up, p_down;
                common_x = inp_1 / 2;
                y_up = inp_0 - 1;
                y_down = inp_0;
                p_up = 5 - inp_1 % 2;
                p_down = 0 + inp_1 % 2;
                return (new Position(common_x, y_up, p_up, false), new Position(common_x, y_down, p_down, false));
            }
            else
            {
                int common_y, x_left, x_right, p_left, p_right;
                common_y = inp_1 / 2;
                x_left = inp_0 - 1;
                x_right = inp_0;
                p_left = 2 + inp_1 % 2;
                p_right = 7 - inp_1 % 2;
                return (new Position(x_left, common_y, p_left, false), new Position(x_right, common_y, p_right, false));
            }
        }

        public Dictionary<string, (Position, Position)> PawnsXML(XmlNode pawns)
        {
            if(pawns.Name != "map"){
                throw new ArgumentException("Expected map tag for pawns");
            }
            Dictionary<string, (Position, Position)> result = new Dictionary<string, (Position, Position)>();
            XmlNodeList entry_list = pawns.SelectNodes("ent");
            foreach (XmlNode entry in entry_list)
            {
                string color = this.ColorXML(entry.FirstChild);
                (Position position, Position position1) = this.PawnLocXML(entry.LastChild);
                result.Add(color, (position, position1));
            }
            return result;
        }

        public List<int> ConnectXML(XmlNode connect)
        {
            if(connect.Name != "connect"){
                throw new ArgumentException("Expected connect tag");
            }

            XmlNodeList n_list = connect.ChildNodes;
            List<int> result = new List<int>();
            foreach (XmlNode n in n_list)
            {
                result.Add(this.NXML(n));
            }
            return result;
        }

        public Tile TileXML(XmlNode tile)
        {
            if(tile.Name != "tile"){
                throw new ArgumentException("Expected tile tag");
            }
            XmlNodeList connect_list = tile.ChildNodes;
            List<int> path = new List<int>();
            foreach (XmlNode connect in connect_list)
            {
                path.AddRange(this.ConnectXML(connect));
            }
            Tile result = new Tile(-1, path);
            foreach (Tile each in Constants.tiles)
            {
                if (!each.IsDifferent(result))
                {
                    result.id = each.id;
                    break;
                }
            }
            return result;
        }

        public (int, int) XYXML(XmlNode xy)
        {
            string tagname = xy.Name;
            if (tagname != "xy")
            {
                throw new ArgumentException("Wrong tag passed in. Expected xy.");
            }
            XmlNodeList XandY = xy.ChildNodes;
            string xname = XandY.Item(0).Name;
            string yname = XandY.Item(1).Name;
            if (xname != "x" && yname != "y")
            {
                throw new ArgumentException("Wrong tag. Excpected x and y tag in an xy tag.");
            }

            int x = Convert.ToInt32(XandY.Item(0).InnerText);
            int y = Convert.ToInt32(XandY.Item(1).InnerText);
            return (x, y);
        }

        public Dictionary<(int, int), Tile> TilesXML(XmlNode tiles){
            if(tiles.Name != "map"){
                throw new ArgumentException("Expected map tag for tiles");
            }
            Dictionary<(int, int), Tile> result = new Dictionary<(int, int), Tile>();
            XmlNodeList entry_list = tiles.ChildNodes;
            foreach(XmlNode entry in entry_list){
                XmlNodeList XY_Tile = entry.ChildNodes;
                (int x, int y) = this.XYXML(XY_Tile.Item(0));
                Tile tile = this.TileXML(XY_Tile.Item(1));
                result.Add((x, y), tile);
            }
            return result;
        }

        public (Dictionary<(int, int), Tile>, Dictionary<string, (Position, Position)>) BoardXML(XmlNode board)
        {
            if(board.Name != "board"){
                throw new ArgumentException("Expected board tag");
            }
            Dictionary<(int, int), Tile> TilesTobePlaced = this.TilesXML(board.FirstChild);
            Dictionary<string, (Position, Position)> TokenPositions = this.PawnsXML(board.LastChild);
            return (TilesTobePlaced, TokenPositions);
        }

        public (Dictionary<(int, int), Tile>, Dictionary<string, (Position, Position)>) PlacePawnXML(XmlNode place_pawn)
        {
            if(place_pawn.Name != "place-pawn"){
                throw new ArgumentException("Expected place-pawn tag");
            }
            XmlNode board = place_pawn.FirstChild;
            Dictionary<(int, int), Tile> TilesTobePlaced = this.TilesXML(board.FirstChild);
            Dictionary<string, (Position, Position)> TokenPositions = this.PawnsXML(board.LastChild);
            return (TilesTobePlaced, TokenPositions);
        }

        public HashSet<Tile> SetofTilesXML(XmlNode SetofTiles){
            if(SetofTiles.Name != "set"){
                throw new ArgumentException("Expected set tag for set of tiles");
            }
            HashSet<Tile> result = new HashSet<Tile>();
            Boolean same = false;
            XmlNodeList tilesXML = SetofTiles.ChildNodes;

            foreach( XmlNode tileXML in tilesXML){
                Tile newTile = this.TileXML(tileXML);
                foreach(Tile each in result){
                    if(!each.IsDifferent(newTile)){
                        same = true;
                        break;
                    }
                }
                if(!same){
                    result.Add(newTile);
                }
            }
            return result;
        }

        public List<Tile> ListofTilesXML(XmlNode TileList)
        {
            if(TileList.Name != "list") {
                throw new ArgumentException("Expected list tag for TileList");
            }
            List<Tile> result = new List<Tile>();
            XmlNodeList tilesXML = TileList.ChildNodes;

            foreach (XmlNode tileXML in tilesXML)
            {
                Tile newTile = this.TileXML(tileXML);
                result.Add(newTile);
            }
            return result;
        }

        public (Dictionary<(int, int), Tile>, Dictionary<string, (Position, Position)>, HashSet<Tile>, List<int>) PlayTurnXML(XmlNode playturn)
        {
            if(playturn.Name != "play-turn"){
                throw new ArgumentException("expected play-turn tag here");
            }
            XmlNode board = playturn.FirstChild;
            XmlNode set_of_tiles = playturn.SelectSingleNode("/play-turn/set");
            XmlNodeList numsXML = playturn.SelectNodes("/play-turn/n");
            List<int> nums = new List<int>();
            foreach(XmlNode num in numsXML) {
                nums.Add(this.NXML(num));

            }

            (Dictionary<(int, int), Tile> TilesTobePlaced, Dictionary<string, (Position, Position)> TokenPositions) = this.BoardXML(board);
            return (TilesTobePlaced, TokenPositions, this.SetofTilesXML(set_of_tiles), nums);
        }

        public (string, List<Tile>, Boolean) SPlayerXML(XmlNode splayer)
        {
            if(splayer.Name != "splayer-dragon" && splayer.Name != "splayer-nodragon"){
                throw new ArgumentException("Expected either splayer-dragon or splayer-nodragon");
            }
            XmlNode color = splayer.FirstChild;
            XmlNode setoftiles = splayer.LastChild;
            return (this.ColorXML(color), this.SetofTilesXML(setoftiles).ToList(), splayer.Name == "splayer-dragon");
        }

        public Dictionary<string, (List<Tile>, Boolean)> ListSPlayerXML(XmlNode listofsplayer)
        {
            if (listofsplayer.Name != "list")
            {
                throw new ArgumentException("Expected list tag for the list of splayer");
            }
            Dictionary<string, (List<Tile>, Boolean)> result = new Dictionary<string, (List<Tile>, bool)>();
            foreach (XmlNode splayer in listofsplayer.ChildNodes)
            {
                (string color, List<Tile> hand, Boolean isdragon) = this.SPlayerXML(splayer);
                result.Add(color, (hand, isdragon));
            }
            return result;
        }
        public (Dictionary<(int, int), Tile>, Dictionary<string, (Position, Position)>, List<string>) EndGameXML (XmlNode endgame)
        {
            if (endgame.Name != "end-game")
                throw new ArgumentException("Expected end-game tag");

            XmlNode board = endgame.FirstChild;
            if (board.Name != "board")
                throw new ArgumentException("Expected board as first tag");

            XmlNode listofcolorsXML = endgame.LastChild;
            if (listofcolorsXML.Name != "list")
                throw new ArgumentException("Expected list of color as the second tag");

            List<string> list_of_color = new List<string>();
            foreach (XmlNode each in listofcolorsXML)
            {
                if (each.Name != "color")
                    throw new ArgumentException("Expected color in list");
                list_of_color.Add(this.ColorXML(each));
            }
            (Dictionary<(int, int), Tile> TilesTobePlaced, Dictionary<string, (Position, Position)> TokenPositions) = this.BoardXML(board);
            return (TilesTobePlaced, TokenPositions, list_of_color);
        }
    }
}
