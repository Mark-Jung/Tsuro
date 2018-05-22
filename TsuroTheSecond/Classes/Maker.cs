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
            } else {
                XElement vxml = new XElement("v","");
                return vxml;
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

        //public XElement TilesListXML(List<(Tile, int, int)> listoftiles){
        //    XElement ListofTiles = new XElement("list", "");
        //}
    }
}
