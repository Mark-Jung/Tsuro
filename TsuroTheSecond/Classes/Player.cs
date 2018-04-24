﻿using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace TsuroTheSecond
{
    public class Player
    {
        public List<int> position;
        public int onward;
        List<Tile> Hand;
        int age;
        string color;

        public Player(List<int> _position, int _onward, List<Tile> _Hand, int _age, string _color)
        {
            Hand = _Hand;
            age = _age;
            color = _color;
            // port, x, y range check
            if ( (_position[2] < 8 || _position[2] > -1) && ( ((_position[0] == -1) || (_position[0] == 6)) ^ ( (_position[1] == -1) || (_position[1] == 6)) ) ) {
                position = _position;
            } else {
                throw new ArgumentException("Illegal position to initialize player", "_position");
            }
            // invalid _onward
            if ( _onward != (position[2] / 2) ) {
                throw new ArgumentException("Illegal onward value", "_onward");
            }
            // invalid combinations of _onward and initial position
            switch(_onward) {
                case 0:
                    if( _position[1] == 6 ){}
                    else{throw new ArgumentException("Illegal onward value", "_onward");}
                    break;
                case 1:
                    if (_position[0] == -1) { }
                    else { throw new ArgumentException("Illegal onward value", "_onward"); }
                    break;
                case 2:
                    if (_position[1] == -1) { }
                    else { throw new ArgumentException("Illegal onward value", "_onward"); }
                    break;
                case 3:
                    if (_position[0] == 6) { }
                    else { throw new ArgumentException("Illegal onward value", "_onward"); }
                    break;
                default:
                    throw new ArgumentException("Illegal onward value", "_onward");
            }
            onward = _onward;
        }   

        public Boolean CheckDead() {
            if ( (this.position[0] < 0) || (this.position[0] > 5) || (this.position[1] < 0) || (this.position[1] > 5) ) {
                return true;
            } else {
                return false;
            }
        }

        public void UpdatePosition(Board board){
            List<int> cur_pos = new List<int>(3){0, 0, 0};
            List<int> nxt_pos = new List<int>(3){0, 0, 0};
            int[] port_table = new int[]{5, 4, 7, 6, 1, 0, 3, 2};
            Tile nxt_tile = null;
            int enter_port, heading;
            bool recur = true;
            cur_pos = this.position;
            heading = this.onward;

            while (recur) {
                // calculate next x, y from onward

                switch (heading) {
                    case 0:
                        nxt_pos[0] = cur_pos[0];
                        nxt_pos[1] = cur_pos[1] - 1;
                        break;
                    case 1:
                        nxt_pos[0] = cur_pos[0] + 1;
                        nxt_pos[1] = cur_pos[1];
                        break;
                    case 2:
                        nxt_pos[0] = cur_pos[0];
                        nxt_pos[1] = cur_pos[1] + 1;
                        break;
                    case 3:
                        nxt_pos[0] = cur_pos[0] - 1;
                        nxt_pos[1] = cur_pos[1];
                        break;
                    default:
                        break;
                }
                // see if that position if the next tile is there.
                // if not, break
                // if so, update cur_pos

                try{
                    nxt_tile = board.tiles[nxt_pos[1]][nxt_pos[0]];
                } catch(IndexOutOfRangeException) {
                    recur = false;
                }
                if (nxt_tile == null) {
                    recur = false;
                } else {
                    cur_pos[0] = nxt_pos[0];
                    cur_pos[1] = nxt_pos[1];
                    enter_port = port_table[cur_pos[2]];
                    // find destination port in tile by enterport and update cur_pos
                    cur_pos[2] = nxt_tile.FindEndofPath(enter_port);
                    heading = cur_pos[2] / 2;
                }
            }

            this.position = cur_pos;
            this.onward = heading;

        }
    }
}
