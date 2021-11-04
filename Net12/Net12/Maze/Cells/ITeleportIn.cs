namespace Net12.Maze
{
    public interface ITeleportIn : IBaseCell
    {
     
        TeleportOut TeleportExit { get; set; }

    }
}