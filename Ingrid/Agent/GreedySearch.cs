﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ingrid.Board;

namespace Ingrid.Agent
{
    class GreedySearch
    {
        public static Move GetBestMove(GameState state, Team forPlayer)
        {
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
    }
}
