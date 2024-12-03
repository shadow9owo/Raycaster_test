class Types
{
    public enum Stages
    {
        mainmenu,
        game,
        leveleditor
    }
    public enum RenderType
    {
        _3d,
        _2d
    }
    public class Gameobject
    {
        public int Xpos { get; set; }
        public int Ypos { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
    }
}