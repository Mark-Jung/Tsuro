﻿using System;
using System.Linq;
using System.Collections.Generic;

namespace TsuroTheSecond
{
    public class Board : IBoard
    {
        // "board describes where tiles are, what orientation they're in, 
        // where each pawn is, and what color it is" - hw5
        public readonly List<List<Tile>> tiles;
        public Dictionary<string, Position> tokenPositions;
        public Dictionary<string, Position> initialPositions;

        public Board(int size)
        {
            if (size <= 0) {
                throw new ArgumentException("Board size must be > 0");
            }

            tiles = new List<List<Tile>>();
            for (int i = 0; i < size; i++)
            {
                tiles.Add(new List<Tile>(new Tile[size]));
            }

            tokenPositions = new Dictionary<string, Position>();
            initialPositions = new Dictionary<string, Position>();
        }

        public void PlaceTile(Tile tile, int x, int y)
        {
            if (InBoard(x, y)) 
            {
                this.tiles[x][y] = tile;
            }
            else
            {
                throw new ArgumentException("Tile placement is out of board range");
            }
        }

        public Boolean InBoard(int x, int y)
        {
            return !(x < 0 || x > 5 || y < 0 || y > 5);
        }

        public Boolean FreeTokenSpot(Position position) {
            return !tokenPositions.ContainsValue(position);
        }

        public void AddPlayerToken(string color, Position position) {
            // add safety check to make sure tiles arent initialized?
            // make sure color is in list of acceptable colors?

            if (tokenPositions.ContainsKey(color)) {
                throw new Exception("Initializing a second player of color " + color);
            }

            foreach (Position pos in tokenPositions.Values) {
                if (pos == position) {
                    throw new Exception("Initializing a second player at used position");
                }
            }

            tokenPositions.Add(color, position);
            initialPositions.Add(color, position);
        }

        public (int, int) ReturnNextSpot(string color) {
            return tokenPositions[color].WhatNext();
        }

        public Position ReturnPlayerSpot(string color)
        {
            return tokenPositions[color];
        }

        public Boolean IsDead(string color) {
            if(initialPositions.Count > 0) {
                if (initialPositions[color] == tokenPositions[color]) return false;
            }
            return ((tokenPositions[color].x < 0) ||
                    (tokenPositions[color].x > 5) ||
                    (tokenPositions[color].y < 0) ||
                    (tokenPositions[color].y > 5));
        }

        public Boolean ValidTilePlacement(string color, Tile tile)
        {
            // checks if placing a tile on the board will kill the player 
            // remember old positions
            Boolean playerAlive = true;
            var origNext = this.ReturnNextSpot(color);
            Position origPosition = new Position(this.ReturnPlayerSpot(color));
            if(InBoard(origNext.Item1, origNext.Item2))
            {
                this.PlaceTile(tile, origNext.Item1, origNext.Item2);
            }
            else
            {
                throw new Exception("Cannot place tile at this position");
            }

            this.MovePlayer(color);

            playerAlive = !this.IsDead(color);

            // undoing changes to the board
            this.PlaceTile(null, origNext.Item1, origNext.Item2);
            this.tokenPositions[color].x = origPosition.x;
            this.tokenPositions[color].y = origPosition.y;
            this.tokenPositions[color].port = origPosition.port;
            return playerAlive;
        }

        public List<Tile> AllPossibleTiles(string color, List<Tile> hands) {
            List<Tile> legal = new List<Tile>();
            List<Tile> illegal = new List<Tile>();
            //Console.WriteLine("Entered AllPossibleTiles for color: " + color);

            int hand_size = hands.Count;
            //Console.WriteLine("Hand size: " + hand_size);
            for (int i = 0; i < hand_size; i++) {
                for (int j = 0; j < 4; j++) {
                    Tile tile = new Tile(hands[i]);
                    for (int k = 0; k < j; k++) {
                        tile.Rotate(); 
                    }
                    //Console.WriteLine("Rotated " + j + "times!");
                    if (this.ValidTilePlacement(color, tile)) {
                        legal.Add(tile);
                    } else {
                        illegal.Add(tile);
                    }
                }
            }
            //Console.WriteLine("done sorting out the legal illegal tiles");
            // if none of the options are legal, return all illegal options
            //Console.WriteLine("legal count: " + legal.Count.ToString());
            if(legal.Count > 0){
                //Console.WriteLine("returning legal!");
                return legal;
            } else {
                //Console.WriteLine("returning illegal! :: " + illegal.Count);
                return illegal;
            }
        }

        public int TilesOnBoard()
        {
            int tilecount = 0;
            foreach (List<Tile> row in tiles)
            {
                foreach (Tile each in row)
                {
                    if (each != null)
                    {
                        tilecount++;
                    }
                }
            }
            return tilecount;
        }

        public void MovePlayer(string color)
        {
            Position cur_pos = new Position(tokenPositions[color]);
            List<int> nxt_pos = new List<int>(3) { 0, 0, 0 };
            int[] port_table = new int[] { 5, 4, 7, 6, 1, 0, 3, 2 };
            Tile nxt_tile = null;
            int enter_port, heading;
            bool recur = true;
            heading = cur_pos.port / 2;

            while (recur)
            {
                // calculate next x, y from onward
                switch (heading)
                {
                    case 0:
                        nxt_pos[0] = cur_pos.x;
                        nxt_pos[1] = cur_pos.y - 1;
                        break;
                    case 1:
                        nxt_pos[0] = cur_pos.x + 1;
                        nxt_pos[1] = cur_pos.y;
                        break;
                    case 2:
                        nxt_pos[0] = cur_pos.x;
                        nxt_pos[1] = cur_pos.y + 1;
                        break;
                    case 3:
                        nxt_pos[0] = cur_pos.x - 1;
                        nxt_pos[1] = cur_pos.y;
                        break;
                    default:
                        break;
                }
                // see if that position if the next tile is there.
                // if not, break
                // if so, update cur_pos


                if(nxt_pos[0] < 0 || nxt_pos[0] > 5 || nxt_pos[1] < 0 || nxt_pos[1] > 5)
                {
                    recur = false;
                } else {
                    nxt_tile = this.tiles[nxt_pos[0]][nxt_pos[1]];
                }
                if (nxt_tile == null){
                    recur = false;
                }
                else
                {
                    cur_pos.x = nxt_pos[0];
                    cur_pos.y = nxt_pos[1];
                    enter_port = port_table[cur_pos.port];
                    // find destination port in tile by enterport and update cur_pos
                    if (recur)
                    {
                        cur_pos.port = nxt_tile.FindEndofPath(enter_port);
                    }
                    else
                    {
                        cur_pos.port = enter_port;
                    }
                    heading = cur_pos.port / 2;
                }
            }

            tokenPositions[color] = cur_pos;
        }



    }
}
