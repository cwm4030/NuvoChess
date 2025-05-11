using System.Collections.Frozen;

namespace NuvoChess.BoardState;


public static class SquareIndex
{
    public const int SquareListLength = 192;
    // Row 8
    public const byte A8 = 36;
    public const byte B8 = 37;
    public const byte C8 = 38;
    public const byte D8 = 39;
    public const byte E8 = 40;
    public const byte F8 = 41;
    public const byte G8 = 42;
    public const byte H8 = 43;

    // Row 7
    public const byte A7 = 52;
    public const byte B7 = 53;
    public const byte C7 = 54;
    public const byte D7 = 55;
    public const byte E7 = 56;
    public const byte F7 = 57;
    public const byte G7 = 58;
    public const byte H7 = 59;

    // Row 6
    public const byte A6 = 68;
    public const byte B6 = 69;
    public const byte C6 = 70;
    public const byte D6 = 71;
    public const byte E6 = 72;
    public const byte F6 = 73;
    public const byte G6 = 74;
    public const byte H6 = 75;

    // Row 5
    public const byte A5 = 84;
    public const byte B5 = 85;
    public const byte C5 = 86;
    public const byte D5 = 87;
    public const byte E5 = 88;
    public const byte F5 = 89;
    public const byte G5 = 90;
    public const byte H5 = 91;

    // Row 4
    public const byte A4 = 100;
    public const byte B4 = 101;
    public const byte C4 = 102;
    public const byte D4 = 103;
    public const byte E4 = 104;
    public const byte F4 = 105;
    public const byte G4 = 106;
    public const byte H4 = 107;

    // Row 3
    public const byte A3 = 116;
    public const byte B3 = 117;
    public const byte C3 = 118;
    public const byte D3 = 119;
    public const byte E3 = 120;
    public const byte F3 = 121;
    public const byte G3 = 122;
    public const byte H3 = 123;

    // Row 2
    public const byte A2 = 132;
    public const byte B2 = 133;
    public const byte C2 = 134;
    public const byte D2 = 135;
    public const byte E2 = 136;
    public const byte F2 = 137;
    public const byte G2 = 138;
    public const byte H2 = 139;

    // Row 1
    public const byte A1 = 148;
    public const byte B1 = 149;
    public const byte C1 = 150;
    public const byte D1 = 151;
    public const byte E1 = 152;
    public const byte F1 = 153;
    public const byte G1 = 154;
    public const byte H1 = 155;


    public static readonly byte[] OnBoardSquares =
    [
        A8, B8, C8, D8, E8, F8, G8, H8,
        A7, B7, C7, D7, E7, F7, G7, H7,
        A6, B6, C6, D6, E6, F6, G6, H6,
        A5, B5, C5, D5, E5, F5, G5, H5,
        A4, B4, C4, D4, E4, F4, G4, H4,
        A3, B3, C3, D3, E3, F3, G3, H3,
        A2, B2, C2, D2, E2, F2, G2, H2,
        A1, B1, C1, D1, E1, F1, G1, H1
    ];

    public static readonly string[] SquareNames =
    [
        "a8", "b8", "c8", "d8", "e8", "f8", "g8", "h8",
        "a7", "b7", "c7", "d7", "e7", "f7", "g7", "h7",
        "a6", "b6", "c6", "d6", "e6", "f6", "g6", "h6",
        "a5", "b5", "c5", "d5", "e5", "f5", "g5", "h5",
        "a4", "b4", "c4", "d4", "e4", "f4", "g4", "h4",
        "a3", "b3", "c3", "d3", "e3", "f3", "g3", "h3",
        "a2", "b2", "c2", "d2", "e2", "f2", "g2", "h2",
        "a1", "b1", "c1", "d1", "e1", "f1", "g1", "h1"
    ];

    public static readonly FrozenDictionary<string, byte> SquareNameToIndex = SquareNames.Zip(OnBoardSquares).ToDictionary(x => x.First, x => x.Second).ToFrozenDictionary();
    public static readonly FrozenDictionary<byte, string> SquareIndexToName = OnBoardSquares.Zip(SquareNames).ToDictionary(x => x.First, x => x.Second).ToFrozenDictionary();
}