﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net12.Maze.Cells.Enemies
{
    public class Walker : BaseEnemy
    {
  
        public Direction _rotation  { get; set; } = Direction.Up;
        private int _leftwallX;
        private int _leftwallY;
        public Walker(int x, int y, IMazeLevel maze) : base(x, y, maze)
        {
            _leftwallX = X - 1;
            _leftwallY = Y;
        }

        public override void Step()
        {
            //var poses = Maze.Cells.Where(cell => Math.Abs(this.X - cell.X) == 1 && this.Y == cell.Y || Math.Abs(this.Y - cell.Y) == 1 && this.X == cell.X).OfType<Wall>().ToList();
            var no_wall =
                !(_leftwallX < 0
                    || _leftwallX >= Maze.Width
                    || _leftwallY < 0
                    || _leftwallY >= Maze.Height)
                && !(Maze[_leftwallX, _leftwallY] is Wall);

            switch (_rotation)
            {
                case Direction.Up:
                    RotUp(no_wall);
                    break;
                     
                case Direction.Down:
                    RotDown(no_wall);
                    break;
                
                case Direction.Left:
                    RotLeft(no_wall);
                    break;
                
                case Direction.Right:
                    RotRight(no_wall);
                    break;
            }

           
        }

        private void Hit()
        {
            if (X == Maze.Hero.X && Y == Maze.Hero.Y)
            {
                Maze.Hero.Hp--;
            }
        }
        private void RotUp(bool noWall)
        {
                if (!(Maze[X, Y - 1] is Wall) && (Maze[_leftwallX, _leftwallY] is Wall || noWall == false))
                {
                    Y--;
                    _leftwallX = X - 1;
                    _leftwallY = Y;

                }
                else if (noWall && !(Maze[X - 1, Y] is Wall))
                {

                    X--;
                    _leftwallX = X;
                    _leftwallY = Y + 1;
                    _rotation = Direction.Left;

                }
                else if (noWall == false && !(Maze[X + 1, Y] is Wall))
                {

                    X++;
                    _leftwallX = X;
                    _leftwallY = Y - 1;
                    _rotation = Direction.Right;

                }
                else
                {
                    _rotation = Direction.Left;
                    Step();
                }
            
        }
        private void RotLeft(bool noWall)
        {
              
            
                if (!(Maze[X - 1, Y] is Wall) && (Maze[_leftwallX, _leftwallY] is Wall || noWall == false))
                {
                    X--;
                    _leftwallX = X;
                    _leftwallY = Y + 1;

                }
                else if (!(noWall && Maze[X, Y + 1] is Wall))
                {

                    Y++;
                    _leftwallX = X + 1;
                    _leftwallY = Y;
                    _rotation = Direction.Down;

                }
                else if (noWall == false && !(Maze[X, Y - 1] is Wall))
                {

                    Y--;
                    _leftwallX = X - 1;
                    _leftwallY = Y;
                    _rotation = Direction.Up;

                }
                else
                {
                    _rotation = Direction.Down;
                    Step();
                }
            
        }
        private void RotDown(bool noWall)
        {
          
                if (!(Maze[X, Y + 1] is Wall) && (Maze[_leftwallX, _leftwallY] is Wall || noWall == false))
                {
                    Y++;
                    _leftwallX = X + 1;
                    _leftwallY = Y;

                }
                else if (noWall && !(Maze[X + 1, Y] is Wall))
                {

                    X++;
                    _leftwallX = X;
                    _leftwallY = Y - 1;
                    _rotation = Direction.Right;

                }
                else if (noWall == false && !(Maze[X - 1, Y] is Wall))
                {

                    X--;
                    _leftwallX = X;
                    _leftwallY = Y + 1;
                    _rotation = Direction.Left;

                }
                else
                {
                    _rotation = Direction.Right;
                    Step();
                }
            
        }    
        private void RotRight(bool noWall)
        {
            
                if (!(Maze[X + 1, Y] is Wall) && (Maze[_leftwallX, _leftwallY] is Wall || noWall == false))
                {
                    X++;
                    _leftwallX = X;
                    _leftwallY = Y - 1;

                }
                else if (!(noWall && Maze[X, Y - 1] is Wall))
                {

                    Y--;
                    _leftwallX = X - 1;
                    _leftwallY = Y;
                    _rotation = Direction.Up;

                }
                else if (noWall == false && !(Maze[X, Y + 1] is Wall))
                {

                    Y++;
                    _leftwallX = X + 1;
                    _leftwallY = Y;
                    _rotation = Direction.Up;

                }
                else
                {
                    _rotation = Direction.Up;
                    Step();
                }
            
        }

        public override bool TryToStep()
        {
            Hit();
            return true;
        }

    }
}
