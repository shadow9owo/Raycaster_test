using Raylib_cs;

class HandleInput
{
    static float angle;
    public static void HandleGameInput() {
        if (Raylib.IsKeyPressed(KeyboardKey.F3)) {
            GameData.Debug = !GameData.Debug;
        }

        switch (GameData.currentstage)
        {
            case Types.Stages.game:

                if (Raylib.IsKeyPressed(KeyboardKey.Escape)) {
                    GameData.paused = !GameData.paused;
                }

                if (!GameData.paused) {
                    if (MathF.Abs(Raylib.GetMouseDelta().X) > 0.1f) {
                        PlayerData.CameraRotation += Raylib.GetMouseDelta().X / Raylib.GetFPS();
                        PlayerData.CameraRotation = CustomMEth.InvClamp(PlayerData.CameraRotation, -180, 180);
                    }
    
                    float angle = PlayerData.CameraRotation * MathF.PI / 180f;
    
                    int step = 8;
    
                    if (Raylib.IsKeyDown(KeyboardKey.W) || Raylib.IsKeyDown(KeyboardKey.Up)) {
                        PlayerData.PlayerPosition.Y += step;
                    }
                    if (Raylib.IsKeyDown(KeyboardKey.S) || Raylib.IsKeyDown(KeyboardKey.Down)) {
                        PlayerData.PlayerPosition.Y -= step;
                    }
                    if (Raylib.IsKeyDown(KeyboardKey.A) || Raylib.IsKeyDown(KeyboardKey.Left)) {
                        PlayerData.PlayerPosition.X += step;
                    }
                    if (Raylib.IsKeyDown(KeyboardKey.D) || Raylib.IsKeyDown(KeyboardKey.Right)) {
                        PlayerData.PlayerPosition.X -= step;
                    }
    
                    if (Raylib.IsKeyPressed(KeyboardKey.Q)) {
                        if (GameData.RenderType == Types.RenderType._3d) {
                            GameData.RenderType = Types.RenderType._2d;
                        }else {
                            GameData.RenderType = Types.RenderType._3d;
                        }
                    }
                }
                break;
            default:
                break;
        }
    }
}