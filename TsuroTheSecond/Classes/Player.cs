﻿using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace TsuroTheSecond
{
    public class Player
    {
        public readonly string Color;
        public List<Tile> Hand;
        public IPlayer player;

        public Player(IPlayer p, string c) {
            if (!Constants.colors.Contains(c)) {
                throw new ArgumentException("Color not allowed");
            }
            Hand = new List<Tile>();
            player = p;
            Color = c;
        }

        public void AddTiletoHand(Tile tile){
            if( this.Hand.Count > 3 ) {
                throw new Exception("Player hand is already full!");
            }
            this.Hand.Add(tile);
        }

        public void RemoveTilefromHand(Tile tile) {
            if( this.Hand.Count < 0 ) {
                throw new Exception("Player hand is already empty!");
            }
            int hand_cnt = this.Hand.Count;
            for (int i = 0; i < this.Hand.Count; i++) {
                if( this.Hand[i].id == tile.id ) {
                    this.Hand.Remove(this.Hand[i]);
                }
            }
            if(hand_cnt == this.Hand.Count) {
                throw new Exception("Remove Tile from hand was not effective");
            }
        }

        public bool TileinHand(Tile tile) {
            if ( this.Hand.Find(each => each.id == tile.id) == null ) {
                return false;
            } else {
                return true;
            }

        }
    }
}
