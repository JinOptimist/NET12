using Net12.Maze.Cells;
using Net12.Maze.Cells.Enemies;
using Net12.Maze.Enums;
using System;
using System.Collections.Generic;

namespace Net12.Maze
{
    public interface IMazeLevel
    {
        BaseCell this[int x, int y] { get; set; }
        List<BaseCell> Cells { get; set; }
        List<BaseEnemy> Enemies { get; set; }
        int Height { get; set; }
        IHero Hero { get; set; }
        string Message { get; set; }
        int Width { get; set; }
        bool ExitIsOpen { get; set; }
        MazeStatusEnum MazeStatus { get; set; }
        IBaseCell GetCellOrUnit(int x, int y);
        void HeroStep(Direction direction);
        void ReplaceCell(BaseCell baseCell);
        Action<int> GetCoins { get; set; }
    }
}