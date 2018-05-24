using System;
using System.Xml.Linq;
using System.Xml;
using System.Collections.Generic;
namespace TsuroTheSecond
{
    public class Maker
    {
        public Maker()
        {
        }

        public XElement PlayerNameXML(string playername){
            XElement playernamexml = new XElement("player-name", playername);
            return playernamexml;
        }

        public XElement NXML(int n) {
            XElement nxml = new XElement("n", n);
            return nxml;
        }

        public XElement ConnectXML(int n1, int n2) {
            XElement n1xml = this.NXML(n1);
            XElement n2xml = this.NXML(n2);

            XElement connectxml = new XElement("connect", n1xml, n2xml);
            return connectxml;
        }

        public XElement TileXML(Tile input) {
            List<XElement> connects = new List<XElement>();
            foreach(List<int> path in input.paths){
                XElement connect = this.ConnectXML(path[0], path[1]);
                connects.Add(connect);
            }

            XElement tilexml = new XElement("tile", connects[0], connects[1], connects[2], connects[3]);
            return tilexml;
        }

        public XElement HVXML(string horv){
            if(horv == "h"){
                XElement hxml = new XElement("h","");
                return hxml;
            } else if (horv == "v"){
                XElement vxml = new XElement("v","");
                return vxml;
            } else {
                throw new ArgumentException("horv can only be either h or v");
            }
        }

        //public XElement MaybeListofSplayersXML(){
            
        //}

        public XElement XYXML(int x, int y) {
            XElement xy = new XElement("xy", new XElement("x", x), new XElement("y", y));
            return xy;
        }

        public XElement ColorXML(string color) {
            if(Constants.colors.Contains(color)){
                return new XElement("color", color);
            }
            throw new ArgumentException("given color needs to be one of the legal colors");
        }

        public XElement ListofTilesXML(List<Tile> listoftiles){
            XElement ListofTiles = new XElement("list", "");
            foreach(Tile each in listoftiles){
                ListofTiles.Add(this.TileXML(each));
            }
            return ListofTiles;
        }

        public XElement SetofTilesXML(List<Tile> listoftiles)
        {
            XElement ListofTiles = new XElement("set", "");
            foreach (Tile each in listoftiles)
            {
                ListofTiles.Add(this.TileXML(each));
            }
            return ListofTiles;
        }

        public XElement DragonSPlayerXML(Player player)
        {
            XElement dragonSplayer = new XElement("splayer-dragon", "");
            XElement color = this.ColorXML(player.Color);
            XElement setoftiles = this.SetofTilesXML(player.Hand);
            dragonSplayer.Add(color);
            dragonSplayer.Add(setoftiles);
            return dragonSplayer;
        }

        public XElement SPlayerXML(Player player){
            XElement dragonSplayer = new XElement("splayer-nodragon", "");
            XElement color = this.ColorXML(player.Color);
            XElement setoftiles = this.SetofTilesXML(player.Hand);
            dragonSplayer.Add(color);
            dragonSplayer.Add(setoftiles);
            return dragonSplayer;
        }

        public XElement ListofSPlayersXML(List<(Player, Boolean)> players){
            XElement Splayers = new XElement("list", "");
            foreach((Player each, Boolean IsDrag) in players){
                if(IsDrag){
                    Splayers.Add(this.DragonSPlayerXML(each));
                } else {
                    Splayers.Add(this.SPlayerXML(each));
                }
            }
            return Splayers;
        }

        public XElement FalseXML(){
            return new XElement("false", "");
        }

        public XElement TilesXML(List<(int, int)> locs, List<Tile> tiles)
        {
            XElement mapoftiles = new XElement("map", "");
            if(locs.Count != tiles.Count){
                throw new ArgumentException("Expected the two input lists to be the same length");
            }
            for (int i = 0; i < locs.Count; i++){
                (int x, int y) = locs[i];
                XElement xyxml = this.XYXML(x, y);
                XElement tilexml = this.TileXML(tiles[i]);
                XElement ent = new XElement("ent", "");
                ent.Add(xyxml);
                ent.Add(tilexml);
                mapoftiles.Add(ent);
            }
            return mapoftiles;
        }

        public XElement PawnLocXML(Position position)
        {
            string horv = "";
            int first_arg, second_arg;
            List<int> horizontal_ports = new List<int> { 0, 1, 4, 5 };
            List<int> vertical_ports = new List<int> { 2, 3, 6, 7 };
            List<int> one_plus = new List<int> { 2, 3, 4, 5 };
            List<int> one_plusto2 = new List<int> { 1, 4, 6, 3 };
            if(horizontal_ports.Contains(position.port)) {
                horv = "h";
                first_arg = position.y;
                second_arg = 2 * position.x;
            } else if (vertical_ports.Contains(position.port)){
                horv = "v";
                first_arg = position.x;
                second_arg = 2 * position.y;
            } else {
                throw new ArgumentException("Port number should range from 0 to 7");
            }

            if(one_plus.Contains(position.port)){
                first_arg++;
            }
            if(one_plusto2.Contains(position.port)){
                second_arg++;
            }
            XElement pawnloc = new XElement("pawn-loc", "");
            pawnloc.Add(this.HVXML(horv));
            pawnloc.Add(this.NXML(first_arg));
            pawnloc.Add(this.NXML(second_arg));

            return pawnloc;
        }

        public XElement PawnsXML(Dictionary<string, Position> tokenPositions) {
            XElement pawns = new XElement("map", "");
            foreach(KeyValuePair<string, Position>entry in tokenPositions){
                XElement ent = new XElement("ent", "");
                ent.Add(this.ColorXML(entry.Key));
                ent.Add(this.PawnLocXML(entry.Value));
                pawns.Add(ent);
            }
            return pawns;
        }

        public XElement BoardXML(Board board)
        {
            XElement boardxml = new XElement("board", "");
            List < (int, int) > loc = new List<(int, int)>();
            List<Tile> placed_tiles = new List<Tile>();
            for (int i = 0; i < 6; i++){
                for (int j = 0; j < 6; j++){
                    if(board.tiles[i][j] is null){
                        continue;
                    } else{
                        loc.Add((i, j));
                        placed_tiles.Add(board.tiles[i][j]);
                    }
                }
            }
            XElement tilesxml = this.TilesXML(loc, placed_tiles);
            XElement pawnsxml = this.PawnsXML(board.tokenPositions);
            boardxml.Add(tilesxml);
            boardxml.Add(pawnsxml);
            return boardxml;
        }

        public XElement VoidXML()
        {
            return new XElement("void", "");
        }

        public XmlNode ToXmlNode(XElement input){
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(input.ToString());
            XmlNode xmlNode = xmlDocument.FirstChild;
            return xmlNode;
        }
    }
}
