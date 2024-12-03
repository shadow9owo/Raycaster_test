
using System.Numerics;

class GameData
{
    public class Consts
    {
        public static Vector2 WindowSize = new Vector2(1000,512);
        public static string ApplicationName() {
            if (!GameData.familyfriendlymode) {

            }else {
                return "raytest";
            }
        }
    }
    public static string GetWindowTitle() 
    {
        return Consts.ApplicationName() + " - demo";
    }

    public static string version = "1.0.0.0";
    public static Types.Stages currentstage;
    public static bool Debug = false;

    public static bool ShouldClose = false;

    public const bool familyfriendlymode = true;
    public static Types.RenderType RenderType;

    public static bool transitioning = false;
    public static bool paused = false;
}
class PlayerData
{
    public static Vector2 PlayerPosition = new Vector2(0,0); 
    public static float CameraRotation = 0;
}