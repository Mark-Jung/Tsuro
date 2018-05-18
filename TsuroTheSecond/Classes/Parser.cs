using System;
using System.Xml.Linq;
using System.Xml;
using System.Collections.Generic;

namespace TsuroTheSecond
{
    public class Parser
    {
        public Parser()
        {
            
        }
        public string GetCommand(XmlDocument input){
            string command = input.FirstChild.Name;
            // string and object in internal game structure
            return command;
        }

        public (string, List<string>) InitializeXML(XmlDocument input){
            /*
             * <initialize> color list-of-color </initalize>
             * returns "initialize and a list of string
             */

            XmlNodeList list_of_colorXML = input.SelectSingleNode("/initialize/list").ChildNodes;
            XmlNode first = input.SelectSingleNode("/initialize").FirstChild;
            string own_color = this.ColorXML(input.SelectSingleNode("/initialize").FirstChild);

            List<string> list_of_color = new List<string>();

            foreach(XmlNode each in list_of_colorXML){
                list_of_color.Add(this.ColorXML(each));
            }

            return (own_color, list_of_color);
        }

        public string ColorXML(XmlNode colorXML){
            return colorXML.InnerText.Replace(" ", "");
        }

        public int NXML(XmlNode n){
            return int.Parse(n.InnerText);
        }

        public string HVXML(XmlNode hv){
            return hv.Name;
        }

        public (string, int, int) PawnLocXML(XmlNode pawn_loc){
            XmlNodeList pawn_child = pawn_loc.ChildNodes;
            string horv = this.HVXML(pawn_child.Item(0));
            int x = this.NXML(pawn_child.Item(1));
            int y = this.NXML(pawn_child.Item(2));
            return (horv, x, y);
        }
        


    }
}
