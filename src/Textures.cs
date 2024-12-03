using Raylib_cs;

class Textures
{
    public static Texture2D mainmenutexture;

    public static void LoadTextures() {
        mainmenutexture = Raylib.LoadTexture(Directory.GetCurrentDirectory() + "\\assets\\mainmenu.png");
    }
}