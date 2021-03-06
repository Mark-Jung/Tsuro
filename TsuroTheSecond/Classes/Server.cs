﻿using System;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;

namespace TsuroTheSecond
{
    public class Server 
    {

        public List<Tile> deck;
        public List<Player> alive;
        public List<Player> dead;
        public Board board;
        public List<Player> dragonQueue; // whoever is the first person of the queue has the tile
        public enum State { start, ready, loop, safe, end };
        public State gameState;

        public Server() {
            gameState = State.start;
            //Console.WriteLine("Starting game with state: start");
            // initializes the game
            dragonQueue = new List<Player>();
            deck = ShuffleDeck(Constants.tiles);
            alive = new List<Player>();
            dead = new List<Player>();
            board = new Board(Constants.boardSize);
        }

        public void AddPlayer(IPlayer p, string color) {
            if (gameState != State.start) {
                throw new Exception("Invalid game state");
            }

            if (!Constants.colors.Contains(color)) {
                throw new ArgumentException("Invalid color");
            }

            List<string> colors = alive.Select(x => x.Color).ToList();
            if (colors.Contains(color)) {
                throw new ArgumentException("Duplicate color");
            }

            // somehow check that at least 2 players are in teh game?

            if (alive.Count >= 8) {
                throw new InvalidOperationException("Only 8 players allowed in game");
            }
            // todo organize alive by age and don't let players pick duplicate colors
            alive.Add(new Player(p, color));
            Console.WriteLine(String.Format("Added player {0} with a {1} token!", p.GetName(), color));
            Console.WriteLine(String.Format("{0} : {1}", p.GetName(), color));
        }

        public void ReplacePlayer(Player player) {
            // might want a better way to do this
            List<IPlayer> iplayers = new List<IPlayer>();
            iplayers.Add(new RandomPlayer(player.iplayer.GetName()));
            iplayers.Add(new LeastSymmetricPlayer(player.iplayer.GetName()));
            iplayers.Add(new MostSymmetricPlayer(player.iplayer.GetName()));

            Random random = new Random();
            IPlayer replacement = iplayers[random.Next(0, 3)];

            while (replacement.GetType() == player.iplayer.GetType()) {
                replacement = iplayers[random.Next(0, 3)];
            }

            List<string> colorCopy = new List<string>();
            //foreach (string color in Constants.colors) {
            //    if (color != player.Color) {
            //        colorCopy.Add(color);
            //    }
            //}
            foreach(Player each in alive){
                colorCopy.Add(each.Color);
            }
            foreach(Player each in dead){
                colorCopy.Add(each.Color);
            }

            replacement.Initialize(player.Color, colorCopy);
            player.ReplaceIPlayer(replacement);
        }

        public void InitPlayerPositions() {
            if (gameState != State.start)
            {
                throw new Exception("Invalid game state");
            }

            foreach(Player p in alive) {
                p.iplayer.Initialize(p.Color, alive.Select(x => x.Color).ToList());
            }
            gameState = State.loop;
            //Console.WriteLine("Currently in InitPlayerPositions Server.cs");
            //Console.WriteLine("Current state is: loop");

            foreach(Player p in alive) {
                Position position = new Position(6, 1, 7);
                try
                {
                    position = p.iplayer.PlacePawn(this.board);
                }catch (ArgumentException)
                {
                    //Console.WriteLine("Player initialized invalid position and has been replaced");
                    ReplacePlayer(p);
                }
                //Console.WriteLine("Initialized player: " + p.Color);
                this.board.AddPlayerToken(p.Color, position);
                // initialize hand
                //Console.WriteLine("Initial Tile draw: ");
                this.DrawTile(p, deck);
                this.DrawTile(p, deck);
                this.DrawTile(p, deck);
                //Console.WriteLine("\n");
                //Console.WriteLine("\n");
            }

        }

        public List<Tile> ShuffleDeck(List<Tile> deck)
        {
            // doesnt quite work the way we want it to yet
            List<Tile> shuffledDeck = new List<Tile>();
            Random rng = new Random();
            HashSet<int> chosen = new HashSet<int>();

            while (shuffledDeck.Count < deck.Count)
            {
                int k = rng.Next(deck.Count);
                if (chosen.Contains(k)) { continue; }

                Tile tile = deck[k];
                int r = rng.Next(4);

                for (int i = 0; i < r; i++) {
                    tile.Rotate();
                }

                shuffledDeck.Add(tile);
                chosen.Add(k);
            }
            return shuffledDeck;
        }

        public Boolean LegalPlay(Player player, Board b, Tile tile) {
            if (!(gameState == State.loop || gameState == State.safe))
            {
                throw new Exception("Invalid game state");
            }
            gameState = State.safe;

            // Check for valid tile
            if (tile == null || !player.TileinHand(tile)) {
                ReplacePlayer(player);
                return false;
            }

            List<Tile> all_options = b.AllPossibleTiles(player.Color, player.Hand);

            // if there's no options that don't kill you, then any tile is legal
            if (all_options.Count == 0) {
                return true;
            }

            // try if the tile is in all_options
            // If so, return true
            foreach(Tile goodTile in all_options){
                if(goodTile.id == tile.id){
                    return true;
                }
            }

            ReplacePlayer(player);
            return false;
        }

