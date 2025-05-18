using System.Text;

namespace NuvoChess.BoardState;

public static class AttackCheckPin
{
    public const int AttackCheckPinLength = 192;
    public const byte Attack = 1;
    public const byte Defend = 2;
    public const byte Pin = 4;

    public static string ToString(byte attackCheckPin)
    {
        var acp = new StringBuilder()
            .Append((attackCheckPin & Attack) == Attack ? "A" : string.Empty)
            .Append((attackCheckPin & Defend) == Defend ? "D" : string.Empty)
            .Append((attackCheckPin & Pin) == Pin ? "P" : string.Empty)
            .ToString();
        if (acp.Length == 0)
        {
            acp = "   ";
        }
        else if (acp.Length == 1)
        {
            acp = $" {acp} ";
        }
        else if (acp.Length == 2)
        {
            acp = $" {acp}";
        }
        return acp;
    }
}