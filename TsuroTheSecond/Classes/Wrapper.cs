using System;
using TsuroTheSecond;
using System.Xml;
using System.Xml.Linq;

namespace TsuroTheSecond
{
    public class Wrapper
    {
        Maker maker = new Maker();
        public Wrapper()
        {
        }

        public XmlNode GetName(Player player){
            string name = player.iplayer.GetName();
            XElement nameXelement = maker.PlayerNameXML(name);
            return maker.ToXmlNode(nameXelement);
        }


    }
}
