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
        float _heuristic;
        long _evals;

        public Move(IPiece piece, Position from, Position to, float heuristic = 0, long evals = 0)
        {
            _piece = piece;
            _from = from;
            _to = to;
            _heuristic = heuristic;
            _evals = evals;
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

        public float Heuristic
        {
            get
            {
                return _heuristic;
            }
            set
            {
                _heuristic = value;
            }
        }
    }
}
