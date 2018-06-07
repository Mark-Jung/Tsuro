using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TsuroTheSecond;

namespace TsuroTheSecondTests
{
    [TestClass]
    public class ServerTest
    {
        Server server;

        [TestInitialize]
        public void Initialize() {
            server = new Server();
        }

        void AddThreePlayers()
        {
            RandomPlayer p1 = new RandomPlayer("john");
            RandomPlayer p2 = new RandomPlayer("jim");
            RandomPlayer p3 = new RandomPlayer("george");

            server.AddPlayer(p1, "blue");
            server.AddPlayer(p2, "green");
            server.AddPlayer(p3, "hotpink");
        }

        void AddFourPlayers(){
            RandomPlayer p1 = new RandomPlayer("john");
            RandomPlayer p2 = new RandomPlayer("jim");
            RandomPlayer p3 = new RandomPlayer("george");
            RandomPlayer p4 = new RandomPlayer("jimmy");

            server.AddPlayer(p1, "blue");
            server.AddPlayer(p2, "green");
            server.AddPlayer(p3, "red");
            server.AddPlayer(p4, "hotpink");
        }

        /* TODO ******************************
         * Test Tiles are unique
         * Dragon tile more
         */

        [TestMethod]
        public void TestTilesUnique()
        {
            // test that no tile is a rotated version of any other tile
            // there are only 35 such tiles possible
            Console.WriteLine(server.deck.Count);
            HashSet<string> tileCombinations = new HashSet<string>();
            foreach (Tile t in server.deck) {
                string tilePathMap = t.PathMap();
                tileCombinations.Add(tilePathMap);
            }

            Assert.AreEqual(35, tileCombinations.Count);
        }

        [TestMethod]
        public void TestConstructor()
        {
            Assert.AreEqual(0, server.alive.Count);
            Assert.AreEqual(0, server.dead.Count);
        }

        [TestMethod]
        public void TestConstructor3Player()
        {
            AddThreePlayers();

            Assert.AreEqual(3, server.alive.Count);
            Assert.AreEqual(0, server.dead.Count);
        }

        [TestMethod]
        public void TestDraw()
        {
            AddThreePlayers();

            Assert.AreEqual(0, server.alive[0].Hand.Count);
            Assert.AreEqual(35, server.deck.Count);
            server.DrawTile(server.alive[0], server.deck);
            Assert.AreEqual(1, server.alive[0].Hand.Count);
            Assert.AreEqual(34, server.deck.Count);
        }

        [TestMethod]
        public void TestShuffleDeck()
        {
            var shuffledDeck = server.ShuffleDeck(Constants.tiles);
            Assert.AreEqual(35, shuffledDeck.Count);
            Assert.AreNotEqual(Constants.tiles, shuffledDeck);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Invalid color")]
        public void TestDuplicateColor()
        {
            AddThreePlayers();

            MPlayer1 mPlayer = new MPlayer1("harry");
            server.AddPlayer(mPlayer, "blue");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Invalid color")]
        public void TestInvalidColor()
        {
            AddThreePlayers();

            MPlayer1 mPlayer = new MPlayer1("john");
            server.AddPlayer(mPlayer, "not blue");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "Player can't have more than 3 cards in hand")]
        public void TestDrawTooManyTiles()
        {
            AddThreePlayers();

            server.DrawTile(server.alive[0], server.deck);
            server.DrawTile(server.alive[0], server.deck);
            server.DrawTile(server.alive[0], server.deck);
            server.DrawTile(server.alive[0], server.deck);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "Player can't have more than 3 cards in hand")]
        public void TestDrawTooManyTilesTilesSet()
        {
            // tiles were externally set
            AddThreePlayers();

            server.alive[0].Hand = new List<Tile>{
                new Tile(1, new List<int>{1, 2, 3, 4, 5, 6, 7, 0}),
                new Tile(1, new List<int>{1, 2, 3, 4, 5, 6, 7, 0}),
                new Tile(1, new List<int>{1, 2, 3, 4, 5, 6, 7, 0}),
            };

            server.DrawTile(server.alive[0], server.deck);
        }

