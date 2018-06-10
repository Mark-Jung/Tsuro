using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TsuroTheSecond;

namespace TsuroTheSecondTests
{
    [TestClass]
    public class PlayerTest
    {
        Server server;
        RandomPlayer rplayer;
        LeastSymmetricPlayer lplayer;
        MostSymmetricPlayer mplayer;
        Player randBlue;
        List<Tile> tiles;

        [TestInitialize]
        public void Initialize()
        {
            server = new Server();
            rplayer = new RandomPlayer("jim");
            lplayer = new LeastSymmetricPlayer("reggie");
            mplayer = new MostSymmetricPlayer("michael");

            randBlue = new Player(rplayer, "blue");
            tiles = new List<Tile>{
                new Tile(1, new List<int>{0, 1, 2, 3, 4, 5, 6, 7}),
                new Tile(2, new List<int>{0, 1, 2, 4, 3, 6, 5, 7}),
                new Tile(3, new List<int>{0, 6, 1, 5, 2, 4, 3, 7}),
                new Tile(4, new List<int>{0, 5, 1, 4, 2, 7, 3, 6}),
                new Tile(5, new List<int>{0, 2, 1, 4, 3, 7, 5, 6}),
            };
        }

        Tile testTile1 = new Tile(1, new List<int>(8) {
            0, 1, 2, 3, 4, 5, 6, 7,
        });


        Tile testTile2 = new Tile(2, new List<int>(8) {
            2, 3, 4, 5, 6, 7, 0, 1,

        });

        Tile testTile3 = new Tile(3, new List<int>(8) {
            6, 7, 0, 1, 2, 3, 4, 5,
        });

        [TestMethod]
        public void TestConstructorRandomPlayer()
        {
            for (int i = 0; i < Constants.colors.Count; i++) {
                RandomPlayer player = new RandomPlayer("john");
                Player p = new Player(player, Constants.colors[i]);
            }
        }

        [TestMethod]
        public void TestConstructorMostSymmetricPlayer()
        {
            for (int i = 0; i < Constants.colors.Count; i++)
            {
                MostSymmetricPlayer player = new MostSymmetricPlayer("john");
                Player p = new Player(player, Constants.colors[i]);
            }
        }

