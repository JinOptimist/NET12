namespace WebMaze.EfStuff.DbModel
{
    public class GameDevices : BaseModel
    {
        public virtual User Creater { get; set; }

        public string TypeDevice { get; set; }
        public string DeviceName { get; set; }
        public string BrandName { get; set; }
        public string Picture { get; set; }
        public string Desc { get; set; }
    }
}
