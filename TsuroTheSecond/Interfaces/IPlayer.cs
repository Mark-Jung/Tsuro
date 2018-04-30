﻿using System;
using System.Collections.Generic;
namespace TsuroTheSecond
{
    public interface IPlayer
    {
        Tile ChooseTile(List<Tile> hand);
        String GetName();
        void Initialize(string color, List<string> other_colors);
        List<int> PlacePawn(Board board);
        Tile PlayTurn(Board board, List<Tile> hand, int unused); //suitably rotated tile is the output
        void EndGame(Board board, List<string> colors); // stat which player won.
    }
}
