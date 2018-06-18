using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ingrid.Board
{
    [Serializable]
    class Piece
    {
        protected Team _team;
        public Piece(Team team)
        {
            _team = team;
        }

        public Team Team()
        {
            return _team;
        }

        public virtual bool CanMove(Position from, Position to, GameState state)
        {
            return true;
        }

        public IEnumerable<Position> AllowedMoves(Position from, GameState state)
        {
            for(int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    var to = new Position(x, y);
                    if (CanMove(from, to, state))
                    {
                        yield return to;
                    }
                }
            }
        }
        protected IEnumerable<int> range(int from, int to)
        {
            if (from < to)
            {
                for (int i = from + 1; i < to; i++)
                {
                    yield return i;
                }
            }
            for (int i = from - 1; i > to; i--)
            {
                yield return i;
            }
        }
        protected bool ClearOrTake(Position p, GameState state)
        {
            return ClearOrTake(p.X, p.Y, state);
        }
        protected bool ClearOrTake(int x, int y, GameState state)
        {
            var piece = state.At(x, y);
            return (piece == null || piece.Team() != _team);
        }

        public enum Type
        {
            King,
            Queen,
            Rook,
            Knight,
            Bishop,
            Pawn
        }
    }
}
