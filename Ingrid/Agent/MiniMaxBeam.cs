using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ingrid.Board;

namespace Ingrid.Agent
{
    class MiniMaxBeam
    {
        public static Move GetBestMove(GameState state, Team forPlayer, int k, ref long evals, int depth = 2)
        {
            depth--;

            List<Move> moves = new List<Move>();
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    var p = new Position(x, y);
                    var piece = state.At(p);
                    if (piece != null && piece.Team() == forPlayer)
                    {
                        foreach (var m in piece.AllowedMoves(p, state))
                        {
                            var newstate = state.Clone();
                            newstate.MovePiece(piece, p, m);
                            float h = Heuristic.GetHeuristic(newstate, forPlayer, ref evals);
                            moves.Add(new Move(piece, p, m, h));
                            if (moves.Count > k)
                            {
                                Trim(moves, k);
                            }
                        }
                    }
                }
            }
            if (moves.Count > k)
            {
                Trim(moves, k);
            }
            if (depth > 0)
            {
                foreach (var m in moves)
                {
                    var newstate = state.Clone();
                    newstate.MovePiece(m.Piece, m.From, m.To);
                    var otherMove = GetBestMove(newstate, OtherPlayer(forPlayer), k, ref evals, depth);
                    newstate.MovePiece(otherMove.Piece, otherMove.From, otherMove.To);
                    float h = Heuristic.GetHeuristic(newstate, forPlayer, ref evals);
                    m.Heuristic = h;
                }
            }

            Sort(moves);

            return moves[0];
        }
        private static void Sort(List<Move> moves)
        {
            moves.Sort(delegate (Move a, Move b)
            {
                if (a.Heuristic < b.Heuristic) return 1;
                if (a.Heuristic > b.Heuristic) return -1;
                return 0;
            });
        }
        private static void Trim(List<Move> moves, int k)
        {
            Sort(moves);
            moves.RemoveRange(k, moves.Count - k);
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
