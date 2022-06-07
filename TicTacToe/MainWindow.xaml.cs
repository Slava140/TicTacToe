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
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace TicTacToe
{
    public partial class MainWindow : Window
    {
        private string _currentTheme = "Dark";

        private readonly byte _mode = 3;
        private readonly int _cellSize;
        private readonly double _margin;

        private readonly Game _game;
        private char _currentElement = 'x';

        public MainWindow()
        {
            InitializeComponent();

            Resources = new ResourceDictionary { Source = new Uri($"Themes/{_currentTheme}.xaml", UriKind.Relative) };
            _game = new Game(_mode);
            _cellSize = (int)GameCanvas.Width / _mode;
            _margin = _cellSize * 0.1;

            RefreshField();
        }
        private void ThemeChange(object sender, MouseButtonEventArgs e)
        {
            _currentTheme = _currentTheme == "Light" ? "Dark" : "Light";
            Resources = new ResourceDictionary { Source = new Uri($"Themes/{_currentTheme}.xaml", UriKind.Relative) };

            RefreshField();
        }
        private void GameCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            Point mousePos = e.GetPosition(GameCanvas);
            int mouseX = (int)mousePos.X,
                mouseY = (int)mousePos.Y;

            byte arrayX = (byte)(mouseX / _cellSize),
                 arrayY = (byte)(mouseY / _cellSize);

            RefreshField();
            //if (_game.GetEmptyCells().Contains(arrayX.ToString() + arrayY.ToString()))
            if (_game.GameField[arrayY][arrayX] == '0')
            {
                Canvas elementCanvas = GenerateElement(_currentElement, _cellSize - _margin * 2);
                elementCanvas.Opacity = 0.3;
                Canvas.SetLeft(elementCanvas, (arrayX * _cellSize) + _margin);
                Canvas.SetTop(elementCanvas, (arrayY * _cellSize) + _margin);

                _ = GameCanvas.Children.Add(elementCanvas);
            }
        }
        private void GameCanvas_MouseLeave(object sender, MouseEventArgs e)
        {
            RefreshField();
        }
        private void GameCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point mousePos = e.GetPosition(GameCanvas);
            int mouseX = (int)mousePos.X,
                mouseY = (int)mousePos.Y;

            byte arrayX = (byte)(mouseX / _cellSize),
                 arrayY = (byte)(mouseY / _cellSize);

            if (_game.GameField[arrayY][arrayX] == '0')
            {
                _game.SetValue(_currentElement, arrayX, arrayY);
                _currentElement = _currentElement == 'x' ? 'o' : 'x';
                _game.CheckWinner();
            }
            RefreshField();
            Header.Children.Add(SetElement(_game.winner, 100, 0, 0));
        }
        private void ThemeChangeButton_MouseMove(object sender, MouseEventArgs e)
        {
            label.Content = "Hover";
        }
        private void RefreshField()
        {
            GameCanvas.Children.Clear();
            DrawGrid();

            string[] field = _game.GameField;

            for (byte y = 0; y < _mode; y++)
            {
                for (byte x = 0; x < _mode; x++)
                {
                    GameCanvas.Children.Add(SetElement(field[y][x], _cellSize - _margin * 2, (x * _cellSize) + _margin, (y * _cellSize) + _margin));
                }
            }
        }
        private Canvas SetElement(char element, double size, double x, double y)
        {
            Canvas elementCanvas = GenerateElement(element, size);
            Canvas.SetLeft(elementCanvas, x);
            Canvas.SetTop(elementCanvas, y);

            return elementCanvas;
        }
        private Canvas GenerateElement(char element, double size)
        {
            Canvas elementCanvas = new Canvas { Width = size, Height = size };

            if (element == 'x')
            {
                Line line1 = new Line { X1 = 0, X2 = size, Y1 = 0, Y2 = size, Style = (Style)Resources["CrossLine"] };
                Line line2 = new Line { X1 = size, X2 = 0, Y1 = 0, Y2 = size, Style = (Style)Resources["CrossLine"] };
                elementCanvas.Children.Add(line1);
                elementCanvas.Children.Add(line2);
            }
            else if (element == 'o')
            {
                Ellipse circle = new Ellipse { Width = size, Height = size, Style = (Style)Resources["CircleLine"] };
                elementCanvas.Children.Add(circle);
            }

            return elementCanvas;
        }
        private void DrawGrid()
        {
            for (int i = _cellSize; i < _cellSize * _mode; i += _cellSize)
            {
                Line verticalLine = new Line { X1 = i, X2 = i, Y1 = 0, Y2 = _cellSize * _mode, Name = "GridLine" };
                Line horizontalLine = new Line { X1 = 0, X2 = _cellSize * _mode, Y1 = i, Y2 = i, Name = "GridLine" };
                GameCanvas.Children.Add(verticalLine);
                GameCanvas.Children.Add(horizontalLine);
            }
        }

    }
}