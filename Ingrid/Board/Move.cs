using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ingrid.Board
{
    class Move
    {
        IPiece _piece;
        Position _from;
        Position _to;

        public Move(IPiece piece, Position from, Position to)
        {
            _piece = piece;
            _from = from;
            _to = to;
        }

        public IPiece Piece
        {
            get
            {
                return _piece;
            }
        }

        public Position From
        {
            get
            {
                return _from;
            }
        }

        public Position To
        {
            get
            {
                return _to;
            }
        }
    }
}
