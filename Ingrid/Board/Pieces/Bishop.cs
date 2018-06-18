using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ingrid.Board.Pieces
{
    [Serializable]
    class Bishop : Piece, IPiece
    {
        public Bishop(Team team) : base(team)
        {

        }

        public string ImageFile()
        {
            return _team.ToString().ToLower() + "_bishop.png";
        }

        public int Value()
        {
            return 3;
        }

        public Piece.Type Type()
        {
            return Piece.Type.Bishop;
        }

        public override bool CanMove(Position from, Position to, GameState state)
        {
            int dX = Math.Abs(from.X - to.X);
            int dY = Math.Abs(from.Y - to.Y);

            if(dX == dY)
            {
                var rX = range(from.X, to.X);
                var rY = range(from.Y, to.Y);
                for(int i = 0; i < rX.Count(); i++) { 
                    if (state.At(rX.ElementAt(i), rY.ElementAt(i)) != null)
                    {
                        return false;
                    }                    
                }
                return ClearOrTake(to, state);
            }
            return false;
        }


    }
}
