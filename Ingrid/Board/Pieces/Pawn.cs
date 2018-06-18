using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ingrid.Board.Pieces
{
    [Serializable]
    class Pawn : Piece, IPiece
    {
        Direction _direction;
        public Pawn(Team team, Direction direction) : base(team)
        {
            _direction = direction;
        }

        public string ImageFile()
        {
            return _team.ToString().ToLower() + "_pawn.png";
        }

        public int Value()
        {
            return 1;
        }

        public Piece.Type Type()
        {
            return Piece.Type.Pawn;
        }

        public override bool CanMove(Position from, Position to, GameState state)
        {
            if (_direction == Direction.Down && from.Y > to.Y)
            {
                return false;
            }
            if (_direction == Direction.Up && from.Y < to.Y)
            {
                return false;
            }
            int dX = Math.Abs(from.X - to.X);
            int dY = Math.Abs(from.Y - to.Y);

            var piece = state.At(to);
            if (dX == 1 && dY == 1 && piece != null && piece.Team() != _team)
            {
                return true;
            }
            if (dX == 0 && dY == 1 && piece == null)
            {
                return true;
            }
            int startY = (_direction == Direction.Down ? 1 : 6);
            if (dX == 0 && dY == 2 && piece == null && from.Y == startY)
            {
                return true;
            }
            return false;
        }
    }
}
