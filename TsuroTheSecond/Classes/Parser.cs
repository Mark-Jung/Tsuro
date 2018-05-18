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
            string own_color = input.SelectSingleNode("/initialize").FirstChild.InnerText.Replace(" ", "");

            List<string> list_of_color = new List<string>();

            foreach(XmlNode each in list_of_colorXML){
                list_of_color.Add(each.InnerText.Replace(" ", ""));
            }

            return (own_color, list_of_color);
        }
        


    }
}
