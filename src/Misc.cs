using System.Numerics;
using Raylib_cs;

class Misc
{
    public static void RenderDebug() {
        if (GameData.Debug) {
            Raylib.DrawText(Raylib.GetFPS() + "FPS",(int)(GameData.Consts.WindowSize.X - Raylib.MeasureText(Raylib.GetFPS() + "FPS",18)),0,18,Color.Red);
        }
        return;
    }

    public static (bool,float) checkcollision(Rectangle objectvar,Rectangle[] objects) {
        foreach (var item in objects)
        {
            Vector2 tmp = Vector2.Subtract(new Vector2(objectvar.X,objectvar.Y),PlayerData.PlayerPosition);
            if (Raylib.CheckCollisionRecs(item,new Rectangle(tmp,new Vector2(objectvar.Width,objectvar.Width)))) {
                return (true,Math.Abs(Vector2.Distance(tmp,new Vector2(item.X,item.Y))));
            }
        }
        return (false,0);
    }
}