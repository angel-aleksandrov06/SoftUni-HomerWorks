﻿using System;
using System.Collections.Generic;

namespace DemoTetris
{
    public class TetrisGame : ITetrisGame
    {
        private readonly List<Tetrominoe> TetrisFigures = new List<Tetrominoe>()
            {
                new Tetrominoe(new bool[,] // ----
                {
                    { true, true, true, true }
                }),
                new Tetrominoe(new bool[,] // O
                {
                    { true, true },
                    { true, true }
                }),
                new Tetrominoe(new bool[,] // T
                {
                    { false, true, false },
                    { true, true, true },
                }),
                new Tetrominoe(new bool[,] // S
                {
                    { false, true, true, },
                    { true, true, false, },
                }),
                new Tetrominoe(new bool[,] // Z
                {
                    { true, true, false },
                    { false, true, true },
                }),
                new Tetrominoe(new bool[,] // J
                {
                    { true, false, false },
                    { true, true, true }
                }),
                new Tetrominoe(new bool[,] // L
                {
                    { false, false, true },
                    { true, true, true }
                }),
            };

        private Random random;

        public TetrisGame(int tetrisRows, int tetrisColumns)
        {
            this.Level = 1;
            this.CurrentFigure = null;
            this.CurrentFigureRow = 0;
            this.CurrentFigureCol = 0;
            this.TetrisField = new bool[tetrisRows, tetrisColumns];
            this.TetrisRows = tetrisRows;
            this.TetrisColumns = tetrisColumns;
            this.random = new Random();
            this.NewRandomFugire();
        }


        public int Level { get; private set; }


        public Tetrominoe CurrentFigure { get; set; }

        public int CurrentFigureRow { get; set; }

        public int CurrentFigureCol { get; set; }

        public bool[,] TetrisField { get; private set; }
        public int TetrisRows { get; }
        public int TetrisColumns { get; }

        public void UpdateLevel(int score)
        {
            if (score <= 0)
            {
                this.Level = 1;
                return;
            }

            this.Level = (int)Math.Log10(score) - 1;
            if (this.Level < 1)
            {
                this.Level = 1;
            }

            if (this.Level > 10)
            {
                this.Level = 10;
            }
        }

        public void NewRandomFugire()
        {
            this.CurrentFigure = TetrisFigures[this.random.Next(0, this.TetrisFigures.Count)];
            this.CurrentFigureRow = 0;
            this.CurrentFigureCol = this.TetrisColumns / 2 - this.CurrentFigure.Width / 2 - 1;
        }

        public void AddCurrentFigureToTetrisField()
        {
            for (int row = 0; row < this.CurrentFigure.Width; row++)
            {
                for (int col = 0; col < this.CurrentFigure.Height; col++)
                {
                    if (this.CurrentFigure.Body[row, col])
                    {
                        this.TetrisField[this.CurrentFigureRow + row, this.CurrentFigureCol + col] = true;
                    }
                }
            }
        }

        public int CheckForFullLines() // 0, 1, 2, 3, 4
        {
            int lines = 0;

            for (int row = 0; row < this.TetrisField.GetLength(0); row++)
            {
                bool rowIsFull = true;
                for (int col = 0; col < this.TetrisField.GetLength(1); col++)
                {
                    if (this.TetrisField[row, col] == false)
                    {
                        rowIsFull = false;
                        break;
                    }
                }

                if (rowIsFull)
                {
                    for (int rowToMove = row; rowToMove >= 1; rowToMove--)
                    {
                        for (int col = 0; col < this.TetrisField.GetLength(1); col++)
                        {
                            this.TetrisField[rowToMove, col] = this.TetrisField[rowToMove - 1, col];
                        }
                    }

                    lines++;
                }
            }

            return lines;
        }

        public bool Collision(Tetrominoe figure)
        {
            if (this.CurrentFigureCol > this.TetrisColumns - figure.Height)
            {
                return true;
            }

            if (this.CurrentFigureRow + figure.Width == this.TetrisRows)
            {
                return true;
            }

            for (int row = 0; row < figure.Width; row++)
            {
                for (int col = 0; col < figure.Height; col++)
                {
                    if (figure.Body[row, col] &&
                        this.TetrisField[this.CurrentFigureRow + row + 1, this.CurrentFigureCol + col])
                    {
                        return true;
                    }

                }
            }

            return false;
        }
    }
}
