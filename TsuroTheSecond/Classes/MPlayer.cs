using System;
using System.Collections.Generic;
namespace TsuroTheSecond
{
    public class MPlayer
    {
        protected string name;
        string color;
        protected List<string> player_colors;
        protected enum State { start, initialized, loop, end };
        protected State playerState;

        public MPlayer(string _name)
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
            if(board.IsDead(this.color)){
                Console.WriteLine("You Win!!");
            }
            //if (colors.Contains(color))
            //{
            //    Console.WriteLine("You win!");
            //}
            //else
            //{
            //    Console.WriteLine("You lose!");
            //}
        }
    }
}
