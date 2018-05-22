using System;
using System.Linq;
using System.Collections.Generic;

namespace TsuroTheSecond
{
    public class RandomPlayer : MPlayer, IPlayer
    {
        public RandomPlayer(string _name) : base(_name) {
        }

        public Tile PlayTurn(Board board, List<Tile> hand, int unused)
        {
            if (playerState != State.loop)
            {
                throw new Exception("Player should be in loop state");
            }
            Random random = new Random();
            // all legal options
            List<Tile> legal_options = board.AllPossibleTiles(this.color, hand);
            // Console.WriteLine(legal_options.Count);
            // all legal options, rid of overlapped.
            IDictionary<string, Tile> unique_legal_options = new Dictionary<string, Tile>();
            foreach (Tile each in legal_options)
            {
                string path_map = each.PathMap();
                if (!(unique_legal_options.ContainsKey(path_map)))
                {
                    unique_legal_options.Add(path_map, each);
                }
            }
            int r = random.Next(0, unique_legal_options.Count);
            return unique_legal_options.Values.ToList()[r];
        }
    }
}
