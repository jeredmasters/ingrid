using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Ingrid.Board
{
    class GameState
    {
        List<IPiece> _takenPieces;
        SquareState[,] _board;
        Team _playerTurn;
        public GameState()
        {
            _board = new SquareState[8, 8];
            _playerTurn = Team.White;
            _takenPieces = new List<IPiece>();
        }

        public GameState(
            SquareState[,] board,
            List<IPiece> takenPieces,
            Team playerTurn)
        {
            _board = board;
            _playerTurn = playerTurn;
            _takenPieces = takenPieces;
        }

        public IPiece At(int x, int y)
        {
            return _board[x, y].Piece;
        }
        public IPiece At(Position p)
        {
            return At(p.X, p.Y);
        }
        public Team PlayerTurn
        {
            get
            {
                return _playerTurn;
            }
        }
        public IEnumerable<IPiece> TakenPieces
        {
            get
            {
                return _takenPieces;
            }
        }
        public void Initialize()
        {
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    _board[x, y] = new SquareState { Piece = null };
                }
            }

            Team top = Team.Black;
            Team bottom = Team.White;

            for (int x = 0; x < 8; x++)
            {
                _board[x, 1] = new SquareState { Piece = new Pieces.Pawn(top, Direction.Down) };
            }
            _board[0, 0] = new SquareState { Piece = new Pieces.Rook(top) };
            _board[7, 0] = new SquareState { Piece = new Pieces.Rook(top) };
            _board[1, 0] = new SquareState { Piece = new Pieces.Knight(top) };
            _board[6, 0] = new SquareState { Piece = new Pieces.Knight(top) };
            _board[2, 0] = new SquareState { Piece = new Pieces.Bishop(top) };
            _board[5, 0] = new SquareState { Piece = new Pieces.Bishop(top) };
            _board[3, 0] = new SquareState { Piece = new Pieces.King(top) };
            _board[4, 0] = new SquareState { Piece = new Pieces.Queen(top) };


            for (int x = 0; x < 8; x++)
            {
                _board[x, 6] = new SquareState { Piece = new Pieces.Pawn(bottom, Direction.Up) };
            }
            _board[0, 7] = new SquareState { Piece = new Pieces.Rook(bottom) };
            _board[7, 7] = new SquareState { Piece = new Pieces.Rook(bottom) };
            _board[1, 7] = new SquareState { Piece = new Pieces.Knight(bottom) };
            _board[6, 7] = new SquareState { Piece = new Pieces.Knight(bottom) };
            _board[2, 7] = new SquareState { Piece = new Pieces.Bishop(bottom) };
            _board[5, 7] = new SquareState { Piece = new Pieces.Bishop(bottom) };
            _board[3, 7] = new SquareState { Piece = new Pieces.King(bottom) };
            _board[4, 7] = new SquareState { Piece = new Pieces.Queen(bottom) };
        }
        public void MovePiece(IPiece piece, Position from, Position to)
        {
            if (piece.Team() != _playerTurn)
            {
                return;
            }
            if (At(from) != piece)
            {
                return;
                throw new ArgumentException("That piece isn't there");
            }
            if (!piece.CanMove(from, to, this))
            {
                return;
                throw new ArgumentException("Illegal Move");
            }
            var takePiece = At(to);
            if (takePiece != null)
            {
                _takenPieces.Add(takePiece);
            }

            _board[to.X, to.Y].Piece = _board[from.X, from.Y].Piece;
            _board[from.X, from.Y].Piece = null;


            if (_playerTurn == Team.Black)
            {
                _playerTurn = Team.White;
            }
            else
            {
                _playerTurn = Team.Black;
            }
        }
        public Position Find(IPiece piece)
        {
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    if (piece == At(x, y))
                    {
                        return new Position(x, y);
                    }
                }
            }
            return null;
        }
        public void ForceMovePiece(IPiece piece, Position to)
        {
            Position from = Find(piece);
            if (from == null)
            {
                throw new Exception("Piece not on board");
            }
            
            var takePiece = At(to);
            if (takePiece != null)
            {
                _takenPieces.Add(takePiece);
            }

            _board[to.X, to.Y].Piece = _board[from.X, from.Y].Piece;
            _board[from.X, from.Y].Piece = null;


            if (_playerTurn == Team.Black)
            {
                _playerTurn = Team.White;
            }
            else
            {
                _playerTurn = Team.Black;
            }
        }
        public GameState Clone()
        {
            var newboard = new SquareState[8, 8];
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    newboard[x, y] = new SquareState
                    {
                        Piece = _board[x, y].Piece,
                        Position = _board[x, y].Position,
                    };
                }
            }

            var newTakenPieces = new List<IPiece>();
            newTakenPieces.AddRange(_takenPieces);

            return new GameState(newboard, newTakenPieces, _playerTurn);
        }
    }
}
