using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Ingrid.Board;
namespace Ingrid
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GameState _gameState;
        Rectangle[,] _imageSquares;
        Rectangle[,] _colorSquares;


        public MainWindow()
        {
            InitializeComponent();
            _gameState = new GameState();
            drawSquares();
        }

        private Brush getSquareFill(int x, int y)
        {
            bool dark = (x % 2 == 1 || y % 2 == 1) && ((x + y) % 2 == 1);
            return new SolidColorBrush(dark ? Color.FromRgb(180, 180, 180) : Color.FromRgb(220, 220, 220));
        }

        public void drawSquares()
        {
            _imageSquares = new Rectangle[8, 8];
            _colorSquares = new Rectangle[8, 8];
            int width = 60;
            int height = 60;
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    // Create the rectangle
                    
                    Rectangle colorRec = new Rectangle()
                    {
                        Width = width,
                        Height = height,
                        Fill = getSquareFill(x, y),
                    };
                    Rectangle imageRec = new Rectangle()
                    {
                        Width = width,
                        Height = height,
                        Fill = new ImageBrush(),
                        Stroke = Brushes.Red,
                        StrokeThickness = 2,
                        AllowDrop = true,
                        Tag = new SquareState { Position = new Position(x,y), Piece = null }
                    };
                    imageRec.MouseMove += Rec_MouseMove;
                    imageRec.MouseDown += Rec_MouseDown;
                    imageRec.Drop += Rec_Drop;
                    Grid.SetColumn(colorRec, x);
                    Grid.SetRow(colorRec, y);
                    gridBoard.Children.Add(colorRec);
                    Grid.SetColumn(imageRec, x);
                    Grid.SetRow(imageRec, y);
                    gridBoard.Children.Add(imageRec);
                    _imageSquares[x, y] = imageRec;
                    _colorSquares[x, y] = colorRec;
                }
            }
        }

        private void Rec_Drop(object sender, DragEventArgs e)
        {
            if (_draggingFrom.HasValue && _draggingFrom.Value.Piece != null)
            {
                Rectangle rec = (Rectangle)sender;
                SquareState dropState = (SquareState)rec.Tag;
                SquareState dragState = (SquareState)_draggingFrom;
                _gameState.MovePiece(dragState.Piece, dragState.Position, dropState.Position);
                render();
                ShowAllowedMoves();
            }
        }

        private void Rec_MouseMove(object sender, MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Rectangle rec = (Rectangle)sender;
                SquareState state = (SquareState)rec.Tag;
                if (state.Piece != null && state.Piece.Team() == _gameState.PlayerTurn)
                {
                    // Package the data.
                    DataObject data = new DataObject();
                    data.SetData(rec.Tag);

                    // Inititate the drag-and-drop operation.
                    DragDrop.DoDragDrop(this, data, DragDropEffects.Move);
                }
            }
        }
        SquareState? _draggingFrom = null;
        private void Rec_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Rectangle rec = (Rectangle)sender;            
            _draggingFrom = (SquareState)rec.Tag;
            if (_draggingFrom.HasValue && _draggingFrom.Value.Piece != null && _draggingFrom.Value.Piece.Team() == _gameState.PlayerTurn)
            {
                var allowedMoves = _draggingFrom.Value.Piece.AllowedMoves(_draggingFrom.Value.Position, _gameState);
                ShowAllowedMoves(allowedMoves);
            }
        }


        private void btnNewGame_Click(object sender, RoutedEventArgs e)
        {
            _gameState.Initialize();
            render();
        }

        private void render()
        {
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    IPiece piece = _gameState.At(x, y);
                    Rectangle square = _imageSquares[x, y];
                    SquareState currentState = (SquareState)square.Tag;
                    if (currentState.Piece != piece) {
                        if (piece != null)
                        {
                            string path = "images/" + piece.ImageFile();
                            Uri imageUri = new Uri(BaseUriHelper.GetBaseUri(this), path);
                            BitmapImage image = new BitmapImage(imageUri);

                            currentState.Piece = piece;
                            square.Tag = currentState;
                            square.Fill = new ImageBrush(image);                            
                        }
                        else
                        {
                            currentState.Piece = null;
                            square.Tag = currentState;
                            square.Fill = new ImageBrush();
                        }
                    }
                }
            }
            lblPlayerTurn.Content = _gameState.PlayerTurn.ToString();
            //lblWhiteHeuristic.Content = Agent.Heuristic.GetHeuristic(_gameState, Team.White);

            if (_gameState.PlayerTurn == Team.Black)
            {
                TakeAiTurn();
            }
        }
        private void TakeAiTurn()
        {
            var bestMove = Agent.Heuristic.GetBestMove(_gameState, Team.Black);
            _gameState.MovePiece(bestMove.Piece, bestMove.From, bestMove.To);
            render();
        }
        private void ShowAllowedMoves(IEnumerable<Position> positions)
        {
            ShowAllowedMoves();
            foreach (var p in positions)
            {
                Rectangle square = _colorSquares[p.X, p.Y];
                square.Fill = new SolidColorBrush(Color.FromRgb(140, 140, 255));
            }
        }
        private void ShowAllowedMoves()
        {
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    Rectangle square = _colorSquares[x, y];
                    square.Fill = getSquareFill(x, y);
                }
            }
        }       
    }
}