        [TestMethod]
        public void TestKillPlayer()
        {
            AddFourPlayers();

            Assert.AreEqual(0, server.alive[0].Hand.Count);
            Assert.AreEqual(35, server.deck.Count);
            server.DrawTile(server.alive[0], server.deck);
            Assert.AreEqual(0, server.dead.Count);
            Assert.AreEqual(4, server.alive.Count);
            Assert.AreEqual(34, server.deck.Count);

            server.KillPlayer(server.alive[0]);
            Assert.AreEqual(1, server.dead.Count);
            Assert.AreEqual(3, server.alive.Count);
            Assert.AreEqual(35, server.deck.Count);
        }

        //[TestMethod]
        //public void TestDragonTile()
        //{
        //    AddFourPlayers();

        //    // manually shorten deck to 5 cards
        //    server.deck = server.deck.GetRange(0, 5);
        //    server.DrawTile(server.alive[0], server.deck);
        //    server.DrawTile(server.alive[1], server.deck);
        //    server.DrawTile(server.alive[2], server.deck);
        //    server.DrawTile(server.alive[3], server.deck);
        //    server.DrawTile(server.alive[0], server.deck);
        //    Assert.AreEqual(0, server.deck.Count);
        //    Assert.AreEqual(0, server.dragonQueue.Count);

        //    server.DrawTile(server.alive[1], server.deck);
        //    Assert.AreEqual(1, server.dragonQueue.Count);
        //    Assert.AreEqual(server.alive[1], server.dragonQueue[0]);

        //    // kill off player 2, it's cards should go to player 1 because
        //    // p1 has the dragon tile
        //    Assert.AreEqual(1, server.alive[2].Hand.Count);
        //    server.KillPlayer(server.alive[2]);
        //    Assert.AreEqual(1, server.dead.Count);
        //    Assert.AreEqual(0, server.dragonQueue.Count);
        //    Assert.AreEqual(2, server.alive[1].Hand.Count);
        //}

        //[TestMethod]
        //public void TestKillYourselfWithDragonTile()
        //{
        //    AddFourPlayers(); 

        //    server.deck = server.deck.GetRange(0, 5);
        //    server.DrawTile(server.alive[0], server.deck);
        //    server.DrawTile(server.alive[1], server.deck);
        //    server.DrawTile(server.alive[2], server.deck);
        //    server.DrawTile(server.alive[3], server.deck);
        //    server.DrawTile(server.alive[0], server.deck);
        //    // player 1 has the dragon tile
        //    server.DrawTile(server.alive[1], server.deck);
        //    server.KillPlayer(server.alive[1]);
        //    Assert.AreEqual(1, server.deck.Count);
        //    Assert.AreEqual(0, server.dragonQueue.Count);
        //}

        [TestMethod]
        public void TestSetInitialMarkers()
        {
        }

        [TestMethod]
        public void TestInitGame()
        {
        }

        [TestMethod]
        public void TestLegalPlayCheckLastPossibleMoveNotUndone()
        {
            Server server = new Server();
            MPlayer1 p1 = new MPlayer1("john");
            server.AddPlayer(p1, "blue");

            Tile testTile1 = new Tile(1, new List<int>(8) {
                0, 1, 2, 3, 4, 5, 6, 7,
            });
            Tile testTile2 = new Tile(2, new List<int>(8) {
                0, 1, 2, 3, 4, 5, 6, 7,
            });
            Tile testTile3 = new Tile(3, new List<int>(8) {
                0, 5, 1, 4, 2, 7, 3, 6,
            });

            Player p_1 = server.alive[0];
            server.board.AddPlayerToken("blue", new Position(4, 6, 0));
            p_1.Hand = new List<Tile> { testTile1, testTile2, testTile3 };
            server.gameState = Server.State.loop;
            Boolean legalPlay = server.LegalPlay(p_1, server.board, testTile1);
            Assert.IsFalse(legalPlay);
            Assert.AreEqual(3, p_1.Hand.Count);
            for (int i = 1; i < 4; i++){
                Assert.AreEqual(i, p_1.Hand[i-1].id);
            }
        }

