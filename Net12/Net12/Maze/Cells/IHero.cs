namespace Net12.Maze.Cells
{
    public interface IHero : IBaseCell
    {
        int CurrentFatigue { get; set; }
        int Hp { get; set; }
        int Max_hp { get; set; }
        int MaxFatigue { get; set; }
        int Money { get; set; }
    }
}