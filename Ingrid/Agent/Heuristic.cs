using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ingrid.Board;
namespace Ingrid.Agent
{
    class Heuristic
    {
        public static float GetHeuristic(GameState state, Team forPlayer, ref long evals)
        {
            float pointsFor = 0;
            float pointsAgainst = 0;
            for(int x = 0; x < 8; x++)
            {
                for(int y = 0; y < 8; y++)
                {
                    var p = new Position(x, y);
                    var piece = state.At(p);
                    if (piece != null)
                    {
                        var moves = piece.AllowedMoves(p, state);
                        foreach (var m in moves)
                        {
                            evals++;
                            var enemyPiece = state.At(m);
                            if (enemyPiece != null)
                            {
                                if (piece.Team() == forPlayer)
                                {
                                    pointsFor += ((float)enemyPiece.Value()) / 4;
                                }
                                else
                                {
                                    pointsAgainst += ((float)enemyPiece.Value()) / 4;
                                }
                            }
                            if (piece.Team() == forPlayer && piece.Type() != Piece.Type.King && (m.Y == 3 || m.Y ==4))
                            {
                                pointsFor += (float)piece.Value() / 10;
                            }
                        }
                        
                    }                    
                }
            }
            foreach(var piece in state.TakenPieces)
            {
                if (piece.Team() == forPlayer)
                {
                    pointsAgainst += ((float)piece.Value());
                }
                else
                {
                    pointsFor += ((float)piece.Value());
                }
            }
            return (pointsFor - pointsAgainst);
        }
    }
}
