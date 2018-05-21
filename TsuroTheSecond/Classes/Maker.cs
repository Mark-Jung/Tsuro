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

        public XElement ConnectXML(XElement n1, XElement n2) {
            XElement connectxml = new XElement("connect",
                                           n1,
                                           n2
                                           );
            return connectxml;
        }

        public XElement TileXML(XElement connect1, XElement connect2, XElement connect3, XElement connect4) {
            XElement tilexml = new XElement("tile",
                                            connect1,
                                            connect2,
                                            connect3,
                                            connect3,
                                            connect4
                                           );
            return tilexml;
        }

        public XElement HVXML(string horv){
            if(horv == "h"){
                XElement hxml = new XElement("h");
                return hxml;
            } else {
                XElement vxml = new XElement("v");
                return vxml;
            }
        }

        public XElement PawnLocXML(XElement horv, XElement x, XElement y) {
            XElement pawnlocxml = new XElement("pawn-loc",
                                               horv,
                                               x,
                                               y
                                              );
            return pawnlocxml; 
        }

    }
}
