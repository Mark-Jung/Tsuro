using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;


namespace TsuroTheSecond
{
    public class Tournament
    {
        public Tournament()
        {
        }


        public void Play(){
            // make server
            Server server = new Server();

            IPEndPoint ip = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9999);

            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);


            // add players
            RandomPlayer mplayer1 = new RandomPlayer("Adam");
            MostSymmetricPlayer mplayer2 = new MostSymmetricPlayer("John");
            LeastSymmetricPlayer mplayer3 = new LeastSymmetricPlayer("Cathy");
            NPlayer nPlayer1 = new NPlayer("Mark", socket);
            NPlayer nPlayer2 = new NPlayer("Ethan", socket);

            server.AddPlayer(mplayer1, "blue");
            server.AddPlayer(mplayer2, "green");
            server.AddPlayer(mplayer3, "hotpink");
            server.AddPlayer(nPlayer1, "red");
            server.AddPlayer(nPlayer2, "purple");

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
            Tournament tournament = new Tournament();
            tournament.Play();

        }


    }
}
