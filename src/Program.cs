using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using Raylib_cs;

namespace suzabkktgame;

class Program
{
    private static int playersize = 32;
    private static Vector2 playerscreenpos;
    private static Vector2 playerscreenMIN;
    private static Vector2 playerscreenMAX;

    public static void GameRenderFrame() {
        Rectangle mouse = new Rectangle(Raylib.GetMouseX(),Raylib.GetMouseY(),20,20);
        switch (GameData.currentstage)
        {
            case Types.Stages.mainmenu:
                Raylib.ClearBackground(Color.Black);

                //Title

                Raylib.DrawText(GameData.Consts.ApplicationName(),(int)GameData.Consts.WindowSize.X /2 - (int)Raylib.MeasureTextEx(Raylib.GetFontDefault(),GameData.Consts.ApplicationName(),72,1).X / 2,(int)Raylib.MeasureTextEx(Raylib.GetFontDefault(),GameData.Consts.ApplicationName(),72,1).Y,72,Color.White);

                //Play BTN

                Rectangle playbtn = new Rectangle((GameData.Consts.WindowSize.X / 2) - Raylib.MeasureTextEx(Raylib.GetFontDefault(),"Play",36,0).X,(GameData.Consts.WindowSize.Y / 2) - Raylib.MeasureTextEx(Raylib.GetFontDefault(),"Play",36,0).Y,36 * 3,48);

                if (Raylib.CheckCollisionRecs(mouse,playbtn)) {
                    Raylib.DrawRectangleRec(playbtn,Color.LightGray);
                    if (Raylib.IsMouseButtonPressed(MouseButton.Left)) { //button clicked
                        if (!GameData.transitioning) {
                            Utils.CallTransitionToOtherScene(Types.Stages.game);
                            
                            Raylib.HideCursor();
                        }
                    }
                }else {
                    Raylib.DrawRectangleRec(playbtn,Color.RayWhite);
                }
                Raylib.DrawText("Play",(int)playbtn.X + 20,(int)playbtn.Y + 5,36,Color.Black);

                //Quit BTN

                Rectangle quitbtn = new Rectangle((GameData.Consts.WindowSize.X / 2) - Raylib.MeasureTextEx(Raylib.GetFontDefault(),"Play",36,0).X,(GameData.Consts.WindowSize.Y / 2) - Raylib.MeasureTextEx(Raylib.GetFontDefault(),"Play",36,0).Y + 76,36 * 3,48);

                if (Raylib.CheckCollisionRecs(mouse,quitbtn)) {
                    Raylib.DrawRectangleRec(quitbtn,Color.LightGray);
                    if (Raylib.IsMouseButtonPressed(MouseButton.Left)) { //button clicked
                        if (!GameData.transitioning) {
                            GameData.ShouldClose = true;
                        }
                    }
                }else {
                    Raylib.DrawRectangleRec(quitbtn,Color.RayWhite);
                }
                Raylib.DrawText("Quit",(int)quitbtn.X + 20,(int)quitbtn.Y + 5,36,Color.Black);

                break;
            case Types.Stages.game:
                if (!GameData.paused) 
                {
                    Raylib.SetMousePosition((int)GameData.Consts.WindowSize.X / 2,(int)GameData.Consts.WindowSize.Y / 2);
                }
                switch (GameData.RenderType)
                {
                    case Types.RenderType._2d:
                        Raylib.ClearBackground(Color.Black);
                        foreach (var item in Map.Objects)
                        {
                            var objectobj = item;
                            objectobj.X = objectobj.X + PlayerData.PlayerPosition.X;
                            objectobj.Y = objectobj.Y + PlayerData.PlayerPosition.Y;
                            Raylib.DrawRectangleRec(objectobj, Color.White);
                        }
                    
                        Program.playersize = 32;
                        Program.playerscreenpos = new Vector2((int)GameData.Consts.WindowSize.X / 2, (int)GameData.Consts.WindowSize.Y / 2);
                        Program.playerscreenMIN = new Vector2((int)GameData.Consts.WindowSize.X / 2, (int)(GameData.Consts.WindowSize.Y / 2) - Program.playersize / 2);
                        Program.playerscreenMAX = new Vector2((int)GameData.Consts.WindowSize.X / 2, (int)(GameData.Consts.WindowSize.Y / 2) + Program.playersize / 2);
                        Raylib.DrawCircle((int)Program.playerscreenpos.X, (int)Program.playerscreenpos.Y, Program.playersize, Color.White);

                        int numRays = 200;

                        float fov = MathF.PI / 3f;
                        float startAngle = PlayerData.CameraRotation - fov / 2f;
                    
                        for (int i = 0; i < numRays; i++)
                        {
                            float angle = startAngle + (i * fov / numRays);
                    
                            float maxLineLength = 500f;
                            float lineLength = 1;
                            Vector2 lineEnd = Vector2.Zero;
                            bool hit = false;
                    
                            while (!hit && lineLength < maxLineLength)
                            {
                                lineEnd = new Vector2(
                                    Program.playerscreenpos.X + MathF.Cos(angle) * lineLength,
                                    Program.playerscreenpos.Y + MathF.Sin(angle) * lineLength
                                );
                    
                                if (Misc.checkcollision(new Rectangle(lineEnd, 4, 4), Map.Objects).Item1)
                                {
                                    hit = true;
                                    break;
                                }
                                else
                                {
                                    lineLength += 2;
                                }
                            }

                            if (hit)
                            {
                                Raylib.DrawLine(
                                    (int)Program.playerscreenpos.X,
                                    (int)Program.playerscreenpos.Y,
                                    (int)lineEnd.X,
                                    (int)lineEnd.Y,
                                    Color.Gray
                                );
                            }
                            else if ((i == 0 || i == numRays - 1) && GameData.Debug)
                            {
                                lineEnd = new Vector2(
                                    Program.playerscreenpos.X + MathF.Cos(angle) * lineLength,
                                    Program.playerscreenpos.Y + MathF.Sin(angle) * lineLength
                                );
                    
                                Raylib.DrawLine(
                                    (int)Program.playerscreenpos.X,
                                    (int)Program.playerscreenpos.Y,
                                    (int)lineEnd.X,
                                    (int)lineEnd.Y,
                                    Color.Gray
                                );
                            }
                        }
                        break;
                    case Types.RenderType._3d:
                        Raylib.ClearBackground(Color.White);
                        int _numRays = 500;

                        float raywallsize = GameData.Consts.WindowSize.X / _numRays;
                        float _fov = MathF.PI / 3f;
                        float _startAngle = PlayerData.CameraRotation - _fov / 2f;
                    
                        for (int a = 0; a < _numRays; a++)
                        {
                            float angle = _startAngle + (a * _fov / _numRays);
                    
                            float maxLineLength = 500f;
                            float lineLength = 1;
                            Vector2 lineEnd = Vector2.Zero;
                            bool hit = false;
                    
                            while (!hit && lineLength < maxLineLength) //the cam is horrible but like it does the trick i guess?
                            {
                                lineEnd = new Vector2(
                                    Program.playerscreenpos.X + MathF.Cos(angle) * lineLength,
                                    Program.playerscreenpos.Y + MathF.Sin(angle) * lineLength
                                );
                    
                                if (Misc.checkcollision(new Rectangle(lineEnd, 4, 4), Map.Objects).Item1)
                                {
                                    hit = true;

                                    Raylib.DrawRectangle(
                                        (int)(raywallsize * a),
                                        (int)(GameData.Consts.WindowSize.Y / Misc.checkcollision(new Rectangle(lineEnd, 4, 4),Map.Objects).Item2),
                                        (int)raywallsize,
                                        (int)(GameData.Consts.WindowSize.Y - (GameData.Consts.WindowSize.Y / Misc.checkcollision(new Rectangle(lineEnd, 4, 4), Map.Objects).Item2)),
                                        new Color(
                                            (int)Math.Clamp(0 + (255 / (Misc.checkcollision(new Rectangle(lineEnd, 4, 4), Map.Objects).Item2)), 0, 255),
                                            (int)Math.Clamp(0 + (255 / (Misc.checkcollision(new Rectangle(lineEnd, 4, 4), Map.Objects).Item2)), 0, 255),
                                            (int)Math.Clamp(0 + (255 / (Misc.checkcollision(new Rectangle(lineEnd, 4, 4), Map.Objects).Item2)), 0, 255),
                                            255
                                        )
                                    );
                                    break;
                                }
                                else
                                {
                                    lineLength += 2;
                                }
                            }
                        }
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }
    }

    public static void Main()
    {
        Raylib.InitWindow((int)GameData.Consts.WindowSize.X,(int)GameData.Consts.WindowSize.Y, GameData.GetWindowTitle());

        Raylib.SetExitKey(0);

        GameData.RenderType = Types.RenderType._2d;

        while (!Raylib.WindowShouldClose() && !GameData.ShouldClose)
        {
            Raylib.BeginDrawing();

            HandleInput.HandleGameInput();

            GameRenderFrame();

            Utils.TransitionToOtherSceneProgress();

            Misc.RenderDebug();

            Raylib.EndDrawing();
        }

        Raylib.ShowCursor();

        Raylib.CloseWindow();
    }
}