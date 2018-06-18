using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ingrid.Board
{
    interface IPiece
    {
        string ImageFile();
        IEnumerable<Position> AllowedMoves(Position from, GameState state);
        bool CanMove(Position from, Position to, GameState state);
        Team Team();
        int Value();
    }
}
