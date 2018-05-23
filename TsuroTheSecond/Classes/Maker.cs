using System;
using System.Xml.Linq;
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

        public XElement PawnLocXML(string horv, int x, int y) {
            XElement horvxml = this.HVXML(horv);
            XElement xxml = this.NXML(x);
            XElement yxml = this.NXML(y);

            XElement pawnlocxml = new XElement("pawn-loc", horvxml, xxml, yxml);
            return pawnlocxml; 
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
    }
}
