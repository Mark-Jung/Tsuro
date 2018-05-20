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
        public string GetCommand(XmlDocument input)
        {
            string command = input.FirstChild.Name;
            // string and object in internal game structure
            return command;
        }

        public (string, List<string>) InitializeXML(XmlDocument input)
        {
            /*
             * <initialize> color list-of-color </initalize>
             * returns "initialize and a list of string
             */

            XmlNodeList list_of_colorXML = input.SelectSingleNode("/initialize/list").ChildNodes;
            XmlNode first = input.SelectSingleNode("/initialize").FirstChild;
            string own_color = this.ColorXML(input.SelectSingleNode("/initialize").FirstChild);

            List<string> list_of_color = new List<string>();

            foreach (XmlNode each in list_of_colorXML)
            {
                list_of_color.Add(this.ColorXML(each));
            }

            return (own_color, list_of_color);
        }

        public string ColorXML(XmlNode colorXML)
        {
            return colorXML.InnerText.Replace(" ", "");
        }

        public int NXML(XmlNode n)
        {
            return int.Parse(n.InnerText);
        }

        public string HVXML(XmlNode hv)
        {
            return hv.Name;
        }

        public (Position, Position) PawnLocXML(XmlNode pawn_loc)
        {
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
                p_left = 3 - inp_1 % 2;
                p_right = 6 + inp_1 % 2;
                return (new Position(x_left, common_y, p_left, false), new Position(x_right, common_y, p_right, false));
            }
        }

        public Dictionary<string, (Position, Position)> PawnsXML(XmlNode pawns)
        {
            Dictionary<string, (Position, Position)> result = new Dictionary<string, (Position, Position)>();
            XmlNodeList entry_list = pawns.SelectNodes("//ent");
            foreach (XmlNode entry in entry_list)
            {
                string color = this.ColorXML(entry.FirstChild);
                (Position position, Position position1) = this.PawnLocXML(entry.LastChild);
                result.Add(color, (position, position1));
            }
            return result;
        }

    }
}