        [TestMethod]
        public void TestConstructorLeastSymmetricPlayer()
        {
            for (int i = 0; i < Constants.colors.Count; i++)
            {
                LeastSymmetricPlayer player = new LeastSymmetricPlayer("john");
                Player p = new Player(player, Constants.colors[i]);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Color not allowed")]
        public void TestBadColorConstructor()
        {
            RandomPlayer mPlayer = new RandomPlayer("time");
            Player player = new Player(mPlayer, "turquoise");
        }

        [TestMethod]
        public void TestAddTiletoHand()
        {
            // takes tile and adds the player to the Hand

            Assert.AreEqual(0, randBlue.Hand.Count);
            randBlue.AddTiletoHand(testTile1);

            Assert.AreEqual(1, randBlue.Hand.Count);
            Assert.AreEqual(testTile1.id, (randBlue.Hand.Find(each => each.id == 1)).id);
        }

        [TestMethod]
        public void TestAddTileFromHand()
        {
            // should fail
            randBlue.AddTiletoHand(new Tile(1, new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 }));

            Assert.AreEqual(randBlue.Hand.Count, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Player hand is already full!")]
        public void TestAddTileToFullHand()
        {
            randBlue.AddTiletoHand(new Tile(1, new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 }));
            randBlue.AddTiletoHand(new Tile(2, new List<int> { 2, 1, 0, 3, 4, 5, 6, 7 }));
            randBlue.AddTiletoHand(new Tile(3, new List<int> { 4, 1, 2, 3, 0, 5, 6, 7 }));
            randBlue.AddTiletoHand(new Tile(4, new List<int> { 3, 1, 2, 4, 0, 5, 6, 7 }));
        }

        [TestMethod]
        public void TestRemoveTilefromHand()
        {
            randBlue.AddTiletoHand(new Tile(1, new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 }));
            randBlue.AddTiletoHand(new Tile(2, new List<int> { 2, 1, 0, 3, 4, 5, 6, 7 }));
            randBlue.AddTiletoHand(new Tile(3, new List<int> { 4, 1, 2, 3, 0, 5, 6, 7 }));

            Assert.AreEqual(3, randBlue.Hand.Count);

            randBlue.RemoveTilefromHand(testTile1);
            Assert.AreEqual(2, randBlue.Hand.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Player hand is already empty!")]
        public void TestRemoveTilefromEmptyHand()
        {
            randBlue.RemoveTilefromHand(testTile1);
        }

        [TestMethod]
        public void TestTileinHand()
        {
            randBlue.AddTiletoHand(new Tile(1, new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 }));

            Tile testTile4 = new Tile(4, new List<int>(8) {
                0, 1, 2, 3, 4, 5, 6, 7,
            });

            Assert.IsTrue(randBlue.TileinHand(testTile1));
            Assert.IsFalse(randBlue.TileinHand(testTile4));
        }

        [TestMethod]
        public void TestRandomPlayerPlayTurnNoLegalHand()
        {
            Board board = new Board(6);
            randBlue.iplayer.Initialize(randBlue.Color, new List<string> {"red", "hotpink"});

            Position p = randBlue.iplayer.PlacePawn(board); // this position should be (0, -1, 5)

            board.AddPlayerToken("blue", p);
            board.tokenPositions["blue"] = new Position(0, -1, 5); // we want to set new position so player dies
            Tile t = randBlue.iplayer.PlayTurn(board, new List<Tile> { tiles[0] }, 35); // both these tiles should kill player
            board.PlaceTile(t, 0, 0);
            board.MovePlayer("blue");
            Assert.IsTrue(board.IsDead("blue"));
        }

        [TestMethod]
        public void TestRandomPlayerPlayTurnOneLegalHand()
        {
            Board board = new Board(6);
            randBlue.iplayer.Initialize(randBlue.Color, new List<string> { "blue", "red", "hotpink" });
            Position p = randBlue.iplayer.PlacePawn(board);
            board.AddPlayerToken("blue", p);
            board.tokenPositions["blue"] = new Position(0, -1, 5); // we want to set new position so player dies

            Tile tile1 = new Tile(1, new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 });
            Tile tile2 = new Tile(2, new List<int> { 0, 1, 2, 4, 3, 6, 5, 7 });
            Tile t = randBlue.iplayer.PlayTurn(board, new List<Tile> { tile1, tile2}, 35); // both these tiles should kill player

            board.PlaceTile(t, 0, 0);
            board.MovePlayer("blue");
            Assert.AreEqual(2, t.id);
            Assert.IsFalse(board.IsDead("blue"));
            // will randomly select an orientation that will leave the player at port 5 or 2
            Assert.IsTrue(board.tokenPositions["blue"].port == 5 || board.tokenPositions["blue"].port == 2);
        }

        [TestMethod]
        public void TestRandomPlayerPlayTurnMutlipleLegalTiles() {
            Board board = new Board(6);
            randBlue.iplayer.Initialize(randBlue.Color, new List<string> { "blue", "red", "hotpink" });
            Position p = randBlue.iplayer.PlacePawn(board);
            board.AddPlayerToken("blue", p);
            board.tokenPositions["blue"] = new Position(0, -1, 5); // we want to set new position so player dies

            Tile tile1 = new Tile(1, new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 });
            Tile tile2 = new Tile(2, new List<int> { 0, 1, 2, 4, 3, 6, 5, 7 });
            Tile tile3 = new Tile(6, new List<int> { 0, 4, 1, 7, 2, 3, 5, 6 });
            Tile t = randBlue.iplayer.PlayTurn(board, new List<Tile> { tile1, tile2, tile3 }, 35); // both these tiles should kill player


            Assert.IsTrue(t.id == 2 || t.id == 6);
            board.PlaceTile(t, 0, 0);
            board.MovePlayer("blue");

            Assert.IsFalse(board.IsDead("blue"));
        }


