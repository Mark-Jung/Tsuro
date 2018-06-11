using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TsuroTheSecond;

namespace TsuroTheSecondTests
{
    [TestClass]
    public class BoardTest
    {
        Server server;
        List<Tile> tiles;

        [TestInitialize]
        public void Initialize()
        {
            server = new Server();
            tiles = new List<Tile>{
                new Tile(1, new List<int>{0, 1, 2, 3, 4, 5, 6, 7}),
                new Tile(2, new List<int>{0, 1, 2, 4, 3, 6, 5, 7}),
                new Tile(3, new List<int>{0, 6, 1, 5, 2, 4, 3, 7}),
                new Tile(4, new List<int>{0, 5, 1, 4, 2, 7, 3, 6}),
            };
        }
        void AddTwoPlayers()
        {
            MostSymmetricPlayer p1 = new MostSymmetricPlayer("john");
            MostSymmetricPlayer p2 = new MostSymmetricPlayer("john");

            server.AddPlayer(p1, "blue");
            server.AddPlayer(p2, "green");
        }

        void AddFourPlayers()
        {
            MostSymmetricPlayer p1 = new MostSymmetricPlayer("john");
            MostSymmetricPlayer p2 = new MostSymmetricPlayer("jim");
            MostSymmetricPlayer p3 = new MostSymmetricPlayer("harry");
            MostSymmetricPlayer p4 = new MostSymmetricPlayer("miles");

            server.AddPlayer(p1, "blue");
            server.AddPlayer(p2, "green");
            server.AddPlayer(p3, "red");
            server.AddPlayer(p4, "hotpink");
        }

