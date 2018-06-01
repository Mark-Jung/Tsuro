using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;


namespace TsuroTheSecond
{
    public class Tournament
    {
        public int port = 9990;
        public int toPlay = 1000;
        public int nplayercnt = 2;
        public int totalplayercnt = 0;
        public IPEndPoint iP;
        public Socket socket;
        //public object threadlock;
        //public int threadcnt;

        Dictionary<string, int> WinnerCount = new Dictionary<string, int>();
        //static TcpListener tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 9999);
        public Tournament()
        {
        }

        public void ProxyPlay(string name, string color, IPAddress address, int portnum)
        {
            // launch the proxy player
            // 1. constructor: machine player, color, port number
            PlayerProxy playerProxy = new PlayerProxy(new MostSymmetricPlayer(name), color, address, portnum);
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
            List<string> colors = new List<string> { "blue", "red", "green", "orange", "sienna", "hotpink", "darkgreen", "purple" };
            List<string> names = new List<string> { "Mark", "Ethan", "Robby", "Chris", "James", "San", "Nell", "Vinay" };

            Server server = new Server();
            Console.WriteLine("Game Server instantiated");
            //NPlayer nPlayer1 = GetConnection("Mark", "red", this.port);
            //NPlayer nPlayer2 = GetConnection("Ethan", "purple", this.port);
            for (int i = 0; i < nplayercnt; i++){
                Console.WriteLine("Name for this network player is: " + names[i]);
                Console.WriteLine("Color for this network player is: "  + colors[i]);
                NPlayer nPlayer1 = GetConnection(names[i], colors[i], this.port);
                server.AddPlayer(nPlayer1,  colors[i]);
            }

            // add players
            int mplayercnt = totalplayercnt - nplayercnt;
            Console.WriteLine("Adding " + mplayercnt +  " mplayers!");
            for (int i = nplayercnt; i < totalplayercnt; i++){
                int which_machine = i % 3;
                if(which_machine == 0){
                    // random
                    RandomPlayer random = new RandomPlayer(names[i]);
                    server.AddPlayer(random, colors[i]);
                } else if(which_machine == 1){
                    //lest symm
                    LeastSymmetricPlayer least = new LeastSymmetricPlayer(names[i]);
                    server.AddPlayer(least, colors[i]);
                } else{
                    //most symm
                    MostSymmetricPlayer most = new MostSymmetricPlayer(names[i]);
                    server.AddPlayer(most, colors[i]);
                }

            }
            Console.WriteLine("Added "  + mplayercnt + " machine players");

            // connect nPlayer1 to ProxyPlayer

            // init positions of players
            server.InitPlayerPositions();

            server.ShuffleDeck(server.deck);

            // game loop
            bool game = true;
            while (game && server.alive.Count > 0)
            {
                Player currentPlayer = server.alive[0];
                //Console.WriteLine("\n");
                //Console.WriteLine("\n");
                //Console.WriteLine("Now " + currentPlayer.iplayer.GetName() + "'s turn.");
                //Console.WriteLine("Position is: ");
                server.board.tokenPositions[currentPlayer.Color].PrintMe();

                // choose what tile to play
                //Console.WriteLine("Choosing Tile to play from");
                foreach (Tile each in currentPlayer.Hand)
                {
                    each.PrintMe();
                }

                Tile playTile = currentPlayer.iplayer.PlayTurn(server.board, currentPlayer.Hand, server.deck.Count);
                //Console.WriteLine("Tile to be played is: ");
                playTile.PrintMe();
                while (!server.LegalPlay(currentPlayer, server.board, playTile))
                {
                    //Console.WriteLine("Seems like that tile wasn't legal! Going into LegalPlay loop");
                    playTile = currentPlayer.iplayer.PlayTurn(server.board, currentPlayer.Hand, server.deck.Count);

                }
                //Console.WriteLine("Tile to be played was legal!");
                // take that tile out of hand
                currentPlayer.RemoveTilefromHand(playTile);
                //Console.WriteLine("Removed that tile!");
                // play that tile
                (List<Tile> _deck, List<Player> _alive, List<Player> _dead, Board _board, Boolean GameDone, List<Player> Victors) = server.PlayATurn(server.deck, server.alive, server.dead, server.board, playTile);


                //Console.WriteLine("Turn Summary:");
                //Console.WriteLine("Deck count: " + server.deck.Count);
                //Console.WriteLine("Alive count: " + server.alive.Count);
                //Console.WriteLine("Position after turn: ");
                //server.board.tokenPositions[currentPlayer.Color].PrintMe();
                //Console.WriteLine("Survivor list: ");
                //foreach (Player survived in server.alive)
                //{
                //    Console.WriteLine(survived.Color);
                //}
                //Console.WriteLine("Dead count: " + server.dead.Count);
                //Console.WriteLine("\n");
                //Console.WriteLine("\n");



                if (GameDone)
                {
                    foreach (Player victor in Victors)
                    {
                        if (!WinnerCount.ContainsKey(victor.iplayer.GetName()))
                        {
                            WinnerCount.Add(victor.iplayer.GetName(), 0);
                        }
                        WinnerCount[victor.iplayer.GetName()]++;
                    }
                    server.WinGame(Victors);
                    // mutex cnt;
                }
                game = !GameDone;
            }
        }
        static void Main(string[] args)
        {
            if(args.Length == 0){
                Tournament tournament_player = new Tournament();
                string name = "Mark";
                string color = "blue";
                tournament_player.toPlay = 1000;
                tournament_player.port = 12345;

                IPAddress ip;
                IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                ip = ipAddress;
                //arg[0] name
                //arg[1] color
                //arg[2] portnumber
                for (int i = 0; i < tournament_player.toPlay; i++)
                {
                    tournament_player.ProxyPlay(name, color, ip, tournament_player.port);
                }
            }
            else if(args[0] == "player")
            {
                
                Console.WriteLine("Running player mode!");
                Console.WriteLine("args after player:");
                Console.WriteLine("1st arg: name");
                Console.WriteLine(args[1]);
                Console.WriteLine("2nd arg: color");
                Console.WriteLine(args[2]);
                Console.WriteLine("3rd arg: games to play");
                Console.WriteLine(args[3]);
                Console.WriteLine("4th arg: portnumber");
                Console.WriteLine(args[4]);
                Console.WriteLine("5th arg: ip_address. Default to local");

                Tournament tournament_player = new Tournament();
                string name = args[1];
                string color = args[2];
                tournament_player.toPlay = Int32.Parse(args[3]);
                tournament_player.port = Int32.Parse(args[4]);

                IPAddress ip;
                if(args.Length == 5){
                    // not provided default ip
                    IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                    IPAddress ipAddress = ipHostInfo.AddressList[0];
                    ip = ipAddress;
                } else if (args.Length == 6){ 
                    Console.WriteLine(args[5]);
                    ip = IPAddress.Parse(args[5]); 
                } else 
                {
                    Console.WriteLine(args.Length);
                    throw new Exception("Unexpected amount of input params.");
                }
                //arg[0] name
                //arg[1] color
                //arg[2] portnumber
                for (int i = 0; i < tournament_player.toPlay; i++)
                {
                    tournament_player.ProxyPlay(name, color, ip, tournament_player.port);
                }
                Console.WriteLine("\n");
                Console.WriteLine("\n");
                Console.WriteLine("\n");
                Console.WriteLine("\n");
                Console.WriteLine("Successfully finished tournament!");

            } else if (args[0] == "server"){
                Console.WriteLine("Going into server mode!");
                Console.WriteLine("args after server:");
                Console.WriteLine("1st arg: port number of the server.");
                Console.WriteLine(args[1]);
                Console.WriteLine("2nd arg: number of games to play.");
                Console.WriteLine(args[2]);
                Console.WriteLine("3rd arg: number of network players");
                Console.WriteLine(args[3]);
                Console.WriteLine("4th arg: number of total players");
                Console.WriteLine(args[4]);
                Tournament tournament = new Tournament();

                tournament.port = Int32.Parse(args[1]);
                tournament.toPlay = Int32.Parse(args[2]);
                tournament.nplayercnt = Int32.Parse(args[3]);
                tournament.totalplayercnt = Int32.Parse(args[4]);

                IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                Console.WriteLine("IP address is: " + ipAddress);


                //Console.WriteLine("Socket bound to ip address: " + ipAddress.ToString());

                //Console.WriteLine("Now listening for connection");
                tournament.iP = new IPEndPoint(ipAddress, tournament.port);
                tournament.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                tournament.socket.Bind(tournament.iP);
                Console.WriteLine("Binding to: " + tournament.iP);
                tournament.socket.Listen(10);

                // game loop
                for (int i = 0; i < tournament.toPlay; i++)
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
            else {
                Console.WriteLine("Invalid mode to run. Program closed.");  
            }
            

        }


    }
}
