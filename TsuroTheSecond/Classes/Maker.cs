using System;
using System.Xml.Linq;
namespace TsuroTheSecond
{
    public class Maker
    {
        public Maker()
        {
        }

        public string PlayerName(string name){
            XElement playername = new XElement("player-name", name);
            return playername.ToString();
        }




    }
}
