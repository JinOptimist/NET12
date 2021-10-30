using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net12.Maze.Cells.Enemies
{
    class AgressiveTroll : BaseEnemy
    {
        public AgressiveTroll(int x, int y, MazeLevel maze) : base(x, y, maze) { }
        public List<BaseCell> DeadEnd { get; set; } = new List<BaseCell>();
        public BaseCell PreviousCell { get; set; }
        public int stepCount { get; set; } = 0;

        public override void Step()
        {
            int count = 0;

            if (Maze[X + 1, Y] is Wall || X + 1 == Maze.Width)
            {
                count++;
            }
            if (Maze[X - 1, Y] is Wall || X - 1 == -1)
            {
                count++;
            }
            if (Maze[X, Y + 1] is Wall || Y + 1 == Maze.Height)
            {
                count++;
            }
            if (Maze[X, Y - 1] is Wall || Y - 1 == -1)
            {
                count++;
            }

            if (count >= 3)
            {
                DeadEnd.Add(Maze[X, Y]);
            }



            if (Maze.Hero.X < X)
            {
                if (X - 1 > -1
                    && Maze[X - 1, Y].TryToStep()
                    && !(DeadEnd.Where(cell => cell.X == X - 1 && cell.Y == Y).Any())
                    && Maze[X - 1, Y] != PreviousCell
                   )
                {
                    X--;
                    PreviousCell = Maze[X + 1, Y];

                    return;
                }
                else
                {
                    if (Y + 1 < Maze.Height
                        && Maze[X, Y + 1].TryToStep()
                        && Maze.Hero.Y >= Y
                        && !(DeadEnd.Where(cell => cell.X == X && cell.Y == Y + 1).Any())
                        && Maze[X, Y + 1] != PreviousCell
                       )
                    {
                        Y++;
                        PreviousCell = Maze[X, Y - 1];

                        return;
                    }

                    if (Y - 1 > -1
                        && Maze[X, Y - 1].TryToStep()
                        && Maze.Hero.Y <= Y
                        && !(DeadEnd.Where(cell => cell.X == X && cell.Y == Y - 1).Any())
                        && Maze[X, Y - 1] != PreviousCell
                       )
                    {
                        Y--;
                        PreviousCell = Maze[X, Y + 1];

                        return;
                    }

                    if (X + 1 < Maze.Width
                        && Maze[X + 1, Y].TryToStep()
                        && !(DeadEnd.Where(cell => cell.X == X + 1 && cell.Y == Y).Any())
                        && Maze[X + 1, Y] != PreviousCell
                        )

                    {
                        X++;
                        PreviousCell = Maze[X - 1, Y];

                        return;
                    }
                }

            }

            if (Maze.Hero.X > X)
            {
                if (X + 1 < Maze.Width
                    && Maze[X + 1, Y].TryToStep()
                    && !(DeadEnd.Where(cell => cell.X == X + 1 && cell.Y == Y).Any())
                    && Maze[X + 1, Y] != PreviousCell
                    )
                {
                    X++;
                    PreviousCell = Maze[X - 1, Y];

                    return;
                }
                else
                {
                    if (Y + 1 < Maze.Height
                        && Maze[X, Y + 1].TryToStep()
                        && Maze.Hero.Y >= Y
                        && !(DeadEnd.Where(cell => cell.X == X && cell.Y == Y + 1).Any())
                        && Maze[X, Y + 1] != PreviousCell
                        )
                    {
                        Y++;
                        PreviousCell = Maze[X, Y - 1];

                        return;
                    }

                    if (Y - 1 > -1
                        && Maze[X, Y - 1].TryToStep()
                        && Maze.Hero.Y <= Y
                        && !(DeadEnd.Where(cell => cell.X == X && cell.Y == Y - 1).Any())
                        && Maze[X, Y - 1] != PreviousCell
                        )
                    {
                        Y--;
                        PreviousCell = Maze[X, Y + 1];

                        return;
                    }

                    if (X - 1 > -1
                        && Maze[X - 1, Y].TryToStep()
                        && !(DeadEnd.Where(cell => cell.X == X - 1 && cell.Y == Y).Any())
                        && Maze[X - 1, Y] != PreviousCell
                        )
                    {
                        X--;
                        PreviousCell = Maze[X + 1, Y];

                        return;
                    }
                }

            }

            if (Maze.Hero.Y < Y)
            {
                if (Y - 1 > -1
                    && Maze[X, Y - 1].TryToStep()
                    && !(DeadEnd.Where(cell => cell.X == X && cell.Y == Y - 1).Any())
                    && Maze[X, Y - 1] != PreviousCell
                    )
                {
                    Y--;
                    PreviousCell = Maze[X, Y + 1];

                    return;
                }
                else
                {
                    if (X + 1 < Maze.Width
                        && Maze[X + 1, Y].TryToStep()
                        && Maze.Hero.X >= X
                        && !(DeadEnd.Where(cell => cell.X == X + 1 && cell.Y == Y).Any())
                        && Maze[X + 1, Y] != PreviousCell
                        )
                    {
                        X++;
                        PreviousCell = Maze[X - 1, Y];

                        return;
                    }

                    if (X - 1 > -1
                        && Maze[X - 1, Y].TryToStep()
                        && Maze.Hero.X <= X
                        && !(DeadEnd.Where(cell => cell.X == X - 1 && cell.Y == Y).Any())
                        && Maze[X - 1, Y] != PreviousCell
                        )
                    {
                        X--;
                        PreviousCell = Maze[X + 1, Y];

                        return;
                    }

                    if (Y + 1 < Maze.Height
                        && Maze[X, Y + 1].TryToStep()
                        && !(DeadEnd.Where(cell => cell.X == X && cell.Y == Y + 1).Any())
                        && Maze[X, Y + 1] != PreviousCell
                        )
                    {
                        Y++;
                        PreviousCell = Maze[X, Y - 1];

                        return;
                    }
                }

            }

            if (Maze.Hero.Y > Y)
            {
                if (Y + 1 < Maze.Height
                    && Maze[X, Y + 1].TryToStep()
                    && !(DeadEnd.Where(cell => cell.X == X && cell.Y == Y + 1).Any())
                    && Maze[X, Y + 1] != PreviousCell
                    )
                {
                    Y++;
                    PreviousCell = Maze[X, Y - 1];

                    return;
                }
                else
                {
                    if (X + 1 < Maze.Width
                        && Maze[X + 1, Y].TryToStep()
                        && Maze.Hero.X >= X
                        && !(DeadEnd.Where(cell => cell.X == X + 1 && cell.Y == Y).Any())
                        && Maze[X + 1, Y] != PreviousCell
                        )
                    {
                        X++;
                        PreviousCell = Maze[X - 1, Y];

                        return;
                    }

                    if (X - 1 > -1
                        && Maze[X - 1, Y].TryToStep()
                        && Maze.Hero.X <= X
                        && !(DeadEnd.Where(cell => cell.X == X - 1 && cell.Y == Y).Any())
                        && Maze[X - 1, Y] != PreviousCell
                        )
                    {
                        X--;
                        PreviousCell = Maze[X + 1, Y];


                        return;
                    }

                    if (Y - 1 > -1
                        && Maze[X, Y - 1].TryToStep()
                        && !(DeadEnd.Where(cell => cell.X == X && cell.Y == Y - 1).Any())
                        && Maze[X, Y - 1] != PreviousCell
                        )
                    {
                        Y--;
                        PreviousCell = Maze[X, Y + 1];

                        return;
                    }
                }

            }



            stepCount++;
            if (stepCount > 1)
            {
                PreviousCell = null;
                stepCount = 0;
            }


            if (DeadEnd.Where(cell => cell.X == X && cell.Y == Y).Any())
            {
                PreviousCell = null;
            }

            return;
        }

        public override bool TryToStep()
        {
            return false;
        }
    }
}
