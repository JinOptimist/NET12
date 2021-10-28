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
        public List<BaseCell> PreviousCell { get; set; } = new List<BaseCell>();

        int prevCount = 0;
        public override void Step()
        {
            int count = 0;
            

            if (Maze[X + 1, Y] is Wall || X + 1 > Maze.Width - 1)
            {
                count++;
            }
            if (Maze[X - 1, Y] is Wall || X - 1 < 0)
            {
                count++;
            }
            if (Maze[X, Y + 1] is Wall || Y + 1 > Maze.Height - 1)
            {
                count++;
            }
            if (Maze[X, Y - 1] is Wall || Y - 1 < 0)
            {
                count++;
            }

            if (count >= 3)
            {
                DeadEnd.Add(Maze[X, Y]);
            }



            if (Maze.Hero.X < X)
            {
                if (X - 1 >= 0 && Maze[X - 1, Y].TryToStep() == true && !(DeadEnd.Where(cell => cell.X == X - 1 && cell.Y == Y).Any())
                                                                     && !(PreviousCell.Where(cell => cell.X == X - 1 && cell.Y == Y).Any()))
                {
                    X--;
                    PreviousCell.Add(Maze[X + 1, Y]);

                    return;
                }
                else
                {
                    if (Y + 1 <= Maze.Height - 1 && Maze[X, Y + 1].TryToStep() == true && Maze.Hero.Y >= Y && !(DeadEnd.Where(cell => cell.X == X && cell.Y == Y + 1).Any())
                                                                                                          && !(PreviousCell.Where(cell => cell.X == X && cell.Y == Y + 1).Any()))
                    {
                        Y++;
                        PreviousCell.Add(Maze[X, Y - 1]);

                        return;
                    }

                    if (Y - 1 >= 0 && Maze[X, Y - 1].TryToStep() == true && Maze.Hero.Y <= Y && !(DeadEnd.Where(cell => cell.X == X && cell.Y == Y - 1).Any())
                                                                                             && !(PreviousCell.Where(cell => cell.X == X && cell.Y == Y - 1).Any()))
                    {
                        Y--;
                        PreviousCell.Add(Maze[X, Y + 1]);

                        return;
                    }

                    if (X + 1 <= Maze.Width - 1 && Maze[X + 1, Y].TryToStep() == true && !(DeadEnd.Where(cell => cell.X == X + 1 && cell.Y == Y).Any())
                                                                                     && !(PreviousCell.Where(cell => cell.X == X + 1 && cell.Y == Y).Any()))
                    {
                        X++;
                        PreviousCell.Add(Maze[X - 1, Y]);

                        return;
                    }
                }

            }

            if (Maze.Hero.X > X)
            {
                if (X + 1 <= Maze.Width - 1 && Maze[X + 1, Y].TryToStep() == true && !(DeadEnd.Where(cell => cell.X == X + 1 && cell.Y == Y).Any())
                                                                                  && !(PreviousCell.Where(cell => cell.X == X + 1 && cell.Y == Y).Any()))
                {
                    X++;
                    PreviousCell.Add(Maze[X - 1, Y]);

                    return;
                }
                else
                {
                    if (Y + 1 <= Maze.Height - 1 && Maze[X, Y + 1].TryToStep() == true && Maze.Hero.Y >= Y && !(DeadEnd.Where(cell => cell.X == X && cell.Y == Y + 1).Any())
                                                                                                           && !(PreviousCell.Where(cell => cell.X == X && cell.Y == Y + 1).Any()))
                    {
                        Y++;
                        PreviousCell.Add(Maze[X, Y - 1]);

                        return;
                    }

                    if (Y - 1 >= 0 && Maze[X, Y - 1].TryToStep() == true && Maze.Hero.Y <= Y && !(DeadEnd.Where(cell => cell.X == X && cell.Y == Y - 1).Any())
                                                                                             && !(PreviousCell.Where(cell => cell.X == X && cell.Y == Y - 1).Any()))
                    {
                        Y--;
                        PreviousCell.Add(Maze[X, Y + 1]);

                        return;
                    }

                    if (X - 1 >= 0 && Maze[X - 1, Y].TryToStep() == true && !(DeadEnd.Where(cell => cell.X == X - 1 && cell.Y == Y).Any())
                                                                         && !(PreviousCell.Where(cell => cell.X == X - 1 && cell.Y == Y).Any()))
                    {
                        X--;
                        PreviousCell.Add(Maze[X + 1, Y]);

                        return;
                    }
                }

            }

            if (Maze.Hero.Y < Y)
            {
                if (Y - 1 >= 0 && Maze[X, Y - 1].TryToStep() == true && !(DeadEnd.Where(cell => cell.X == X && cell.Y == Y - 1).Any())
                                                                     && !(PreviousCell.Where(cell => cell.X == X && cell.Y == Y - 1).Any()))
                {
                    Y--;
                    PreviousCell.Add(Maze[X, Y + 1]);

                    return;
                }
                else
                {
                    if (X + 1 <= Maze.Width - 1 && Maze[X + 1, Y].TryToStep() == true && Maze.Hero.X >= X && !(DeadEnd.Where(cell => cell.X == X + 1 && cell.Y == Y).Any())
                                                                                                          && !(PreviousCell.Where(cell => cell.X == X + 1 && cell.Y == Y).Any()))
                    {
                        X++;
                        PreviousCell.Add(Maze[X - 1, Y]);

                        return;
                    }

                    if (X - 1 >= 0 && Maze[X - 1, Y].TryToStep() == true && Maze.Hero.X <= X && !(DeadEnd.Where(cell => cell.X == X - 1 && cell.Y == Y).Any())
                                                                                             && !(PreviousCell.Where(cell => cell.X == X - 1 && cell.Y == Y).Any()))
                    {
                        X--;
                        PreviousCell.Add(Maze[X + 1, Y]);

                        return;
                    }

                    if (Y + 1 <= Maze.Height - 1 && Maze[X, Y + 1].TryToStep() == true && !(DeadEnd.Where(cell => cell.X == X && cell.Y == Y + 1).Any())
                                                                                       && !(PreviousCell.Where(cell => cell.X == X && cell.Y == Y + 1).Any()))
                    {
                        Y++;
                        PreviousCell.Add(Maze[X, Y - 1]);


                        return;
                    }
                }

            }

            if (Maze.Hero.Y > Y)
            {
                if (Y + 1 <= Maze.Height - 1 && Maze[X, Y + 1].TryToStep() == true && !(DeadEnd.Where(cell => cell.X == X && cell.Y == Y + 1).Any())
                                                                                   && !(PreviousCell.Where(cell => cell.X == X && cell.Y == Y + 1).Any()))
                {
                    Y++;
                    PreviousCell.Add(Maze[X, Y - 1]);


                    return;
                }
                else
                {
                    if (X + 1 <= Maze.Width - 1 && Maze[X + 1, Y].TryToStep() == true && Maze.Hero.X >= X && !(DeadEnd.Where(cell => cell.X == X + 1 && cell.Y == Y).Any())
                                                                                                          && !(PreviousCell.Where(cell => cell.X == X + 1 && cell.Y == Y).Any()))
                    {
                        X++;
                        PreviousCell.Add(Maze[X - 1, Y]);


                        return;
                    }

                    if (X - 1 >= 0 && Maze[X - 1, Y].TryToStep() == true && Maze.Hero.X <= X && !(DeadEnd.Where(cell => cell.X == X - 1 && cell.Y == Y).Any())
                                                                                             && !(PreviousCell.Where(cell => cell.X == X - 1 && cell.Y == Y).Any()))
                    {
                        X--;
                        PreviousCell.Add(Maze[X + 1, Y]);


                        return;
                    }

                    if (Y - 1 >= 0 && Maze[X, Y - 1].TryToStep() == true && !(DeadEnd.Where(cell => cell.X == X && cell.Y == Y - 1).Any())
                                                                         && !(PreviousCell.Where(cell => cell.X == X && cell.Y == Y - 1).Any()))
                    {
                        Y--;
                        PreviousCell.Add(Maze[X, Y + 1]);


                        return;
                    }
                }

            }
            prevCount++;

            if (DeadEnd.Where(cell => cell.X == X && cell.Y == Y).Any())
            {
                PreviousCell.Clear();
            }

            if (prevCount > 1)
            {
                PreviousCell.Clear();
                prevCount = 0;
            }

            return;
        }

        public override bool TryToStep()
        {
            throw new NotImplementedException();
        }
    }
}
