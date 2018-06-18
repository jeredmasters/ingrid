using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ingrid.Board.Pieces
{
    [Serializable]
    class Knight : Piece, IPiece
    {
        public Knight(Team team) : base(team)
        {

        }

        public string ImageFile()
        {
            return _team.ToString().ToLower() + "_knight.png";
        }

        public int Value()
        {
            return 3;
        }

        public Piece.Type Type()
        {
            return Piece.Type.Knight;
        }

        public override bool CanMove(Position from, Position to, GameState state)
        {
            int dX = Math.Abs(from.X - to.X);
            int dY = Math.Abs(from.Y - to.Y);

            if ((dX == 2 && dY == 1) || (dX == 1 && dY == 2))
            {
                return ClearOrTake(to, state);
            }
            return false;
        }
    }
}