        [TestMethod]
        public void TestIllegalPlayNullTile()
        {
            Server server = new Server();
            MPlayer1 mp1 = new MPlayer1("jim");
            server.AddPlayer(mp1, "blue");

            Tile testTile1 = new Tile(1, new List<int>(8) {
                0, 1, 2, 3, 4, 5, 6, 7,
            });
            Tile testTile2 = new Tile(2, new List<int>(8) {
                0, 1, 2, 3, 4, 7, 6, 5,
            });
            Tile testTile3 = new Tile(3, new List<int>(8) {
                0, 5, 1, 4, 2, 7, 3, 6,
            });

            Player p1 = server.alive[0];
            var origClass = p1.iplayer.GetType();
            server.gameState = Server.State.loop;
            bool legalPlay = server.LegalPlay(p1, server.board, null);
            Assert.IsFalse(legalPlay);
            Assert.AreNotEqual(origClass, server.alive[0].iplayer.GetType());
        }

        [TestMethod]
        public void TestIllegalPlayTileNotInHand()
        {
            Server server = new Server();
            MPlayer1 mp1 = new MPlayer1("jim");
            server.AddPlayer(mp1, "blue");

            Tile testTile1 = new Tile(1, new List<int>(8) {
                0, 1, 2, 3, 4, 5, 6, 7,
            });
            Tile testTile2 = new Tile(2, new List<int>(8) {
                0, 1, 2, 3, 4, 7, 6, 5,
            });
            Tile testTile3 = new Tile(3, new List<int>(8) {
                0, 5, 1, 4, 2, 7, 3, 6,
            });

            Tile otherTile = new Tile(4, new List<int>(8) {
                1, 2, 3, 4, 5, 6, 7, 0
            });

            Player p1 = server.alive[0];
            var origClass = p1.iplayer.GetType();
            server.gameState = Server.State.loop;
            bool legalPlay = server.LegalPlay(p1, server.board, otherTile);
            Assert.IsFalse(legalPlay);
            Assert.AreNotEqual(origClass, server.alive[0].iplayer.GetType());
        }

