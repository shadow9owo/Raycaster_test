using System.Numerics;
using Raylib_cs;

class Utils
{
    public static float timerbuffer;
    public static int progress;
    private static Types.Stages varscene;
    public static void CallTransitionToOtherScene(Types.Stages scene) 
    {
        if (!GameData.transitioning) {
            GameData.transitioning = true;
            varscene = scene;
        }
    }
    public static void TransitionToOtherSceneProgress() {
        if (!GameData.transitioning) {
            return;
        }
        if (timerbuffer >= 0.006) {
            progress = progress + 8;
            if (progress > 255) {
                progress = 255;
                GameData.currentstage = varscene;
                progress = 0;
                GameData.transitioning = false;
                varscene = 0;
                GameData.paused = false;
                PlayerData.CameraRotation = 0;
                PlayerData.PlayerPosition = Vector2.Zero;
                Raylib.DrawRectangle(0,0,(int)GameData.Consts.WindowSize.X,(int)GameData.Consts.WindowSize.Y,new Color(0,0,0,255));
            }
            timerbuffer = 0;
        }else {
            timerbuffer = timerbuffer + Raylib.GetFrameTime();
        }
        Raylib.DrawRectangle(0,0,(int)GameData.Consts.WindowSize.X,(int)GameData.Consts.WindowSize.Y,new Color(0,0,0,progress));
    }
}