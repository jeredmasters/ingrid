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
        public static Move GetBestMove(GameState state, Team forPlayer, int depth = 2)
        {
            depth--;
            int bestHeuristic = 0;
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
                            newstate.MovePiece(piece, p, m);
                            if (depth > 0)
                            {
                                var otherMove = GetBestMove(newstate, OtherPlayer(forPlayer), depth);
                                newstate.MovePiece(otherMove.Piece, otherMove.From, otherMove.To);
                            }
                            int h = Heuristic.GetHeuristic(newstate, forPlayer);
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
