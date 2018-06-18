using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ingrid.Board;

namespace Ingrid.Agent
{
    class MiniMax
    {
        public static Move GetBestMove(GameState state, Team forPlayer, ref long evals, int depth = 2)
        {
            depth--;
            float bestHeuristic = 0;
            Move bestMove = null;
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    var p = new Position(x, y);
                    var piece = state.At(p);
                    if (piece != null && piece.Team() == forPlayer)
                    {
                        var moves = piece.AllowedMoves(p, state);
                        foreach (var m in moves)
                        {
                            var newstate = state.Clone();
                            newstate.ForceMovePiece(piece, m);
                            if (depth > 0)
                            {
                                var otherMove = GetBestMove(newstate, OtherPlayer(forPlayer), ref evals, depth);
                                newstate.MovePiece(otherMove.Piece, otherMove.From, otherMove.To);
                            }
                            float h = Heuristic.GetHeuristic(newstate, forPlayer, ref evals);
                            if (h > bestHeuristic || bestMove == null)
                            {
                                bestHeuristic = h;
                                bestMove = new Move(piece, p, m);
                            }
                        }
                    }
                }
            }
            return bestMove;
        }
        private static Team OtherPlayer(Team player)
        {
            if (player == Team.Black)
            {
                return Team.White;
            }
            return Team.Black;
        }
    }
}
