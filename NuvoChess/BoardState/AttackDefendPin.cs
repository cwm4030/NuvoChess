using System.Text;

namespace NuvoChess.BoardState;

public static class AttackDefendPin
{
    public const int AttackCheckPinLength = 192;
    public const byte Attack = 1;
    public const byte Defend = 2;
    public const byte Pin = 4;
    public const byte EpPin = 8;

    public static bool IsAttacked(byte attackCheckPin)
    {
        return (attackCheckPin & Attack) == Attack;
    }
    
    public static bool IsDefend(byte attackCheckPin)
    {
        return (attackCheckPin & Defend) == Defend;
    }
    
    public static bool IsPinned(byte attackCheckPin)
    {
        return (attackCheckPin & Pin) == Pin;
    }
    
    public static bool IsEpPinned(byte attackCheckPin)
    {
        return (attackCheckPin & EpPin) == EpPin;
    }

    public static string ToString(byte attackCheckPin)
    {
        var acp = new StringBuilder()
            .Append((attackCheckPin & Attack) == Attack ? "A" : string.Empty)
            .Append((attackCheckPin & Defend) == Defend ? "D" : string.Empty)
            .Append((attackCheckPin & Pin) == Pin ? "P" : string.Empty)
            .Append((attackCheckPin & EpPin) == EpPin ? "E" : string.Empty)
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