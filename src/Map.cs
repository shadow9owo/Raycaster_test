using Raylib_cs;
using Newtonsoft.Json;
using System.Numerics;

class Map
{
    static int maploaded = -1;
    static string path;
    static List<Rectangle> mapdata = new List<Rectangle>();
    public static Rectangle[] GetMap() {
        if (GameData.currentstage == Types.Stages.leveleditor) {
            return levelmap.ToArray();
        }

        if (maploaded == -1) {
            foreach (var item in Directory.GetFiles(Directory.GetCurrentDirectory()))
            {
                if (item.Contains(".MAP")) {
                    path = item;
                    maploaded = 0;
                    return LoadMap(item);
                }
            }
            maploaded = 1;
        }else if (maploaded == 1) {
            return Obj1ects;
        }else if (maploaded == 0) {
            return LoadMap(path);
        }else if (maploaded == 2) {
            return mapdata.ToArray();
        }
        foreach (var item in Directory.GetFiles(Directory.GetCurrentDirectory()))
        {
            if (item.Contains(".MAP")) {
                return LoadMap(item);
            }
        }
        return Obj1ects;
    }

    public static Rectangle[] LoadMap(string path) {
        string json = File.ReadAllText(path);

        List<Types.Gameobject> gameobjects = JsonConvert.DeserializeObject<List<Types.Gameobject>>(json);

        if (gameobjects.Count < 1) {
            maploaded = 1;
            return Obj1ects;
        }

        foreach (var item in gameobjects)
        {
            mapdata.Add(new Rectangle(new Vector2(item.Xpos,item.Ypos),new Vector2(item.Width,item.Height)));
        }

        maploaded = 2;

        return mapdata.ToArray();
    }

    public static Rectangle[] Obj1ects = {
        new Rectangle(-256,128,32,32),
        new Rectangle(-312,512,64,64),
        new Rectangle(-64,64,16,16),
        new Rectangle(-512,512,128,128),
        new Rectangle(-10,100,32,32)
    };

    public static List<Rectangle> levelmap = new List<Rectangle>();
}