        [TestMethod]
        public void TestConstructor() {
            Board board = new Board(6);
            List<List<Tile>> initBoard = new List<List<Tile>> {
                new List<Tile>{null, null, null, null, null, null},
                new List<Tile>{null, null, null, null, null, null},
                new List<Tile>{null, null, null, null, null, null},
                new List<Tile>{null, null, null, null, null, null},
                new List<Tile>{null, null, null, null, null, null},
                new List<Tile>{null, null, null, null, null, null},
            };
            CollectionAssert.Equals(initBoard, board);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Board size must be > 0")]
        public void TestConstructorNegativeboard()
        {
            Board board = new Board(-1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Tile placement is out of board range")]
        public void TestPlaceTileOutOfRange1() {
            Board board = new Board(6);
            Tile tile = new Tile(1, new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 }); 
            board.PlaceTile(tile, 1, 9);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Tile placement is out of board range")]
        public void TestPlaceTileOutOfRange2()
        {
            Board board = new Board(6);
            Tile tile = new Tile(1, new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 });
            board.PlaceTile(tile, -1, 1);
        }

        [TestMethod]
        public void TestPlaceAndFreeTileSpace()
        {
            Board board = new Board(6);
            Tile tile = new Tile(1, new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 });
            Assert.IsNull(board.tiles[1][1]);
            board.PlaceTile(tile, 1, 1);
            Assert.IsNotNull(board.tiles[1][1]);
        }

        [TestMethod]
        public void TestTakenTokenSpot() {
            Board board = new Board(6);
            board.AddPlayerToken("blue", new Position(0, -1, 5));
            Assert.IsFalse(board.FreeTokenSpot(new Position(0, -1, 5)));
        }

        [TestMethod]
        public void TestFreeTokenSpot()
        {
            Board board = new Board(6);
            board.AddPlayerToken("blue", new Position(0, -1, 5));
            Assert.IsTrue(board.FreeTokenSpot(new Position(0, -1, 4)));
        }

        [TestMethod]
        public void TestAddPlayerToken() {
            Board board = new Board(6);
            board.AddPlayerToken("blue", new Position(0, -1, 5));
            board.AddPlayerToken("green", new Position(0, -1, 4));

            Assert.AreEqual(0, board.tokenPositions["blue"].x);
            Assert.AreEqual(-1, board.tokenPositions["blue"].y);
            Assert.AreEqual(5, board.tokenPositions["blue"].port);

            Assert.AreEqual(0, board.tokenPositions["green"].x);
            Assert.AreEqual(-1, board.tokenPositions["green"].y);
            Assert.AreEqual(4, board.tokenPositions["green"].port);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TestAddDuplicateColorPlayerToken()
        {
            Board board = new Board(6);
            board.AddPlayerToken("blue", new Position(0, -1, 5));
            board.AddPlayerToken("blue", new Position(0, -1, 4));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TestAddDuplicatePositionPlayerToken()
        {
            Board board = new Board(6);
            board.AddPlayerToken("blue", new Position(0, -1, 5));
            board.AddPlayerToken("green", new Position(0, -1, 5));
        }

        [TestMethod]
        public void TestIsDead()
        {
            Board board = new Board(6);
            board.AddPlayerToken("blue", new Position(0, -1, 5));
            Tile tile = new Tile(1, new List<int> { 0, 2, 3, 4, 5, 6, 7, 1 });
            board.PlaceTile(tile, 0, 0);
            board.MovePlayer("blue");

            Assert.IsFalse(board.IsDead("blue"));
        }

        [TestMethod]
        public void TestIsDeadActuallyDead()
        {
            Board board = new Board(6);
            board.AddPlayerToken("blue", new Position(0, -1, 5));
            Tile tile = new Tile(1, new List<int> { 0, 1, 3, 4, 5, 6, 7, 2 });
            board.PlaceTile(tile, 0, 0);
            board.MovePlayer("blue");

            Assert.IsTrue(board.IsDead("blue"));
        }

        [TestMethod]
        public void TestMovePlayer()
        {
            // start at 5, 6 and at port 0
            server.board.AddPlayerToken("blue", new Position(5, 6, 0));
            Tile testTile_1 = new Tile(1, new List<int>(8) {
                0, 4, 1, 5, 2, 6, 3, 7
            });
            // places a tile that gives direct path up 
            server.board.PlaceTile(testTile_1, 5, 5);
            // move and update position of the player
            server.board.MovePlayer("blue");
            // check position
            Position p = server.board.tokenPositions["blue"];

            Assert.AreEqual(p.x, 5);
            Assert.AreEqual(p.y, 5);
            Assert.AreEqual(p.port, 1);
        }

        [TestMethod]
        public void TestMovePlayerInductiveCase()
        {
            server.board.AddPlayerToken("blue", new Position(5, 6, 0));
            // start at 5, 6 and at port 0

            Tile testTile_1 = new Tile(1, new List<int>(8) {
                0, 4, 1, 5, 2, 6, 3, 7
            });
            Tile testTile_2 = new Tile(2, new List<int>(8) {
                4, 7, 0, 6, 1, 3, 5, 2
            });
            // places a tile that gives direct path up 
            server.board.PlaceTile(testTile_1, 5, 5);
            server.board.PlaceTile(testTile_2, 5, 4);
            // move and update position of the player
            server.board.MovePlayer("blue");
            // check position
            Assert.IsFalse(server.board.IsDead("blue"));
            Assert.AreEqual(5, server.board.tokenPositions["blue"].x);
            Assert.AreEqual(4, server.board.tokenPositions["blue"].y);
            Assert.AreEqual(7, server.board.tokenPositions["blue"].port);
        }

        [TestMethod]
        public void TestMovePlayerMultiMove()
        {
            
            server.board.AddPlayerToken("blue", new Position(5, 6, 0));
            server.board.AddPlayerToken("green", new Position(6, 4, 6));
           
            Tile testTile_1 = new Tile(1, new List<int>(8) {
                0, 4, 1, 5, 2, 6, 3, 7
            });
            Tile testTile_2 = new Tile(2, new List<int>(8) {
                4, 7, 0, 6, 1, 3, 5, 2
            });
            // places a tile that gives direct path up 
            server.board.PlaceTile(testTile_1, 5, 5);
            server.board.PlaceTile(testTile_2, 5, 4);
            // move and update position of the player
            server.board.MovePlayer("green");
            server.board.MovePlayer("blue");
            // check position
            Assert.IsFalse(server.board.IsDead("blue"));
            Assert.AreEqual(5, server.board.tokenPositions["blue"].x);
            Assert.AreEqual(4, server.board.tokenPositions["blue"].y);
            Assert.AreEqual(7, server.board.tokenPositions["blue"].port);


            Assert.IsFalse(server.board.IsDead("green"));
            Assert.AreEqual(5, server.board.tokenPositions["green"].x);
            Assert.AreEqual(4, server.board.tokenPositions["green"].y);
            Assert.AreEqual(1, server.board.tokenPositions["green"].port);
        }

        [TestMethod]
        public void TestMovePlayerMultiKill()
        {
            server.board.AddPlayerToken("blue", new Position(5, 6, 1));
            server.board.AddPlayerToken("green", new Position(6, 4, 7));

            Tile testTile_1 = new Tile(1, new List<int>(8) {
                0, 4, 1, 5, 2, 6, 3, 7
            });
            Tile testTile_2 = new Tile(2, new List<int>(8) {
                4, 7, 0, 6, 1, 3, 5, 2
            });
            // places a tile that gives direct path up 
            server.board.PlaceTile(testTile_1, 5, 5);
            server.board.PlaceTile(testTile_2, 5, 4);
            // move and update position of the player
            server.board.MovePlayer("blue");
            server.board.MovePlayer("green");

            // check position
            Assert.IsTrue(server.board.IsDead("blue"));
            Assert.AreEqual(6, server.board.tokenPositions["blue"].x);
            Assert.AreEqual(4, server.board.tokenPositions["blue"].y);
            Assert.AreEqual(7, server.board.tokenPositions["blue"].port);


            Assert.IsTrue(server.board.IsDead("green"));   
            Assert.AreEqual(5, server.board.tokenPositions["green"].x);
            Assert.AreEqual(6, server.board.tokenPositions["green"].y);
            Assert.AreEqual(1, server.board.tokenPositions["green"].port);

        }

        [TestMethod]
        public void TestMovePlayerRotatedTile()
        {
            // start at 5, 6 and at port 0
            server.board.AddPlayerToken("blue", new Position(6, 4, 7));

            Tile testTile_2 = new Tile(2, new List<int>(8) {
                4, 7, 0, 6, 1, 3, 5, 2
            });
            testTile_2.Rotate();

            server.board.PlaceTile(testTile_2, 5, 4);
            // move and update position of the player
            server.board.MovePlayer("blue");
            // check position
            Assert.IsFalse(server.board.IsDead("blue"));
            Assert.AreEqual(server.board.tokenPositions["blue"].x, 5);
            Assert.AreEqual(server.board.tokenPositions["blue"].y, 4);
            Assert.AreEqual(server.board.tokenPositions["blue"].port, 0);   
        }



        [TestMethod]
        public void TestValidTilePlacement()
        {
            AddFourPlayers();

            server.board.AddPlayerToken(server.alive[0].Color, new Position(4, 6, 0));
            Tile testTile = new Tile(1, new List<int> { 1, 2, 3, 4, 5, 6, 7, 0 });
            Assert.IsTrue(server.board.ValidTilePlacement(server.alive[0].Color, testTile));
        }

        [TestMethod]
        public void TestValidTilePlacementFalse()
        {
            AddFourPlayers();

            server.board.AddPlayerToken(server.alive[0].Color, new Position(4, 6, 0));
            Tile testTile = new Tile(1, new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 });
            Assert.IsFalse(server.board.ValidTilePlacement(server.alive[0].Color, testTile));
        }

        [TestMethod]
        public void TestValidTilePlacementFalse1() {
            AddFourPlayers();

            server.board.AddPlayerToken(server.alive[0].Color, new Position(0, -1, 5));
            Assert.IsFalse(server.board.ValidTilePlacement(server.alive[0].Color, tiles[0]));
        }

        [TestMethod]
        public void TestValidTilePlacementFalse2()
        {
            AddFourPlayers();

            server.board.AddPlayerToken(server.alive[0].Color, new Position(0, -1, 5));
            //Console.WriteLine(tiles[1].id);
            //Constants.tiles[1].PrintMe();
            Assert.IsFalse(server.board.ValidTilePlacement(server.alive[0].Color, tiles[1]));
        }

        [TestMethod]
        public void TestValidTilePlacementFalse3()
        {
            AddFourPlayers();

            server.board.AddPlayerToken(server.alive[0].Color, new Position(0, -1, 5));
            Tile tile = new Tile (tiles[1]);
            tile.Rotate();
            tile.Rotate();
            Assert.IsFalse(server.board.ValidTilePlacement(server.alive[0].Color, tile));
        }

        [TestMethod]
        public void TestValidTilePlacementTrue1()
        {
            AddFourPlayers();

            server.board.AddPlayerToken(server.alive[0].Color, new Position(0, -1, 5));

            Tile tile = new Tile(tiles[1]);
            tile.Rotate();
            Assert.IsTrue(server.board.ValidTilePlacement(server.alive[0].Color, tile));
        }

        [TestMethod]
        public void TestValidTilePlacementTrue2()
        {
            AddFourPlayers();

            server.board.AddPlayerToken(server.alive[0].Color, new Position(0, -1, 5));
            Tile tile = new Tile(tiles[1]);
            tile.Rotate();
            tile.Rotate();
            tile.Rotate();

            Assert.IsTrue(server.board.ValidTilePlacement(server.alive[0].Color, tile));
        }

        [TestMethod]
        public void TestAllPossibleTilesFullHand()
        {
            AddTwoPlayers();
            server.board.AddPlayerToken("blue", new Position(4, 6, 0));
            server.board.AddPlayerToken("green", new Position(4, 6, 1));


            Tile testTile1 = new Tile(1, new List<int>(8) {
                0, 1, 2, 3, 4, 5, 6, 7,
            });
            Tile testTile2 = new Tile(2, new List<int>(8) {
                0, 1, 2, 3, 4, 5, 6, 7,
            });
            Tile testTile3 = new Tile(3, new List<int>(8) {
                0, 5, 1, 4, 2, 7, 3, 6,
            });
            Tile testTile4 = new Tile(4, new List<int>(8)
            {
                0, 4, 1, 5, 2, 6, 3, 7
            });

            server.alive[0].Hand = new List<Tile> { testTile1, testTile2, testTile3 };

            List<Tile> every_combi_blue = server.board.AllPossibleTiles(server.alive[0].Color, server.alive[0].Hand);
            // only testTile3 is valid.
            Assert.AreEqual(4, every_combi_blue.Count);
            server.alive[1].Hand = new List<Tile> { testTile1, testTile3, testTile4 };
            List<Tile> every_combi_green = server.board.AllPossibleTiles(server.alive[1].Color, server.alive[1].Hand);

            Assert.AreEqual(8, every_combi_green.Count);

        }

        [TestMethod]
        public void TestAllPossibleTiles1Legal() {
            server.board.AddPlayerToken("blue", new Position(0, -1, 5));
            // one tile is illegal, second tile is legal(only one version is legal if rotated)
            List<Tile> legalTiles = server.board.AllPossibleTiles("blue", new List<Tile> { tiles[0], new Tile(1, new List<int>{0, 1, 2, 7, 3, 4, 5, 6}) });
            foreach( Tile each in legalTiles){
                each.PrintMe();
            }
            Assert.AreEqual(1, legalTiles.Count);

        }

        [TestMethod]
        public void TestCheckAllPossibleTilesReturn()
        {
            Board board = new Board(6);
            board.AddPlayerToken("blue", new Position(0, -1, 5));
            List<Tile> allTiles = board.AllPossibleTiles("blue", new List<Tile>{
                new Tile(1, new List<int>{0, 1, 2, 3, 4, 5, 6, 7}),
                new Tile(2, new List<int>{0, 1, 2, 4, 3, 6, 5, 7})
            });

            Assert.AreEqual(2, allTiles.Count);
            board.PlaceTile(allTiles[0], 0, 0);
            board.MovePlayer("blue");
            Assert.IsFalse(board.IsDead("blue"));
        }

        [TestMethod]
        public void TestAllPossibleTilesNoLegal()
        {
            AddTwoPlayers();
            server.board.AddPlayerToken("blue", new Position(4, 6, 0));

            Tile testTile1 = new Tile(1, new List<int>(8) {
                0, 1, 2, 3, 4, 5, 6, 7,
            });
            Tile testTile2 = new Tile(2, new List<int>(8) {
                0, 1, 2, 3, 4, 5, 6, 7,
            });
            Tile testTile3 = new Tile(3, new List<int>(8) {
                0, 1, 2, 3, 4, 5, 6, 7,
            });

            server.alive[0].Hand = new List<Tile> { testTile1, testTile2, testTile3 };

            List<Tile> every_combi_blue = server.board.AllPossibleTiles(server.alive[0].Color, server.alive[0].Hand);
            // only testTile3 is valid.
            Assert.AreEqual(12, every_combi_blue.Count);
        }
    }
}
