using System;
namespace TsuroTheSecond
{
    public class Position
    {
        public int x;
        public int y;
        public int port;
        public Position(int _x, int _y, int _port, Boolean isInit = true)
        {
            if (isInit)
            {
                if ((_port < 8 && _port > -1) && (((_x == -1) || (_x == 6)) ^ ((_y == -1) || (_y == 6))))
                {
                    x = _x;
                    y = _y;
                    port = _port;
                }
                else
                {
                    throw new ArgumentException("Illegal position to initialize player");
                }
            
             
                switch (port / 2)
                {
                    case 0:
                        // onward is the tile above
                        if (y == 6) { }
                        else { throw new ArgumentException("Illegal port and xy combination"); }
                        break;
                    case 1:
                        if (x == -1) { }
                        else { throw new ArgumentException("Illegal port and xy combination"); }
                        break;
                    case 2:
                        if (y == -1) { }
                        else { throw new ArgumentException("Illegal port and xy combination"); }
                        break;
                    case 3:
                        if (x == 6) { }
                        else { throw new ArgumentException("Illegal port and xy combination"); }
                        break;
                    default:
                        throw new ArgumentException("Illegal port value");
                }
            }
            else
            {
                this.x = _x;
                this.y = _y;
                this.port = _port;
            }

        }

        public Position(Position copy)
        {
            x = copy.x;
            y = copy.y;
            port = copy.port;
        }

        public static bool operator ==(Position a, Position b) {
            return a.x == b.x && a.y == b.y && a.port == b.port;
        }

        public static bool operator !=(Position a, Position b)
        {
            return !(a.x == b.x && a.y == b.y && a.port == b.port);
        }

		public override bool Equals(object obj)
		{
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }

            Position position = (Position)obj;
            return this.x == position.x && this.y == position.y && this.port == position.port;
		}

		public override int GetHashCode()
		{
            String s = this.x.ToString() + this.y.ToString() + this.port.ToString();
            return s.GetHashCode();
		}

		public (int, int) WhatNext(){
            // invalid _onward
            int next_x = x;
            int next_y = y;
            switch (port / 2)
            {
                case 0:
                    // onward is the tile above
                    next_y -= 1;
                    if (next_y < 0)
                    {
                        throw new ArgumentException("I think the player is dead?");
                    }
                    break;
                case 1:
                    // onward is the tile to the right
                    next_x += 1;
                    if (next_x > Constants.boardSize - 1)
                    {
                        throw new ArgumentException("I think the player is dead?");
                    }
                    break;
                case 2:
                    // onward is the tile below
                    next_y += 1;
                    if (next_y > Constants.boardSize - 1)
                    {
                        throw new ArgumentException("I think the player is dead?");
                    }
                    break;
                case 3:
                    // onward is the tile to the left
                    next_x -= 1;
                    if (next_x < 0)
                    {
                        throw new ArgumentException("I think the player is dead?");
                    }
                    break;
                default:
                    throw new ArgumentException("Illegal onward value", "_onward");
            }
            return (next_x, next_y);
        }

    }
}
