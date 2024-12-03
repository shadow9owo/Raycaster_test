using System.Net.Http.Headers;
using System.Numerics;
using Raylib_cs;
using suzabkktgame;

class Misc
{
    public static void RenderDebug() {
        if (GameData.Debug) {
            Raylib.DrawText(Raylib.GetFPS() + "FPS",(int)(GameData.Consts.WindowSize.X - Raylib.MeasureText(Raylib.GetFPS() + "FPS",18)),0,18,Color.Red);
            Raylib.DrawText(PlayerData.CameraRotation + "CameraAngle",(int)(GameData.Consts.WindowSize.X - Raylib.MeasureText(PlayerData.CameraRotation + "CameraAngle",18)),20,18,Color.Red);
            Raylib.DrawText(Program.colorshort + "BG COLOR",(int)(GameData.Consts.WindowSize.X - Raylib.MeasureText(Program.colorshort + "BG COLOR",18)),40,18,Color.Red);

            if (GameData.currentstage == Types.Stages.game) {
                foreach (var item in Map.GetMap())
                {
                    var obj = new Rectangle(
                        item.X + PlayerData.PlayerPosition.X,
                        item.Y + PlayerData.PlayerPosition.Y,
                        item.Width,
                        item.Height
                    );
                    
                    Raylib.DrawRectangleLinesEx(obj,2,Color.Blue);
                }
            }
        }

        return;
    }

    public static (bool,float) checkcollision(Rectangle objectvar,Rectangle[] objects) {
        foreach (var item in objects)
        {
            Vector2 tmp = Vector2.Subtract(new Vector2(objectvar.X,objectvar.Y),PlayerData.PlayerPosition);
            if (Raylib.CheckCollisionRecs(item,new Rectangle(tmp,new Vector2(objectvar.Width,objectvar.Height)))) {
                return (true,Math.Abs(Vector2.Distance(tmp,new Vector2(item.X - item.Width / 2, item.Y - item.Height / 2))));
            }
        }
        return (false,0);
    }

    public static Color shorttocolor(short value) //stole this but whatever lol
    {
        byte r = (byte)((value >> 8) & 0xFF);
        byte g = (byte)((value >> 4) & 0xFF);
        byte b = (byte)((value) & 0xFF);
        Color a = new Color((int)r,(int)g,(int)b,255);
        return a;
    }
}