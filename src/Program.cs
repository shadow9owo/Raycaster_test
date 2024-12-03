using System.Numerics;
using ImGuiNET;
using rlImGui_cs;
using Raylib_cs;
using System;
using Newtonsoft.Json;

namespace suzabkktgame;

class Program
{
    private static Color backgroundcolor;
    public static float colorshort = short.MinValue;
    private static int playersize = 8;
    public static Vector2 playerscreenpos;
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
                }else {
                    Raylib.DrawText("PAUSED",0,0,24,Color.White);
                }
                switch (GameData.RenderType)
                {
                    case Types.RenderType._2d:
                        Raylib.ClearBackground(Misc.shorttocolor((short)colorshort));
                        foreach (var item in Map.GetMap())
                        {
                            var objectobj = item;
                            objectobj.X = objectobj.X + PlayerData.PlayerPosition.X;
                            objectobj.Y = objectobj.Y + PlayerData.PlayerPosition.Y;
                            Raylib.DrawRectangleRec(objectobj, Color.White);
                        }
                    
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
                    
                                if (Misc.checkcollision(new Rectangle(lineEnd, 4, 4), Map.GetMap()).Item1)
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
                        Raylib.ClearBackground(Misc.shorttocolor((short)colorshort));
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
                    
                                if (Misc.checkcollision(new Rectangle(lineEnd, 4, 4), Map.GetMap()).Item1)
                                {
                                    hit = true;

                                    Raylib.DrawRectangle(
                                        (int)(raywallsize * a),
                                        (int)(GameData.Consts.WindowSize.Y / Misc.checkcollision(new Rectangle(lineEnd, 4, 4),Map.GetMap()).Item2),
                                        (int)raywallsize,
                                        (int)(GameData.Consts.WindowSize.Y - (GameData.Consts.WindowSize.Y / Misc.checkcollision(new Rectangle(lineEnd, 4, 4), Map.GetMap()).Item2)),
                                        new Color(
                                            backgroundcolor.R,
                                            (int)Math.Clamp(0 + (255 / (Misc.checkcollision(new Rectangle(lineEnd, 4, 4), Map.GetMap()).Item2)), 0, 255),
                                            backgroundcolor.B,
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
            case Types.Stages.leveleditor:
                Raylib.ClearBackground(Color.Black);
                rlImGui.Begin();
                if (ImGui.Begin("Level Editor"))
                {
                    ImGui.Text("Level -- writter tool");

                    ImGui.BeginChild("Tools && Config", new Vector2(0, 150));
                    ImGui.InputText("map name", ref LevelEditor.name, 32);
                    ImGui.InputInt("X position", ref LevelEditor.X_pos, 0);
                    ImGui.InputInt("Y position", ref LevelEditor.Y_pos, 0);
                    ImGui.InputInt("Width", ref LevelEditor.Width, 0);
                    ImGui.InputInt("Height", ref LevelEditor.Height, 0);
                    ImGui.EndChild();
                
                    // Options for adding/removing map objects
                    ImGui.BeginChild("Options", new Vector2(0, 150));
                
                    if (ImGui.Button("Add to map", new Vector2(128, 32)))
                    {
                        if (LevelEditor.Height != 0 && LevelEditor.Width != 0)
                        {
                            Map.levelmap.Add(new Rectangle(new Vector2(LevelEditor.X_pos, LevelEditor.Y_pos), new Vector2(LevelEditor.Width, LevelEditor.Height)));
                        }
                    }
                
                    ImGui.SameLine();
                
                    if (ImGui.Button("Export", new Vector2(128, 32)))
                    {
                        if (Map.levelmap.Count > 0)
                        {
                            List<Types.Gameobject> gameobjects = new List<Types.Gameobject>();
                            foreach (var item in Map.levelmap)
                            {
                                Types.Gameobject temp = new Types.Gameobject
                                {
                                    Xpos = (int)item.X,
                                    Ypos = (int)item.Y,
                                    Width = (int)item.Width,
                                    Height = (int)item.Height
                                };
                                gameobjects.Add(temp);
                            }
                
                            File.WriteAllText(LevelEditor.name + ".MAP", JsonConvert.SerializeObject(gameobjects));
                        }
                    }
                
                    ImGui.SameLine();
                
                    if (ImGui.Button("Remove last", new Vector2(128, 32)))
                    {
                        if (Map.levelmap.Count > 0)
                        {
                            Map.levelmap.RemoveAt(Map.levelmap.Count - 1);
                        }
                    }
                
                    ImGui.SameLine();
                
                    if (ImGui.Button("List Map Data", new Vector2(128, 32)))
                    {
                        if (File.Exists("tmp.txt")) { File.Delete("tmp.txt"); }
                
                        using (FileStream fs = File.Create("tmp.txt"))
                        {
                            if (Map.levelmap.Count > 0)
                            {
                                foreach (var item in Map.levelmap)
                                {
                                    byte[] data = new System.Text.UTF8Encoding(true).GetBytes("X: " + (int)item.X);
                                    fs.Write(data, 0, data.Length);
                                    data = new System.Text.UTF8Encoding(true).GetBytes(" Y: " + (int)item.Y);
                                    fs.Write(data, 0, data.Length);
                                    data = new System.Text.UTF8Encoding(true).GetBytes(" Width: " + (int)item.Width);
                                    fs.Write(data, 0, data.Length);
                                    data = new System.Text.UTF8Encoding(true).GetBytes(" Height: " + (int)item.Height);
                                    fs.Write(data, 0, data.Length);
                                    data = new System.Text.UTF8Encoding(true).GetBytes("\n");
                                    fs.Write(data, 0, data.Length);
                                }
                            }
                        }
                
                        System.Diagnostics.Process.Start("notepad.exe", "tmp.txt");
                    }

                    if (ImGui.Button("Leave", new Vector2(128, 32)))
                    {
                        if (GameData.currentstage == Types.Stages.leveleditor)
                        {
                            if (HandleInput.laststage == Types.Stages.game)
                            {
                                Raylib.HideCursor();
                            }
                            GameData.currentstage = HandleInput.laststage;
                        }
                        else
                        {
                            Raylib.ShowCursor();
                            HandleInput.laststage = GameData.currentstage;
                            GameData.currentstage = Types.Stages.leveleditor;
                        }
                    }
                
                    ImGui.EndChild();
                }

                ImGui.End();
                rlImGui.End();
                break;
            default:
                break;
        }
    }

    public static void Main()
    {
        Raylib.SetConfigFlags(ConfigFlags.Msaa4xHint | ConfigFlags.VSyncHint);

        Raylib.SetTargetFPS(30);

        Raylib.InitWindow((int)GameData.Consts.WindowSize.X,(int)GameData.Consts.WindowSize.Y, GameData.GetWindowTitle());

        Raylib.SetExitKey(0);

        rlImGui.Setup(true);	

        GameData.RenderType = Types.RenderType._2d;

        while (!Raylib.WindowShouldClose() && !GameData.ShouldClose)
        {
            Raylib.BeginDrawing();

            colorshort = CustomMEth.InvClamp(colorshort+1,short.MinValue,short.MaxValue);

            HandleInput.HandleGameInput();

            GameRenderFrame();

            Utils.TransitionToOtherSceneProgress();

            Misc.RenderDebug();

            Raylib.EndDrawing();
        }

        rlImGui.Shutdown();

        Raylib.ShowCursor();

        Raylib.CloseWindow();
    }
}