        [TestMethod]
        public void TestLegalPlayBoardUndo()
        {
            Server server = new Server();
            MPlayer1 p1 = new MPlayer1("john");
            server.AddPlayer(p1, "blue");

            Tile testTile1 = new Tile(1, new List<int>(8) {
                0, 1, 2, 3, 4, 5, 6, 7,
            });
            Tile testTile2 = new Tile(2, new List<int>(8) {
                0, 1, 2, 3, 4, 5, 6, 7,
            });
            Tile testTile3 = new Tile(3, new List<int>(8) {
                0, 5, 1, 4, 2, 7, 3, 6,
            });

            Player p_1 = server.alive[0];

            var origClass = p_1.iplayer.GetType();

            server.board.AddPlayerToken("blue", new Position(4, 6, 0));
            p_1.Hand = new List<Tile> { testTile1, testTile2, testTile3 };
            server.gameState = Server.State.loop;
            Boolean legalPlay = server.LegalPlay(p_1, server.board, testTile1);
            Assert.AreNotEqual(origClass, server.alive[0].iplayer.GetType());
            Assert.IsFalse(legalPlay);
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 6; j++) 
                {
                    Assert.IsNull(server.board.tiles[i][j]);
                }
            }
        }

        [TestMethod]
        public void TestLegalPlayFalse2()
        {
            AddThreePlayers();

            Tile testTile1 = new Tile(1, new List<int>(8) {
                0, 1, 2, 3, 4, 5, 6, 7,
            });
            Tile testTile3 = new Tile(3, new List<int>(8) {
                0, 5, 1, 4, 2, 7, 3, 6,
            });

            Player p_1 = server.alive[0];
            server.board.AddPlayerToken("blue", new Position(4, 6, 0));
            p_1.Hand = new List<Tile> { testTile1, testTile3 };
            server.gameState = Server.State.loop;
            Assert.IsFalse(server.LegalPlay(p_1, server.board, testTile1));
        }

        [TestMethod]
        public void TestLegalPlayTrue3()
        {
            AddThreePlayers();

            Tile testTile1 = new Tile(1, new List<int>(8) {
                0, 1, 2, 3, 4, 5, 6, 7,
            });
            Tile testTile2 = new Tile(2, new List<int>(8) {
                0, 1, 2, 3, 4, 5, 6, 7,
            });
            Tile testTile3 = new Tile(3, new List<int>(8) {
                0, 5, 1, 4, 2, 7, 3, 6,
            });

            Player p_1 = server.alive[0];
            server.board.AddPlayerToken("blue", new Position(4, 6, 0));
            p_1.Hand = new List<Tile> { testTile1, testTile2, testTile3 };
            server.gameState = Server.State.loop;
            Assert.IsTrue(server.LegalPlay(p_1, server.board, testTile3));
        }

        [TestMethod]
        public void TestLegalPlayTrue2()
        {
            AddThreePlayers();

            Tile testTile1 = new Tile(1, new List<int>(8) {
                0, 1, 2, 3, 4, 5, 6, 7,
            });
            Tile testTile3 = new Tile(3, new List<int>(8) {
                0, 5, 1, 4, 2, 7, 3, 6,
            });

            Player p_1 = server.alive[0];
            server.board.AddPlayerToken("blue", new Position(4, 6, 0));
            p_1.Hand = new List<Tile> { testTile1, testTile3 };
            server.gameState = Server.State.loop;
            Assert.IsTrue(server.LegalPlay(p_1, server.board, testTile3));
        }

        [TestMethod]
        public void TestLegalPlayTrue1_t()
        {
            AddThreePlayers();

            Tile testTile3 = new Tile(3, new List<int>(8) {
                0, 5, 1, 4, 2, 7, 3, 6,
            });
            Player p_1 = server.alive[0];
            server.board.AddPlayerToken("blue", new Position(4, 6, 0));
            p_1.Hand = new List<Tile> { testTile3 };
            server.gameState = Server.State.loop;
            Assert.IsTrue(server.LegalPlay(p_1, server.board, testTile3));
        }
        [TestMethod]
        public void TestLegalPlayFalse1()
        {
            AddThreePlayers();

            Tile testTile1 = new Tile(1, new List<int>(8) {
                0, 1, 2, 3, 4, 5, 6, 7,
            });

            Player p_1 = server.alive[0];
            server.board.AddPlayerToken("blue", new Position(4, 6, 0));
            p_1.Hand = new List<Tile> { testTile1 };
            server.gameState = Server.State.loop;
            Assert.IsTrue(server.LegalPlay(p_1, server.board, testTile1));
        }
        [TestMethod]
        public void TestLegalPlayLastResort()
        {
            AddThreePlayers();

            Tile testTile1 = new Tile(1, new List<int>(8) {
                0, 1, 2, 3, 4, 5, 6, 7,
            });
            Tile testTile2 = new Tile(2, new List<int>(8) {
                0, 1, 2, 3, 4, 5, 6, 7,
            });
            Tile testTile3 = new Tile(3, new List<int>(8) {
                0, 1, 2, 3, 4, 5, 6, 7,
            });

            Player p_1 = server.alive[0];
            server.board.AddPlayerToken("blue", new Position(4, 6, 0));
            p_1.Hand = new List<Tile> { testTile1, testTile2, testTile3 };
            server.gameState = Server.State.loop;
            Assert.IsTrue(server.LegalPlay(p_1, server.board, testTile1));
        }

        [TestMethod]
        public void TestLegalPlayFalseWithRotation()
        {
            AddThreePlayers();

            Tile testTile1 = new Tile(1, new List<int>(8) {
                0, 1, 2, 3, 4, 5, 6, 7,
            });
            Tile testTile2 = new Tile(2, new List<int>(8) {
                0, 1, 2, 3, 4, 5, 6, 7,
            });
            Tile testTile3 = new Tile(3, new List<int>(8) {
                0, 1, 2, 7, 3, 6, 4, 5,
            });

            Player p_1 = server.alive[0];
            server.board.AddPlayerToken("blue", new Position(4, 6, 0));
            p_1.Hand = new List<Tile> { testTile1, testTile2, testTile3 };
            // testTile3 will kill without rotation, but with rotation won't kill.
            // Thus, testTile3 is a valid option, making legalPlay of testTile1 false.
            server.gameState = Server.State.loop;
            Assert.IsFalse(server.LegalPlay(p_1, server.board, testTile1));
        }
        [TestMethod]
        public void TestLegalPlayTrueWithRotation()
        {
            AddThreePlayers();

            Tile testTile1 = new Tile(1, new List<int>(8) {
                0, 1, 2, 3, 4, 5, 6, 7,
            });
            Tile testTile2 = new Tile(2, new List<int>(8) {
                0, 1, 2, 3, 4, 5, 6, 7,
            });
            Tile testTile3 = new Tile(3, new List<int>(8) {
                0, 1, 2, 7, 3, 6, 4, 5,
            });
            Player p_1 = server.alive[0];
            server.board.AddPlayerToken("blue", new Position(4, 6, 0));
            p_1.Hand = new List<Tile> { testTile1, testTile2, testTile3 };
            // testTile3 will kill without rotation, but with rotation won't kill.
            server.gameState = Server.State.loop;
            Assert.IsTrue(server.LegalPlay(p_1, server.board, testTile3));
        }

        //[TestMethod]
        //public void TestPlayATurn()
        //{
        //    // alive[0] is blue, alive[1] is green
        //    AddThreePlayers();

        //    server.InitPlayerPositions();

        //    server.board.initialPositions["blue"] = new Position(0, -1, 5);
        //    server.board.initialPositions["green"] = new Position(0, -1, 4);
        //    server.board.initialPositions["hotpink"] = new Position(1, -1, 4);

        //    server.board.tokenPositions["blue"] = new Position(0, -1, 5);
        //    server.board.tokenPositions["green"] = new Position(0, -1, 4);
        //    server.board.tokenPositions["hotpink"] = new Position(1, -1, 4);

        //    server.alive[0].Hand = new List<Tile> { new Tile(3, new List<int> { 1, 2, 3, 4, 5, 6, 7, 0 }), new Tile(2, new List<int> { 4, 7, 3, 2, 0, 1, 5, 6 }) };

        //    Tile playTile = new Tile(1, new List<int>{0, 7, 1, 2, 3, 4, 5, 6});

        //    server.gameState = Server.State.safe;
        //    (List<Tile> deck, List<Player> alive, List<Player> dead, Board board, Boolean GameDone, List<Player> Victors) = server.PlayATurn(server.deck, 
        //                                                                                           server.alive, 
        //                                                                                           server.dead, 
        //                                                                                           server.board, 
        //                                                                                           playTile);
        //    Assert.AreEqual(2, server.alive.Count);
        //    Assert.AreEqual(1, server.dead.Count);
        //    // dead
        //    Assert.AreEqual(-1, server.board.tokenPositions["blue"].x);
        //    Assert.AreEqual(0, server.board.tokenPositions["blue"].y);
        //    Assert.AreEqual(2, server.board.tokenPositions["blue"].port);
        //    Assert.AreEqual(-1, server.board.tokenPositions[server.dead[0].Color].x);
        //    Assert.AreEqual(0, server.board.tokenPositions[server.dead[0].Color].y);
        //    Assert.AreEqual(2, server.board.tokenPositions[server.dead[0].Color].port);
        //    // alive
        //    Assert.AreEqual(0, server.board.tokenPositions["green"].x);
        //    Assert.AreEqual(0, server.board.tokenPositions["green"].y);
        //    Assert.AreEqual(2, server.board.tokenPositions["green"].port);
        //    Assert.AreEqual(0, server.board.tokenPositions[server.alive[0].Color].x);
        //    Assert.AreEqual(0, server.board.tokenPositions[server.alive[0].Color].y);
        //    Assert.AreEqual(2, server.board.tokenPositions[server.alive[0].Color].port);

        //    Assert.IsFalse(GameDone);

        //    Assert.AreEqual(2, alive.Count);
        //    Assert.AreEqual(1, dead.Count);
        //    // dead
        //    Assert.AreEqual(-1, board.tokenPositions["blue"].x);
        //    Assert.AreEqual(0, board.tokenPositions["blue"].y);
        //    Assert.AreEqual(2, board.tokenPositions["blue"].port);
        //    Assert.AreEqual(-1, board.tokenPositions[server.dead[0].Color].x);
        //    Assert.AreEqual(0, board.tokenPositions[server.dead[0].Color].y);
        //    Assert.AreEqual(2, board.tokenPositions[server.dead[0].Color].port);
        //    // alive
        //    Assert.AreEqual(0, board.tokenPositions["green"].x);
        //    Assert.AreEqual(0, board.tokenPositions["green"].y);
        //    Assert.AreEqual(2, board.tokenPositions["green"].port);
        //    Assert.AreEqual(0, board.tokenPositions[server.alive[0].Color].x);
        //    Assert.AreEqual(0, board.tokenPositions[server.alive[0].Color].y);
        //    Assert.AreEqual(2, board.tokenPositions[server.alive[0].Color].port);

        //}

        //[TestMethod]
        //public void TestPlayATurn2TilePath()
        //{
        //    AddThreePlayers();
        //    server.InitPlayerPositions();

        //    server.board.initialPositions["blue"] = new Position(0, -1, 5);
        //    server.board.initialPositions["green"] = new Position(0, -1, 4);
        //    server.board.initialPositions["hotpink"] = new Position(1, -1, 4);

        //    server.board.tokenPositions["blue"] = new Position(0, -1, 5);
        //    server.board.tokenPositions["green"] = new Position(0, -1, 4);
        //    server.board.tokenPositions["hotpink"] = new Position(1, -1, 4);

        //    server.alive[0].Hand = new List<Tile> { new Tile(5, new List<int> { 1, 2, 3, 4, 5, 6, 7, 0 }), new Tile(4, new List<int> { 4, 7, 3, 2, 0, 1, 5, 6 }) };


        //    Tile playTile = new Tile(1, new List<int> { 0, 4, 1, 5, 3, 2, 6, 7 });

        //    Tile secondTile = new Tile(2, new List<int> { 0, 7, 2, 6, 1, 3, 5, 4 });
        //    server.board.PlaceTile(secondTile, 0, 1);

        //    server.gameState = Server.State.safe;
        //    (List<Tile> deck, List<Player> alive, List<Player> dead, Board board, Boolean GameDone, List<Player> Victors) = server.PlayATurn(server.deck,
        //                                                                                           server.alive,
        //                                                                                           server.dead,
        //                                                                                           server.board,
        //                                                                                           playTile);
        //    Assert.AreEqual(2, server.alive.Count);
        //    Assert.AreEqual(1, server.dead.Count);
        //    Assert.AreEqual("hotpink", server.alive[0].Color);
        //    Assert.AreEqual("blue", server.alive[1].Color);
        //    Assert.AreEqual("green", server.dead[0].Color);
        //    // 0th player should move to the end
        //    Assert.AreEqual(-1, server.board.tokenPositions["green"].x);
        //    Assert.AreEqual(1, server.board.tokenPositions["green"].y);
        //    Assert.AreEqual(2, server.board.tokenPositions["green"].port);
        //    Assert.AreEqual(-1, server.board.tokenPositions[server.dead[0].Color].x);
        //    Assert.AreEqual(1, server.board.tokenPositions[server.dead[0].Color].y);
        //    Assert.AreEqual(2, server.board.tokenPositions[server.dead[0].Color].port);

        //    Assert.AreEqual(0, server.board.tokenPositions["blue"].x);
        //    Assert.AreEqual(1, server.board.tokenPositions["blue"].y);
        //    Assert.AreEqual(3, server.board.tokenPositions["blue"].port);
        //    Assert.AreEqual(0, server.board.tokenPositions[server.alive[1].Color].x);
        //    Assert.AreEqual(1, server.board.tokenPositions[server.alive[1].Color].y);
        //    Assert.AreEqual(3, server.board.tokenPositions[server.alive[1].Color].port);

        //}

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Player should have 2 or less tiles in hand")]
        public void TestPlayATurnPlayerTooManyTilesInHand()
        {
            AddThreePlayers();

            server.InitPlayerPositions();

            //for (int i = 0; i < 3; i++) {
            //    server.alive[0].AddTiletoHand(Constants.tiles[i]);
            //}

            server.gameState = Server.State.safe;
            (List<Tile> deck, List<Player> alive, List<Player> dead, Board board, Boolean GameDone, List<Player> Victors) = server.PlayATurn(server.deck,
                                                                                                                        server.alive,
                                                                                                   server.dead,
                                                                                                   server.board,
                                                                                                                                             Constants.tiles[3]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Tile to be played and tiles in hand are not unique")]
        public void TestPlayATurnDuplicateTiles()
        {
            AddThreePlayers();

            server.InitPlayerPositions();

            server.alive[0].Hand[0] = Constants.tiles[0];

            server.gameState = Server.State.safe;
            (List<Tile> deck, List<Player> alive, List<Player> dead, Board board, Boolean GameDone, List<Player> Victors) = server.PlayATurn(server.deck,
                                                                                                                        server.alive,
                                                                                                   server.dead,
                                                                                                   server.board,
                                                                                                   Constants.tiles[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Tile to be played and tiles in hand are not unique")]
        public void TestPlayATurnDuplicateTilesRotated()
        {
            AddThreePlayers();

            server.InitPlayerPositions();

            Tile tile = new Tile(Constants.tiles[0]);
            tile.Rotate();
            server.alive[0].Hand[0] = tile;

            server.gameState = Server.State.safe;
            (List<Tile> deck, List<Player> alive, List<Player> dead, Board board, Boolean GameDone, List<Player> Victors) = server.PlayATurn(server.deck,
                                                                                                                        server.alive,
                                                                                                   server.dead,
                                                                                                   server.board,
                                                                                                   Constants.tiles[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Tile to be placed, tiles in hand, and tiles on board are not unique")]
        public void TestPlayTurnADuplicateTilesOnBoardAndHand()
        {
            AddThreePlayers();

            server.InitPlayerPositions();

            Tile tile = new Tile(Constants.tiles[0]);
            tile.Rotate();
            server.alive[0].Hand[0] = tile;
            server.board.PlaceTile(tile, 0, 0);

            server.gameState = Server.State.safe;
            (List<Tile> deck, List<Player> alive, List<Player> dead, Board board, Boolean GameDone, List<Player> Victors) = server.PlayATurn(server.deck,
                                                                                                                        server.alive,
                                                                                                   server.dead,
                                                                                                   server.board,
                                                                                                   Constants.tiles[1]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Tile to be placed, tiles in hand, and tiles on board are not unique")]
        public void TestPlayATurnDuplicateTilesOnBoardAndPlay()
        {
            AddThreePlayers();

            server.InitPlayerPositions();

            Tile tile = Constants.tiles[0];
            tile.Rotate();
            server.board.PlaceTile(Constants.tiles[0], 0, 0);

            server.gameState = Server.State.safe;
            (List<Tile> deck, List<Player> alive, List<Player> dead, Board board, Boolean GameDone, List<Player> Victors) = server.PlayATurn(server.deck,
                                                                                                                        server.alive,
                                                                                                   server.dead,
                                                                                                   server.board,
                                                                                                   Constants.tiles[0]);
        }

        [TestMethod]
        public void ReplacePlayer() {
            // this is a random function we're testing
            AddFourPlayers();
            var origType = server.alive[0].iplayer.GetType();
            server.ReplacePlayer(server.alive[0]);

            Assert.AreNotEqual(origType, server.alive[0].iplayer.GetType());
        }

        class InitPositionCheatPlayer : IPlayer
        {
            // player that throws a init position error
            public String GetName()
            {
                return "InitPositionCheatPlayer";
            }

            public void Initialize(string color, List<string> other_colors)
            {
            }

            public Position PlacePawn(Board board)
            {
                Position p = new Position(1, 2, 3);
                return p;
            }

            public Tile PlayTurn(Board board, List<Tile> hand, int unused)
            {
                return null;
            }

            public void EndGame(Board board, List<string> colors)
            {
            }
        }

        [TestMethod]
        public void TestPlayerCheatInitPlayerPosition()
        {
            InitPositionCheatPlayer cheat = new InitPositionCheatPlayer();
            server.AddPlayer(cheat, "orange");
            AddThreePlayers();
            server.InitPlayerPositions();
            Assert.AreNotEqual(cheat.GetType(), server.alive[0].iplayer.GetType());
        }
    }
}
