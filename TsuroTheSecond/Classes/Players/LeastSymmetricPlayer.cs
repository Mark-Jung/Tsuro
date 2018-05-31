using System;
using System.Linq;
using System.Collections.Generic;

namespace TsuroTheSecond
{
    public class LeastSymmetricPlayer : IPlayer
    {
        protected string name;
        public string color;
        protected List<string> player_colors;
        public enum State { start, initialized, loop, end };
        public State playerState;

        public LeastSymmetricPlayer(string _name)
        {
            playerState = State.start;
            name = _name;
        }

        public String GetName()
        {
            return name;
        }

        public String GetColor()
        {
            return color;
        }

        public List<string> GetOtherColors()
        {
            return player_colors;
        }

        public void Initialize(string _color, List<string> all_colors)
        {
            if (playerState != State.start)
            {
                throw new Exception("Player should be in start state");
            }
            color = _color;
            player_colors = all_colors;
            playerState = State.initialized;
        }

        public Position PlacePawn(Board board)
        {
            if (playerState != State.initialized)
            {
                throw new Exception("Player should be in initialized state");
            }
            // the board should hold other player start positions so that it can be checked
            // if other players are already at this spot
            Position position = new Position(0, -1, 5);
            while (!board.FreeTokenSpot(position))
            {
                // make this thoroughly checking every position on the board
                // but for right now just check all the top tiles
                position.x++;
                if (position.x > Constants.boardSize - 1)
                {
                    throw new Exception("incomplete place pawn check");
                }
            }
            playerState = State.loop;
            return position;
        }

        public void EndGame(Board board, List<string> colors)
        {
            if (playerState != State.loop)
            {
                throw new Exception("Player is in wrong state");
            }
            playerState = State.end;
            if (board.IsDead(this.color))
            {
                Console.WriteLine("You Lost!!");
            }
          
        }

        public Tile PlayTurn(Board board, List<Tile> hand, int unused)
        {
            if (playerState != State.loop)
            {
                throw new Exception("Player should be in loop state");
            }
            // all legal options
            List<Tile> legal_options = board.AllPossibleTiles(this.color, hand);
            if(legal_options.Count == 0)
            {
                return new Tile(-1, new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 });
            }
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
            // new list of legal tiles sorted by symmetricity.
            List<Tile> sorted_legal_options = unique_legal_options.Values.ToList().OrderByDescending(obj => obj.symmetricity).ToList();
            return sorted_legal_options[0];
        }
    }
}