        public (List<Tile>, List<Player>, List<Player>, Board, Boolean, List<Player>) PlayATurn(List<Tile> _deck,
                                                                                  List<Player> _alive,
                                                                                  List<Player> _dead,
                                                                                  Board _board,
                                                                                  Tile tile)

        {
            if (gameState != State.safe)
            {
                throw new Exception("Invalid game state");
            }

            gameState = State.loop;

            Player currentPlayer = this.alive[0];
            Player dragonPlayer = null;
            if(dragonQueue.Count > 0){
                dragonPlayer = this.dragonQueue[0];
            }

            if (currentPlayer.Hand.Count > 2)
            {
                throw new ArgumentException("Player should have 2 or less tiles in hand");
            }

            List<Tile> all_tiles = new List<Tile>();
            foreach (Tile t in currentPlayer.Hand)
            {
                if (t.id != tile.id)
                {
                    all_tiles.Add(t);
                }
                else
                {
                    throw new ArgumentException("Tile to be played can still be found in hand");
                }
            }

            int tileCount = 0;
            foreach (List<Tile> row in board.tiles)
            {
                foreach (Tile t in row)
                {
                    if (t == null) { continue; }
                    foreach (Tile dif_tile in all_tiles)
                    {
                        if (t.id == dif_tile.id)
                        {
                            throw new Exception("Tile to be played or in hand is already on board");
                        }
                    }
                    all_tiles.Add(t);
                    tileCount++;
                }
            }

            int total = currentPlayer.Hand.Count + tileCount;
            if (all_tiles.Count != total)
            {

                throw new ArgumentException("Tile to be placed, tiles in hand, and tiles on board are not unique");
            }

            // places tile
            var next = board.ReturnNextSpot(currentPlayer.Color);
            board.PlaceTile(tile, next.Item1, next.Item2);

            // consequence of moving
            List<Player> fatalities = new List<Player>();
            foreach (Player p in alive)
            {
                board.MovePlayer(p.Color);
                if (board.IsDead(p.Color))
                {
                    fatalities.Add(p);
                }
            }

            foreach (Player p in fatalities)
            {
                KillPlayer(p);
            }

            // put currentPlayer to end of _alive
            for (int i = 0; i < alive.Count; i++)
            {
                if (alive[i].Color == currentPlayer.Color)
                {
                    Player move_to_end = alive[i];
                    alive.Remove(move_to_end);
                    alive.Add(move_to_end);
                }
            }

            // current player is dead, don't draw
            bool isDead = false;
            foreach (Player p in dead)
            {
                if (p.Color == currentPlayer.Color)
                {
                    isDead = true;
                }
            }
            if (!isDead && dragonQueue.Count == 0 && currentPlayer.Hand.Count < 3)
            {
                DrawTile(currentPlayer, deck);
            }

            // check if 35 tiles have been placed.
            int tilecount = board.TilesOnBoard();
           
            Boolean GameDone = false;
            List<Player> Victors = new List<Player>();

            if(alive.Count == 0)
            {
                GameDone = true;
                Victors.AddRange(fatalities);
            }
            else if(alive.Count == 1 || (alive.Count > 1 && tilecount == 35))
            {
                GameDone = true;
                Victors.AddRange(alive);
            }

            return (deck, alive, dead, board, GameDone, Victors);
        }

        public void KillPlayer(Player player) {
            dead.Add(player);
            alive.Remove(player);


            while (dragonQueue.Contains(player)) {
                dragonQueue.Remove(player);
            }
            //Console.WriteLine("Redistribute hand size: " + player.Hand.Count);

            // distribute player hand to whoevers waiting in queue or just add to deck
            if (player.Hand.Count > 0) {
                deck.AddRange(player.Hand);
                int hand_cnt = player.Hand.Count;
                for (int i = 0; i < hand_cnt; i++)
                {
                    player.RemoveTilefromHand(player.Hand[0]);
                }
                int dragonqueueCount = dragonQueue.Count;

                for (int i = 0; (i < dragonqueueCount && deck.Count > 0); i++)
                {
                    DrawTile(dragonQueue[0], deck);
                    dragonQueue.Remove(dragonQueue[0]);
                }
            }
        }

        public void WinGame(List<Player> winners) {
            //Console.WriteLine("Seems like there are " + winners.Count + "winners!");
            //Console.WriteLine("The champion(s) is(are)...");
            List<string> winner_colors = new List<string>();

            foreach(Player each in winners){
                winner_colors.Add(each.Color);
                //Console.WriteLine(each.Color);
            }
            foreach (Player each in dead)
            {
                each.iplayer.EndGame(board, winner_colors);
            }
            foreach (Player each in alive)
            {
                each.iplayer.EndGame(board, winner_colors);
            }
        }

        public void DrawTile(Player player, List<Tile> d)
        {
            // how is this supposed to work with an interface?
            if (player.Hand.Count >= 3)
            {
                throw new InvalidOperationException("Player can't have more than 3 cards in hand");
            }
            //Console.WriteLine("Deck size is: " + d.Count);

            if (d.Count <= 0)
            {
                this.dragonQueue.Add(player);
            }
            else
            {
                Tile t = d[0];
                d.RemoveAt(0);
                player.AddTiletoHand(t);
                //Console.WriteLine("Player " + player.Color + " drew Tile!");
                //t.PrintMe();
                if(player.Hand.Count < 3){
                    dragonQueue.Add(player);
                }
            }
        }
    }
}
