using System;
using System.Collections.Generic;
namespace TsuroTheSecond
{
    public class Tournament
    {
        public Tournament()
        {
        }

        public void Play(int player_cnt){
            // make server
            Server server = new Server();

            // add players
            RandomPlayer mplayer1 = new RandomPlayer("Adam");
            MostSymmetricPlayer mplayer2 = new MostSymmetricPlayer("John");
            LeastSymmetricPlayer mplayer3 = new LeastSymmetricPlayer("Cathy");

            server.AddPlayer(mplayer1, "blue");
            server.AddPlayer(mplayer2, "green");
            server.AddPlayer(mplayer3, "hotpink");

            // init positions of players
            server.InitPlayerPositions();

            server.ShuffleDeck(server.deck);

            // game loop
            bool game = true;
            while (game && server.alive.Count > 0)
            {
                Player currentPlayer = server.alive[0];
                Console.WriteLine("\n");
                Console.WriteLine("\n");
                Console.WriteLine("Now " + currentPlayer.iplayer.GetName() + "'s turn.");
                Console.WriteLine("Position is: ");
                server.board.tokenPositions[currentPlayer.Color].PrintMe();

                // choose what tile to play
                Console.WriteLine("Choosing Tile to play from");
                foreach(Tile each in currentPlayer.Hand){
                    each.PrintMe();
                }

                Tile playTile = currentPlayer.iplayer.PlayTurn(server.board, currentPlayer.Hand, server.deck.Count);
                Console.WriteLine("Tile to be played is: ");
                playTile.PrintMe();
                while(!server.LegalPlay(currentPlayer, server.board, playTile)){
                    Console.WriteLine("Seems like that tile wasn't legal! Going into LegalPlay loop");
                    playTile = currentPlayer.iplayer.PlayTurn(server.board, currentPlayer.Hand, server.deck.Count);

                }
                // take that tile out of hand
                currentPlayer.RemoveTilefromHand(playTile);
                // play that tile
                (List<Tile> _deck, List<Player> _alive, List<Player> _dead, Board _board, Boolean GameDone, List<Player> Victors) = server.PlayATurn(server.deck, server.alive, server.dead, server.board, playTile);

                Console.WriteLine("Turn Summary:");
                Console.WriteLine("Deck count: " + server.deck.Count);
                Console.WriteLine("Alive count: " + server.alive.Count);
                Console.WriteLine("Position after turn: ");
                server.board.tokenPositions[currentPlayer.Color].PrintMe();
                Console.WriteLine("Survivor list: ");
                foreach(Player survived in server.alive){
                    Console.WriteLine(survived.iplayer.GetName());
                }
                Console.WriteLine("Dead count: " + server.dead.Count);
                Console.WriteLine("\n");
                Console.WriteLine("\n");


                if (GameDone) {
                    server.WinGame(Victors);
                }
                game = !GameDone;
            }
        }
        static void Main(string[] args)
        {
            //for (int i = 0; i < 8; i++)
            //{
            //    Console.WriteLine("Starting the tournament with {0}players!", i);
            //    Tournament tournament = new Tournament();
            //    tournament.Play(i);
            //    Console.WriteLine("Ending the tournament with {0}players!", i);
            //}
            //Tournament tournament = new Tournament();
            //tournament.Play(3);

        }
    }
}
