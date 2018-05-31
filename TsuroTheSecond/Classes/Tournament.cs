using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;


namespace TsuroTheSecond
{
    public class Tournament
    {
        public int port = 8965;
        public IPEndPoint iP;
        public Socket socket;
        //public object threadlock;
        //public int threadcnt;

        Dictionary<string, int> WinnerCount = new Dictionary<string, int>();
        //static TcpListener tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 9999);
        public Tournament()
        {
        }

        public void ProxyPlay(string name, string color, int portnum)
        {
            // launch the proxy player
            // 1. constructor: machine player, color, port number
            PlayerProxy playerProxy = new PlayerProxy(new MostSymmetricPlayer(name), color, portnum);
            Console.WriteLine("Made the proxyplayer");
            // run
            while (!playerProxy.GameFinished)
            {
                playerProxy.Play();
            }
        }

        public NPlayer GetConnection(string name, string color, int portnum)
        {
            // launch another process with three inputs: name, color, portnum
            return new NPlayer(name, socket);
        }


        public void Play(){
            

            // add players

            //Console.WriteLine("Going to add Mark, the network player. Starting proxyplayer thread");
            //Thread nPlayer1Thread = CreatePlayerThread();
            //Thread nPlayer2Thread = CreatePlayerThread();

            //nPlayer1Thread.Start("Mark:red");
            //nPlayer2Thread.Start("Ethan:purple");
            Server server = new Server();
            Console.WriteLine("Server instantiated");
            RandomPlayer mplayer1 = new RandomPlayer("Adam");
            LeastSymmetricPlayer mplayer2 = new LeastSymmetricPlayer("John");
            LeastSymmetricPlayer mplayer3 = new LeastSymmetricPlayer("Cathy");
            Console.WriteLine("Added 3 machine players");
            NPlayer nPlayer1 = GetConnection("Mark", "red", this.port);
            NPlayer nPlayer2 = GetConnection("Ethan", "purple", this.port);
            server.AddPlayer(mplayer1, "blue");
            server.AddPlayer(mplayer2, "green");
            server.AddPlayer(mplayer3, "hotpink");

            nPlayer1.playerState = NPlayer.State.start;
            nPlayer2.playerState = NPlayer.State.start;
            // connect nPlayer1 to ProxyPlayer
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
                foreach (Tile each in currentPlayer.Hand)
                {
                    each.PrintMe();
                }

                Tile playTile = currentPlayer.iplayer.PlayTurn(server.board, currentPlayer.Hand, server.deck.Count);
                Console.WriteLine("Tile to be played is: ");
                playTile.PrintMe();
                while (!server.LegalPlay(currentPlayer, server.board, playTile))
                {
                    Console.WriteLine("Seems like that tile wasn't legal! Going into LegalPlay loop");
                    playTile = currentPlayer.iplayer.PlayTurn(server.board, currentPlayer.Hand, server.deck.Count);

                }
                Console.WriteLine("Tile to be played was legal!");
                // take that tile out of hand
                currentPlayer.RemoveTilefromHand(playTile);
                Console.WriteLine("Removed that tile!");
                // play that tile
                (List<Tile> _deck, List<Player> _alive, List<Player> _dead, Board _board, Boolean GameDone, List<Player> Victors) = server.PlayATurn(server.deck, server.alive, server.dead, server.board, playTile);


                Console.WriteLine("Turn Summary:");
                Console.WriteLine("Deck count: " + server.deck.Count);
                Console.WriteLine("Alive count: " + server.alive.Count);
                Console.WriteLine("Position after turn: ");
                server.board.tokenPositions[currentPlayer.Color].PrintMe();
                Console.WriteLine("Survivor list: ");
                foreach (Player survived in server.alive)
                {
                    Console.WriteLine(survived.Color);
                }
                Console.WriteLine("Dead count: " + server.dead.Count);
                Console.WriteLine("\n");
                Console.WriteLine("\n");



                if (GameDone)
                {
                    foreach (Player victor in Victors)
                    {
                        if (!WinnerCount.ContainsKey(victor.Color))
                        {
                            WinnerCount.Add(victor.Color, 0);
                        }
                        WinnerCount[victor.Color]++;
                    }
                    server.WinGame(Victors);
                    // mutex cnt;
                }
                game = !GameDone;
            }
        }
        static void Main(string[] args)
        {

            if (args.Length == 2)
            {
                Console.WriteLine("Got 2 args so running player mode!");
                Tournament tournament_player = new Tournament();
                for (int i = 0; i < 100; i++)
                {
                    tournament_player.ProxyPlay(args[0], args[1], tournament_player.port);
                    Thread.Sleep(1000);
                }
                Console.WriteLine("\n");
                Console.WriteLine("\n");
                Console.WriteLine("\n");
                Console.WriteLine("\n");
                Console.WriteLine("Stats are:");
                foreach (KeyValuePair<string, int> entry in tournament_player.WinnerCount)
                {
                    Console.WriteLine(entry.Key + ": " + entry.Value);
                }

            } else {
                Console.WriteLine("No args! Going into server mode!");
                Tournament tournament = new Tournament();

                IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddress = ipHostInfo.AddressList[0];


                //Console.WriteLine("Socket bound to ip address: " + ipAddress.ToString());

                //Console.WriteLine("Now listening for connection");
                tournament.iP = new IPEndPoint(ipAddress, tournament.port);
                tournament.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                tournament.socket.Bind(tournament.iP);
                Console.WriteLine("Binding to: " + tournament.iP);
                tournament.socket.Listen(10);

                // game loop
                for (int i = 0; i < 100; i++)
                {
                    tournament.Play();
                }
                tournament.socket.Close();
                Console.WriteLine("\n");
                Console.WriteLine("\n");
                Console.WriteLine("\n");
                Console.WriteLine("\n");
                Console.WriteLine("Stats are:");
                foreach (KeyValuePair<string, int> entry in tournament.WinnerCount)
                {
                    Console.WriteLine(entry.Key + ": " + entry.Value);
                }


            }
            

        }


    }
}