        [TestMethod]
        public void TestLeastSymmetricPlayerPlayTurnNoLegalHand()
        {
            LeastSymmetricPlayer mPlayer = new LeastSymmetricPlayer("mark");
            List<string> other_colors = new List<string>();
            foreach (string color in Constants.colors)
            {
                if (color != "blue")
                {
                    other_colors.Add(color);
                }
            }
            mPlayer.Initialize("blue", other_colors);
            server.AddPlayer(mPlayer, "blue");
            Player player = new Player(mPlayer, "blue");
            server.board.AddPlayerToken("blue", new Position(0, -1, 5));

            // symmetricity of 1
            player.AddTiletoHand(testTile1);

            // symmetricity of 4
            Tile testTile4 = new Tile(4, new List<int>(8) {
                0, 5, 1, 3, 2, 6, 4, 7
            });
            player.AddTiletoHand(testTile4);

            // right of tile placement
            Tile testTile5 = new Tile(5, new List<int>(8) {
                6, 0, 7, 1, 2, 3, 4, 5
            });
            // below of tile placement
            Tile testTile6 = new Tile(6, new List<int>(8) {
                0, 7, 1, 6, 2, 3, 4, 5
            });

            server.board.PlaceTile(testTile5, 1, 0);
            server.board.PlaceTile(testTile6, 0, 1);

            server.gameState = Server.State.loop;
            mPlayer.playerState = LeastSymmetricPlayer.State.loop;
            Tile tobePlayed = player.iplayer.PlayTurn(server.board, player.Hand, 33);

            Assert.IsTrue(testTile4.CompareByPath(tobePlayed));
        }

