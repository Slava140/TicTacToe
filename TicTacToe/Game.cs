using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace TicTacToe
{
    internal class Game
    {
        private static byte _mode;

        internal char winner;
        internal Game(byte mode = 3)
        {
            _mode = mode >= 3 && mode <= 5 ? mode : throw new Exception("Невозможно создать такое игровое поле");
            GameField = GenerateField();
        }

        private string[] GenerateField()
        {
            string[] result = new string[_mode];
            for (int i = 0; i < _mode; i++)
            {
                result[i] = Repeat('0', _mode);
            }
            return result;
        }
        internal string[] GameField { get; }
        internal void SetValue(char value, byte xPos, byte yPos)
        {
            if ((value == 'x' || value == 'o') && (xPos < _mode && yPos < _mode))
            {
                GameField[yPos] = GameField[yPos].Remove(xPos, 1).Insert(xPos, value.ToString());
            }
        }
        internal void CheckWinner()
        {
            List<char> leftDiagonal = new List<char>();
            List<char> RightDiagonal = new List<char>();

            for (int i = 0; i < _mode; i++)
            {
                if (GameField.All(str => str[i] == 'x')) { winner = 'x'; }
                if (GameField.All(str => str[i] == 'o')) { winner = 'o'; }

                leftDiagonal.Add(GameField[i][i]);
                RightDiagonal.Add(GameField[Math.Abs(i - _mode + 1)][i]);
            }

            if (GameField.Contains(Repeat('x', _mode)) || leftDiagonal.All(c => c == 'x') || RightDiagonal.All(c => c == 'x'))
            {
                winner = 'x';
            }
            if (GameField.Contains(Repeat('o', _mode)) || leftDiagonal.All(c => c == 'o') || RightDiagonal.All(c => c == 'o'))
            {
                winner = 'o';
            }
        }
        private static string Repeat(char value, int count)
        {
            return string.Concat(Enumerable.Repeat(value, count));
        }
    }
}