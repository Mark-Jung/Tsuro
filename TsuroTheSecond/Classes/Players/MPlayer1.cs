using System;
using System.Linq;
using System.Collections.Generic;
namespace TsuroTheSecond
{
    public class MPlayer1 : IPlayer
    // MPlayer with a random chooseTile strategy
    {
        private string name;
        private string color;
        private List<string> other_players;
        private enum State { start, initialized, loop, end };
        private State playerState;

        public MPlayer1(string _name)
        {
            playerState = State.start;
            name = _name;
        }

        public String GetName()
        {
            return name;
        }

        public void Initialize(string _color, List<string> colors)
        {
            if (playerState != State.start)
            {
                throw new Exception("Player should be in start state");
            }
            color = _color;
            other_players = colors;
            playerState = State.initialized;
        }

        public Position PlacePawn(Board board)
        {
            if (playerState != State.initialized) {
                throw new Exception("Player should be in initialized state");
            }
            // the board should hold other player start positions so that it can be checked
            // if other players are already at this spot
            Position position = new Position(0, -1, 5);
            while (!board.FreeTokenSpot(position)) {
                // make this thoroughly checking every position on the board
                // but for right now just check all the top tiles
                Position newPosition = new Position(position.x, position.y+1, position.port);
                if (newPosition.y > Constants.boardSize-1) {
                    throw new Exception("incomplete place pawn check");
                }
            }
            playerState = State.loop;
            //Console.WriteLine(playerState);
            return position;
        }

        public Tile PlayTurn(Board board, List<Tile> hand, int unused)
        {
            //Console.WriteLine(playerState);
            if (playerState != State.loop)
            {
                throw new Exception("Player should be in loop state");
            }
            Random random = new Random();
            // all legal options
            List<Tile> legal_options = board.AllPossibleTiles(this.color, hand);
            //Console.WriteLine(legal_options.Count);
            // all legal options, rid of overlapped.
            IDictionary<string, Tile> unique_legal_options = new Dictionary<string, Tile>();
            foreach(Tile each in legal_options){
                string path_map = each.PathMap();
                if(!(unique_legal_options.ContainsKey(path_map))){
                    unique_legal_options.Add(path_map, each);
                }
            }
            int r = random.Next(0, unique_legal_options.Count);
            return unique_legal_options.Values.ToList()[r];
        }

        public void EndGame(Board board, List<string> colors)
        {
            if (playerState != State.loop)
            {
                throw new Exception("Player is in wrong state");
            }
            playerState = State.end;
            if (colors.Contains(color)) {
                Console.WriteLine("You win!");
            } else {
                Console.WriteLine("You lose!");
            }
        }
    }
}
