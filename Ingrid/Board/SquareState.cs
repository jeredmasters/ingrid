using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ingrid.Board
{
    [Serializable]
    struct SquareState
    {
        public IPiece Piece;
        public Position Position;
    }
}
