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
            _mode = mode >= 3 && mode <= 10 ? mode : throw new Exception("Невозможно создать такое игровое поле");
            GameField = GenerateField();
        }

        private string[] GenerateField()
        {
            string[] result = new string[_mode];
            for (int i = 0; i < _mode; i++)
            {
                result[i] = string.Concat(Enumerable.Repeat('0', _mode));
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
            if (_mode > 5) { CheckLargeField(GameField, _mode); }
            else { CheckSmallField(GameField, _mode); }
        }
        private void CheckSmallField(string[] field, byte mode)
        {
            List<char> leftDiagonal = new List<char>();
            List<char> RightDiagonal = new List<char>();

            for (int i = 0; i < mode; i++)
            {
                if (field.All(str => str[i] == 'x')) { winner = 'x'; }
                if (field.All(str => str[i] == 'o')) { winner = 'o'; }

                leftDiagonal.Add(field[i][i]);
                RightDiagonal.Add(field[Math.Abs(i - mode + 1)][i]);
            }

            if (field.Contains(Repeat('x', mode)) || leftDiagonal.All(c => c == 'x') || RightDiagonal.All(c => c == 'x'))
            {
                winner = 'x';
            }
            if (field.Contains(Repeat('o', mode)) || leftDiagonal.All(c => c == 'o') || RightDiagonal.All(c => c == 'o'))
            {
                winner = 'o';
            }
        }
        private void CheckLargeField(string[] field, byte mode)
        {
            for (int kernelPos = 0; kernelPos < (mode - 4); kernelPos++)
            {
                string[] kernel = new string[5];
                for (int pos = 0; pos < 5; pos++)
                {
                    kernel[pos] = field[kernelPos + pos].Substring(kernelPos, 5);
                }
                CheckSmallField(kernel, 5);
            }
        }

        private static string Repeat(char value, int count)
        {
            return string.Concat(Enumerable.Repeat(value, count));
        }
    }
}
