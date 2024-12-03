using Raylib_cs;

class Audio
{
    public static Sound select;

    public static void loadsfxresources() {
        select = Raylib.LoadSound(Directory.GetCurrentDirectory() + "\\assets\\select.wav");
        return;
    }
}