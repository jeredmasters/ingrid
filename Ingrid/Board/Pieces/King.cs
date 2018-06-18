using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ingrid.Board.Pieces
{
    [Serializable]
    class King : Piece, IPiece
    {
        public King(Team team) : base(team)
        {

        }

        public string ImageFile()
        {
            return _team.ToString().ToLower() + "_king.png";
        }

        public int Value()
        {
            return 9999;
        }

        public override bool CanMove(Position from, Position to, GameState state)
        {
            int dX = Math.Abs(from.X - to.X);
            int dY = Math.Abs(from.Y - to.Y);

            if (dX > 1 || dY > 1)
            {
                return false;
            }
            return ClearOrTake(to, state);
        }
    }
}
