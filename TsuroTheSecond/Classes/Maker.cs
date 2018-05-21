using System;
using System.Xml.Linq;
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

        public XElement TileXML(int port1, int port2, int port3, int port4, int port5, int port6, int port7, int port8) {
            XElement connect1 = this.ConnectXML(port1, port2);
            XElement connect2 = this.ConnectXML(port3, port4);
            XElement connect3 = this.ConnectXML(port5, port6);
            XElement connect4 = this.ConnectXML(port7, port8);

            XElement tilexml = new XElement("tile", connect1, connect2, connect3, connect4);
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

    }
}