        [TestMethod]
        public void TestLeastSymmetricPlayerPlayTurnOneLegalHand()
        {
            LeastSymmetricPlayer mPlayer = new LeastSymmetricPlayer("mark");
            List<string> other_colors = new List<string>();
            foreach (string color in Constants.colors)
            {
                if (color != "blue")
                {
                    other_colors.Add(color);
                }
            }
            mPlayer.Initialize("blue", other_colors);
            server.AddPlayer(mPlayer, "blue");
            Player player = new Player(mPlayer, "blue");
            server.board.AddPlayerToken("blue", new Position(0, -1, 5));

            // symmetricity of 1
            player.AddTiletoHand(testTile1);

            // symmetricity of 4
            Tile testTile4 = new Tile(4, new List<int>(8) {
                0, 5, 1, 3, 2, 6, 4, 7
            });
            player.AddTiletoHand(testTile4);

            server.gameState = Server.State.safe;
            mPlayer.playerState = LeastSymmetricPlayer.State.loop;
            Tile tobePlayed = player.iplayer.PlayTurn(server.board, player.Hand, 33);
            Assert.AreEqual(1, server.alive.Count);
         
            Assert.IsTrue(testTile4.CompareByPath(tobePlayed));

            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    Assert.IsNull(server.board.tiles[i][j]);
                }
            }
        }

        [TestMethod]
        public void TestLeastSymmetricPlayerPlayTurnMultiLegalHand()
        {
            LeastSymmetricPlayer mPlayer = new LeastSymmetricPlayer("mark");
            List<string> other_colors = new List<string>();
            foreach (string color in Constants.colors)
            {
                if (color != "blue")
                {
                    other_colors.Add(color);
                }
            }
            mPlayer.Initialize("blue", other_colors);
            server.AddPlayer(mPlayer, "blue");
            Player player = new Player(mPlayer, "blue");
            server.board.AddPlayerToken("blue", new Position(0, -1, 5));

            // symmetricity of 1, illegal
            player.AddTiletoHand(testTile1);

            // symmetricity of 4, legal
            Tile testTile4 = new Tile(4, new List<int>(8) {
                0, 5, 1, 3, 2, 6, 4, 7
            });
            player.AddTiletoHand(testTile4);
            // symmetricity of 1, legal
            Tile testTile5 = new Tile(5, new List<int>(8) {
                0, 5, 1, 4, 2, 7, 6, 3
            });
            player.AddTiletoHand(testTile5);

            server.gameState = Server.State.safe;
            mPlayer.playerState = LeastSymmetricPlayer.State.loop;
            Tile tobePlayed = player.iplayer.PlayTurn(server.board, player.Hand, 32);

            Assert.IsTrue(testTile4.CompareByPath(tobePlayed));

            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    Assert.IsNull(server.board.tiles[i][j]);
                }
            }
        }

        [TestMethod]
        public void TestLeastSymmetricPlayerPlayTurnMultiLegalHandFirst()
        {
            LeastSymmetricPlayer mPlayer = new LeastSymmetricPlayer("mark");
            List<string> other_colors = new List<string>();
            foreach (string color in Constants.colors)
            {
                if (color != "blue")
                {
                    other_colors.Add(color);
                }
            }
            mPlayer.Initialize("blue", other_colors);
            server.AddPlayer(mPlayer, "blue");
            Player player = new Player(mPlayer, "blue");
            server.board.AddPlayerToken("blue", new Position(0, -1, 5));

            // symmetricity of 1, illegal
            player.AddTiletoHand(testTile1);

            // symmetricity of 4, legal
            Tile testTile4 = new Tile(4, new List<int>(8) {
                0, 5, 1, 3, 2, 6, 4, 7
            });
            // symmetricity of 4, legal
            Tile testTile5 = new Tile(5, new List<int>(8) {
                0, 2, 1, 4, 3, 7, 5, 6
            });
            player.AddTiletoHand(testTile4);
            player.AddTiletoHand(testTile5);

            server.gameState = Server.State.safe;
            mPlayer.playerState = LeastSymmetricPlayer.State.loop;
            Tile tobePlayed = player.iplayer.PlayTurn(server.board, player.Hand, 32);

            // same symmetricity, both 4 and 5 legal. But 4 was added to  hand first so it should be 4
            Assert.IsTrue(testTile4.CompareByPath(tobePlayed));

            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    Assert.IsNull(server.board.tiles[i][j]);
                }
            }
        }

        [TestMethod]
        public void TestLeastSymmetricPlayerPlayTurnMultiLegalHandFirstFlipped()
        {
            LeastSymmetricPlayer mPlayer = new LeastSymmetricPlayer("mark");
            List<string> other_colors = new List<string>();
            foreach (string color in Constants.colors)
            {
                if (color != "blue")
                {
                    other_colors.Add(color);
                }
            }
            mPlayer.Initialize("blue", other_colors);
            server.AddPlayer(mPlayer, "blue");
            Player player = new Player(mPlayer, "blue");
            server.board.AddPlayerToken("blue", new Position(0, -1, 5));

            // symmetricity of 1, illegal
            player.AddTiletoHand(testTile1);

            // symmetricity of 4, legal
            Tile testTile4 = new Tile(4, new List<int>(8) {
                0, 5, 1, 3, 2, 6, 4, 7
            });
            // symmetricity of 4, legal
            Tile testTile5 = new Tile(5, new List<int>(8) {
                0, 2, 1, 4, 3, 7, 5, 6
            });
            player.AddTiletoHand(testTile5);
            player.AddTiletoHand(testTile4);

            server.gameState = Server.State.safe;
            mPlayer.playerState = LeastSymmetricPlayer.State.loop;
            Tile tobePlayed = player.iplayer.PlayTurn(server.board, player.Hand, 32);

            // same symmetricity, both 4 and 5 legal. But 4 was added to  hand first so it should be 4
            Assert.IsTrue(testTile5.CompareByPath(tobePlayed));

            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    Assert.IsNull(server.board.tiles[i][j]);
                }
            }
        }

        [TestMethod]
        public void TestMostSymmetricPlayerPlayTurnNoLegalHand()
        {
            MostSymmetricPlayer mPlayer = new MostSymmetricPlayer("mark");
            List<string> other_colors = new List<string>();
            foreach (string color in Constants.colors)
            {
                if (color != "blue")
                {
                    other_colors.Add(color);
                }
            }
            mPlayer.Initialize("blue", other_colors);
            server.AddPlayer(mPlayer, "blue");
            Player player = new Player(mPlayer, "blue");
            server.board.AddPlayerToken("blue", new Position(0, -1, 5));

            // symmetricity of 1
            player.AddTiletoHand(testTile1);

            // symmetricity of 4
            Tile testTile4 = new Tile(4, new List<int>(8) {
                0, 5, 1, 3, 2, 6, 4, 7
            });
            player.AddTiletoHand(testTile4);

            // right of tile placement
            Tile testTile5 = new Tile(5, new List<int>(8) {
                6, 0, 7, 1, 2, 3, 4, 5
            });
            // below of tile placement
            Tile testTile6 = new Tile(6, new List<int>(8) {
                0, 7, 1, 6, 2, 3, 4, 5
            });

            server.board.PlaceTile(testTile5, 1, 0);
            server.board.PlaceTile(testTile6, 0, 1);

            server.gameState = Server.State.safe;
            mPlayer.playerState = MostSymmetricPlayer.State.loop;
            Tile tobePlayed = player.iplayer.PlayTurn(server.board, player.Hand, 33);

            Assert.IsTrue(testTile1.CompareByPath(tobePlayed));
        }

        [TestMethod]
        public void TestMostSymmetricPlayerPlayTurnOneLegalHand()
        {
            MostSymmetricPlayer mPlayer = new MostSymmetricPlayer("mark");

            List<string> other_colors = new List<string>();
            foreach (string color in Constants.colors)
            {
                if (color != "blue")
                {
                    other_colors.Add(color);
                }
            }
            mPlayer.Initialize("blue", other_colors);
            server.AddPlayer(mPlayer, "blue");
            Player player = new Player(mPlayer, "blue");
            server.board.AddPlayerToken("blue", new Position(0, -1, 5));

            // symmetricity of 1
            player.AddTiletoHand(testTile1);

            // symmetricity of 4
            Tile testTile4 = new Tile(4, new List<int>(8) {
                0, 5, 1, 3, 2, 6, 4, 7
            });
            player.AddTiletoHand(testTile4);

            server.gameState = Server.State.safe;
            mPlayer.playerState = MostSymmetricPlayer.State.loop;
            Tile tobePlayed = player.iplayer.PlayTurn(server.board, player.Hand, 33);
            Assert.AreEqual(1, server.alive.Count);

            Assert.IsTrue(testTile4.CompareByPath(tobePlayed));

            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    Assert.IsNull(server.board.tiles[i][j]);
                }
            }
        }

        [TestMethod]
        public void TestMostSymmetricPlayerPlayTurnMultiLegalHand()
        {
            MostSymmetricPlayer mPlayer = new MostSymmetricPlayer("mark");
            List<string> other_colors = new List<string>();
            foreach (string color in Constants.colors)
            {
                if (color != "blue")
                {
                    other_colors.Add(color);
                }
            }
            mPlayer.Initialize("blue", other_colors);
            server.AddPlayer(mPlayer, "blue");
            Player player = new Player(mPlayer, "blue");
            server.board.AddPlayerToken("blue", new Position(0, -1, 5));

            // symmetricity of 1, illegal
            player.AddTiletoHand(testTile1);

            // symmetricity of 4
            Tile testTile4 = new Tile(4, new List<int>(8) {
                0, 5, 1, 3, 2, 6, 4, 7
            });
            player.AddTiletoHand(testTile4);
            // symmetricity of 1, legal
            Tile testTile5 = new Tile(5, new List<int>(8) {
                0, 5, 1, 4, 2, 7, 6, 3
            });
            player.AddTiletoHand(testTile5);



            server.gameState = Server.State.safe;
            mPlayer.playerState = MostSymmetricPlayer.State.loop;
            Tile tobePlayed = player.iplayer.PlayTurn(server.board, player.Hand, 33);
            Assert.AreEqual(1, server.alive.Count);

            Assert.IsTrue(testTile5.CompareByPath(tobePlayed));
        }

        [TestMethod]
        public void TestMostSymmetricPlayerPlayTurnMultiLegalHandFirst()
        {
            MostSymmetricPlayer mPlayer = new MostSymmetricPlayer("mark");
            List<string> other_colors = new List<string>();
            foreach (string color in Constants.colors)
            {
                if (color != "blue")
                {
                    other_colors.Add(color);
                }
            }
            mPlayer.Initialize("blue", other_colors);
            server.AddPlayer(mPlayer, "blue");
            Player player = new Player(mPlayer, "blue");
            server.board.AddPlayerToken("blue", new Position(0, -1, 5));

            // symmetricity of 1, illegal
            player.AddTiletoHand(testTile1);

            // symmetricity of 4
            Tile testTile4 = new Tile(4, new List<int>(8) {
                0, 5, 1, 3, 2, 6, 4, 7
            });
            player.AddTiletoHand(testTile4);
            // symmetricity of 4, legal
            Tile testTile5 = new Tile(5, new List<int>(8) {
                0, 2, 1, 4, 3, 7, 5, 6
            });
            player.AddTiletoHand(testTile5);
            server.gameState = Server.State.safe;
            mPlayer.playerState = MostSymmetricPlayer.State.loop;
            Tile tobePlayed = player.iplayer.PlayTurn(server.board, player.Hand, 33);

            Assert.IsTrue(testTile4.CompareByPath(tobePlayed));
        }

        [TestMethod]
        public void TestMostSymmetricPlayerPlayTurnMultiLegalHandFirstFlipped()
        {
            MostSymmetricPlayer mPlayer = new MostSymmetricPlayer("mark");
            List<string> other_colors = new List<string>();
            foreach(string color in Constants.colors){
                if(color !=  "blue"){
                    other_colors.Add(color);
                }
            }
            Player player = new Player(mPlayer, "blue");
            mPlayer.Initialize("blue", other_colors);
            server.AddPlayer(mPlayer, "blue");
            server.board.AddPlayerToken("blue", new Position(0, -1, 5));

            // symmetricity of 1, illegal
            player.AddTiletoHand(testTile1);

            // symmetricity of 4
            Tile testTile4 = new Tile(4, new List<int>(8) {
                0, 5, 1, 3, 2, 6, 4, 7
            });
            // symmetricity of 4, legal
            Tile testTile5 = new Tile(5, new List<int>(8) {
                0, 2, 1, 4, 3, 7, 5, 6
            });
            player.AddTiletoHand(testTile5);
            player.AddTiletoHand(testTile4);

            server.gameState = Server.State.safe;
            mPlayer.playerState = MostSymmetricPlayer.State.loop;

            Tile tobePlayed = player.iplayer.PlayTurn(server.board, player.Hand, 33);
            Assert.IsTrue(testTile5.CompareByPath(tobePlayed));
        }

        [TestMethod]
        public void TestReplacePlayer()
        {
            MPlayer1 mPlayer1 = new MPlayer1("john");
            LeastSymmetricPlayer replacement = new LeastSymmetricPlayer("adam");

            Player player = new Player(mPlayer1, "blue");
            player.ReplaceIPlayer(replacement);
            Assert.AreEqual("adam", player.iplayer.GetName());
        }


    }
}
