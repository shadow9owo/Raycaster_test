using System.Numerics;

class CustomMEth //MATH LIB (no meth here)
{
    public static float InvClamp(float input,float min,float max) {
        if (input > max) {
            return min;
        }
        else if (input < min) {
            return max;
        }
        return input;
    }
    public static float Clamp10(float input) {
        if (Math.Abs(input +1) < Math.Abs(input - 1)) {
            return -1;
        } else if (Math.Abs(input +1) > Math.Abs(input - 1)) {
            return 1;
        }
        return 0;
    }
    public static float SafeDivide(float input,float divideby) {
        try 
        {
            return Math.Clamp(input,1,float.MaxValue) / Math.Clamp(divideby,1,float.MaxValue); //works in our case
        }
        catch (DivideByZeroException ex)
        {

        }
        return 1;
    }
    public static Vector2 vector2avg(Vector2[] vector2arr) {
        Vector2 tmp = Vector2.Zero;
        foreach (var item in vector2arr)
        {
            tmp = Vector2.Add(item,tmp);
        }
        return new Vector2(tmp.X / vector2arr.Length,tmp.Y / vector2arr.Length);
    }
}