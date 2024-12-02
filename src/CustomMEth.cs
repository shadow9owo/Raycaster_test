using System.ComponentModel.DataAnnotations;

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
}