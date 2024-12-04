using Raylib_cs;

class Logger
{
    private static List<string> messages = new List<string>();
    public static void clearlogger() {
        messages.Clear();
    }
    public static bool renderlogger() {
        for (int i = 0; i < messages.ToArray().Length; i++)
        {
            Raylib.DrawText(messages[i],10,0 + 20 * Math.Clamp(i + 1,1,int.MaxValue),20,new Color(255,255,255,255 - 40 * Math.Clamp(i + 1,1,int.MaxValue)));
        }
        return true;
    }
    public static bool sendmessage(string message) {
        if (messages.ToArray().Length + 1 > 5) {
            messages.RemoveAt(messages.ToArray().Length - 1);
            messages.Insert(0,message);
        }else {
            messages.Insert(0,message);
        }
        return true;
    }
}