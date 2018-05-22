using System;
using System.Collections.Generic;
namespace TsuroTheSecond.Classes
{
    public class Tournament
    {
        public Tournament()
        {
        }

        public void Play(){
            // make server
            Server server = new Server();

            // add players
            RandomPlayer mplayer1 = new RandomPlayer("Adam");
            MostSymmetricPlayer mplayer2 = new MostSymmetricPlayer("John");
            LeastSymmetricPlayer mplayer3 = new LeastSymmetricPlayer("Cathy");

            server.AddPlayer(mplayer1, "blue");
            server.AddPlayer(mplayer1, "green");
            server.AddPlayer(mplayer1, "hotpink");

            // init positions of players
            server.InitPlayerPositions();

            server.ShuffleDeck(server.deck);

            // game loop
            bool game = true;
            while (game && server.alive.Count > 0)
            {
                Player currentPlayer = server.alive[0];
                Console.WriteLine("Now " + currentPlayer.iplayer.GetName() + "'s turn.");
                // choose what tile to play
                Tile playTile = currentPlayer.iplayer.PlayTurn(server.board, currentPlayer.Hand, server.deck.Count);
                Console.WriteLine("Tile to be played is: ");
                playTile.PrintMe();
                // take that tile out of hand
                currentPlayer.RemoveTilefromHand(playTile);
                // play that tile
                (List<Tile> _deck, List<Player> _alive, List<Player> _dead, Board _board, Boolean GameDone, List<Player> Victors) = server.PlayATurn(server.deck, server.alive, server.dead, server.board, playTile);

                if (GameDone) {
                    server.WinGame(Victors);
                }
                game = !GameDone;
            }

        }
    }
}
