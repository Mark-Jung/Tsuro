using System;
using System.Xml.Linq;
using System.Xml;

namespace TsuroTheSecond
{
    public class Parser
    {
        public Parser()
        {
            
        }
        public (string, object) FromXML(XmlDocument input){
            string tag_type = input.FirstChild.Name;
            // string and object in internal game structure
            switch (tag_type){
                case "get-name":
                    return this.Get_NameXML();
                    break;
                case "initialize":
                    break;
                case "place-pawn":
                    break;
                case "play-turn":
                    break;
                case "end-game":
                    break;
                default:
                    throw new Exception("Unknown tag for message. Cannot parse.");



            }
            return (tag_type, null);
        }
        public Board BoardXML(XmlNode board){
            Board new_board = new Board(6);
            //Place tiles for each tag
            //new_board.PlaceTile();
            //Record pawns by calling pawn-loc

            return new_board;
        }
        public (string, object) Get_NameXML(){
            return ("get-name", null);
        }


    }
}
