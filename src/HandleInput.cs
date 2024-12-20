using System.Numerics;
using System.Runtime.Intrinsics;
using Raylib_cs;
using suzabkktgame;

class HandleInput
{
    static float angle;
    static float sensitivity = 4;
    public static Types.Stages laststage;
    public static void HandleGameInput() {
        if (Raylib.IsKeyPressed(KeyboardKey.F3)) {
            GameData.Debug = !GameData.Debug;
        }

        switch (GameData.currentstage)
        {
            case Types.Stages.game:

                if (Raylib.IsKeyPressed(KeyboardKey.Escape)) {
                        if (GameData.currentstage == Types.Stages.game && !GameData.transitioning) {
                            GameData.paused = !GameData.paused;
                        if (GameData.paused) {
                            Raylib.ShowCursor();
                        }else {
                            Raylib.HideCursor();
                        }
                    }
                }

                if (!GameData.paused) {

                    PlayerData.CameraRotation += CustomMEth.Clamp10(Raylib.GetMouseDelta().X) * sensitivity;
                    PlayerData.CameraRotation = CustomMEth.InvClamp(PlayerData.CameraRotation, 0, 360);
    
                    float angle = PlayerData.CameraRotation * MathF.PI / 180f;
    
                    int step = 6;

                    Vector2 originalpos = PlayerData.PlayerPosition;
                    
                    if (Raylib.IsKeyDown(KeyboardKey.W) || Raylib.IsKeyDown(KeyboardKey.Up)) {
                        PlayerData.PlayerPosition.X -= MathF.Cos(angle) * step;
                        PlayerData.PlayerPosition.Y -= MathF.Sin(angle) * step;
                    }
                    if (Raylib.IsKeyDown(KeyboardKey.S) || Raylib.IsKeyDown(KeyboardKey.Down)) {
                        PlayerData.PlayerPosition.X += MathF.Cos(angle) * step;
                        PlayerData.PlayerPosition.Y += MathF.Sin(angle) * step;
                    }
                    if (Raylib.IsKeyDown(KeyboardKey.A) || Raylib.IsKeyDown(KeyboardKey.Left)) {
                        PlayerData.PlayerPosition.X -= MathF.Cos(angle- MathF.PI / 2) * step;
                        PlayerData.PlayerPosition.Y -= MathF.Sin(angle - MathF.PI / 2) * step;
                    }
                    if (Raylib.IsKeyDown(KeyboardKey.D) || Raylib.IsKeyDown(KeyboardKey.Right)) {
                        PlayerData.PlayerPosition.X += MathF.Cos(angle - MathF.PI / 2) * step;
                        PlayerData.PlayerPosition.Y += MathF.Sin(angle - MathF.PI / 2) * step;
                    }
                    
                    foreach (var item in Map.GetMap())
                    {
                        var obj = new Rectangle(
                            item.X + PlayerData.PlayerPosition.X,
                            item.Y + PlayerData.PlayerPosition.Y,
                            item.Width,
                            item.Height
                        );

                        Rectangle player = new Rectangle(
                            Program.playerscreenpos.X,
                            Program.playerscreenpos.Y,
                            8,
                            8
                        );

                        if (Raylib.CheckCollisionRecs(player, obj))
                        {
                            PlayerData.PlayerPosition = originalpos;
                            break;
                        }
                    }

                    if (Raylib.IsKeyPressed(KeyboardKey.Q)) {
                        if (GameData.currentstage == Types.Stages.game) {
                            if (GameData.RenderType == Types.RenderType._3d) {
                                GameData.RenderType = Types.RenderType._2d;
                            }else {
                                GameData.RenderType = Types.RenderType._3d;
                            }
                        }
                    }

                    if (Raylib.IsKeyPressed(KeyboardKey.Comma)) {
                        if (GameData.currentstage == Types.Stages.leveleditor) {
                            if (laststage == Types.Stages.game) {
                                Raylib.HideCursor();
                            }
                            GameData.currentstage = laststage;
                        }else {
                            Raylib.ShowCursor();
                            HandleInput.laststage = GameData.currentstage;
                            GameData.currentstage = Types.Stages.leveleditor;
                        }
                    }
                }
                break;
            case Types.Stages.leveleditor:
                   int _step = 6;

                    if (Raylib.IsKeyDown(KeyboardKey.W) || Raylib.IsKeyDown(KeyboardKey.Up)) {
                        PlayerData.PlayerPosition.Y += _step;
                    }
                    if (Raylib.IsKeyDown(KeyboardKey.S) || Raylib.IsKeyDown(KeyboardKey.Down)) {
                        PlayerData.PlayerPosition.Y -= _step;
                    }
                    if (Raylib.IsKeyDown(KeyboardKey.A) || Raylib.IsKeyDown(KeyboardKey.Left)) {
                        PlayerData.PlayerPosition.X += _step;
                    }
                    if (Raylib.IsKeyDown(KeyboardKey.D) || Raylib.IsKeyDown(KeyboardKey.Right)) {
                        PlayerData.PlayerPosition.X -= _step;
                    }
                break;
            default:
                break;
        }
    }
}