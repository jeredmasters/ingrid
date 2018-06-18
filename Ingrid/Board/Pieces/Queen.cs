using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ingrid.Board.Pieces
{
    [Serializable]
    class Queen : Piece, IPiece
    {
        public Queen(Team team) : base(team)
        {

        }
        
        public string ImageFile()
        {
            return _team.ToString().ToLower() + "_queen.png";
        }

        public int Value()
        {
            return 9;
        }

        public Piece.Type Type()
        {
            return Piece.Type.Queen;
        }

        public override bool CanMove(Position from, Position to, GameState state)
        {
            int dX = Math.Abs(from.X - to.X);
            int dY = Math.Abs(from.Y - to.Y);

            if (dX == dY)
            {
                var rX = range(from.X, to.X);
                var rY = range(from.Y, to.Y);
                for (int i = 0; i < rX.Count(); i++)
                {
                    if (state.At(rX.ElementAt(i), rY.ElementAt(i)) != null)
                    {
                        return false;
                    }
                }
                return ClearOrTake(to, state);
            }
            if (dX != 0 && dY != 0)
            {
                return false;
            }
            if (dX == 0)
            {
                var rY = range(from.Y, to.Y);
                foreach (int y in rY)
                {
                    if (state.At(to.X, y) != null)
                    {
                        return false;
                    }
                }
            }
            if (dY == 0)
            {
                var rX = range(from.X, to.X);
                foreach (int x in rX)
                {
                    if (state.At(x, to.Y) != null)
                    {
                        return false;
                    }
                }
            }
            return ClearOrTake(to, state); ;
        }
    }
}
