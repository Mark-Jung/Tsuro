using System;
using System.Linq;
using System.Collections.Generic;

namespace TsuroTheSecond
{
    public class MostSymmetricPlayer : MPlayer, IPlayer
    {
        public MostSymmetricPlayer(string _name) : base(_name)
        {
        }


        public Tile PlayTurn(Board board, List<Tile> hand, int unused)
        {
            if (playerState != State.loop)
            {
                throw new Exception("Player should be in loop state");
            }
            // all legal options
            List<Tile> legal_options = board.AllPossibleTiles(this.color, hand);

            // all legal options, rid of overlapped.
            IDictionary<string, Tile> unique_legal_options = new Dictionary<string, Tile>();
            //Console.WriteLine("Going into For loop to find unique tiles");
            foreach (Tile each in legal_options)
            {
                string path_map = each.PathMap();
                if (!(unique_legal_options.ContainsKey(path_map)))
                {
                    unique_legal_options.Add(path_map, each);
                }
            }
            // new list of legal tiles sorted by symmetricity, descending.
            List<Tile> sorted_legal_options = unique_legal_options.Values.ToList().OrderBy(obj => obj.symmetricity).ToList();
            //Console.WriteLine("legal options count: " + sorted_legal_options.Count);
            return sorted_legal_options[0];
        }
    }
}
