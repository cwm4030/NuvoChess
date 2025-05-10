namespace NuvoChess.BoardState;

public ref struct Board()
{
    public byte Stm { get; set; }
    public Span<byte> Squares { get; set; }
    public byte CastleRights { get; set; }
    public int EnPassantSquare { get; set; }
    public int HalfMove { get; set; }
    public int FullMove { get; set; }

    public void SetFromFen(string fen)
    {
        var fenStrs = fen.Split(' ').Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();
        var pieces = fenStrs.Length >= 1 ? fenStrs[0] : string.Empty;
        var stm = (fenStrs.Length >= 2 ? fenStrs[1] : string.Empty).ToLower();
        var cr = fenStrs.Length >= 3 ? fenStrs[2] : string.Empty;
        var ep = (fenStrs.Length >= 4 ? fenStrs[3] : string.Empty).ToLower();
        var hm = (fenStrs.Length >= 5 ? fenStrs[4] : string.Empty).ToLower();
        var fm = (fenStrs.Length >= 6 ? fenStrs[5] : string.Empty).ToLower();

        ClearSquares();
        SetPieces(pieces);
        SetSideToMove(stm);
        SetCastleRights(cr);
        EnPassantSquare = Fen.SquareNameToIndex.TryGetValue(ep, out var epIndex) ? epIndex : 0;
        HalfMove = short.TryParse(hm, out var halfMove) ? halfMove : 0;
        FullMove = short.TryParse(fm, out var fullMove) ? fullMove : 1;
    }

    public readonly void PrintBoard()
    {
        var backgroundColor = ConsoleColor.DarkBlue;
        var sideToMove = Stm == SideToMove.White ? "White" : "Black";
        var castleRights = Castling.ToString(CastleRights);
        var enPassantSquare = EnPassantSquare == 0 ? "-" : Fen.SquareIndexToName[EnPassantSquare];
        Console.WriteLine($"Side to move: {sideToMove}");
        Console.WriteLine($"Castle rights: {castleRights}");
        Console.WriteLine($"En passant square: {enPassantSquare}");
        Console.WriteLine($"Half move: {HalfMove}");
        Console.WriteLine($"Full move: {FullMove}");
        for (var i = 0; i < 8; i++)
        {
            Console.Write($"   {8 - i} ");
            for (var j = 0; j < 8; j++)
            {
                var squareIndex = i * 8 + j;
                var onBoardIndex = Fen.OnBoardSquareIndexes[squareIndex];
                var piece = Squares[onBoardIndex];
                piece = (byte)(piece & Square.PieceBits);
                var fen = Fen.PieceToFen.TryGetValue(piece, out var fenChar) ? fenChar : ' ';

                backgroundColor = SetConsoleColor(Square.IsColorWhite(piece), backgroundColor);
                Console.Write($" {fen} ");
                Console.ResetColor();
            }
            backgroundColor = SetConsoleColor(Square.IsColorWhite(Square.Empty), backgroundColor);
            Console.ResetColor();
            Console.WriteLine();
        }
        Console.WriteLine("      a  b  c  d  e  f  g  h");
        Console.WriteLine();
    }

    private static ConsoleColor SetConsoleColor(bool isWhitePiece, ConsoleColor backgroundColor)
    {
        Console.ForegroundColor = isWhitePiece ? ConsoleColor.White : ConsoleColor.Black;
        backgroundColor = backgroundColor == ConsoleColor.DarkGray ? ConsoleColor.DarkBlue : ConsoleColor.DarkGray;
        Console.BackgroundColor = backgroundColor;
        return backgroundColor;
    }

    private readonly void ClearSquares()
    {
        for (var i = 0; i < Squares.Length; i++)
        {
            Squares[i] = Square.Empty;
        }
    }

    private readonly void SetPieces(string pieces)
    {
        var index = 0;
        foreach (var c in pieces)
        {
            if (index >= Fen.OnBoardSquareIndexes.Length) break;
            var squareIndex = Fen.OnBoardSquareIndexes[index];

            if (Fen.FenPieces.TryGetValue(c, out var fp))
            {
                var (fenPiece, isFenPiece) = fp;
                if (isFenPiece)
                {
                    Squares[squareIndex] = fenPiece;
                    index += 1;
                }
                else
                {
                    index += fenPiece;
                }
            }
        }
    }

    private void SetSideToMove(string stm)
    {
        if (stm == "w")
            Stm = SideToMove.White;
        else
            Stm = SideToMove.Black;
    }

    private void SetCastleRights(string cr)
    {
        CastleRights = 0;
        foreach (var c in cr)
        {
            switch (c)
            {
                case 'K':
                    CastleRights |= Castling.WhiteKing;
                    break;
                case 'Q':
                    CastleRights |= Castling.WhiteQueen;
                    break;
                case 'k':
                    CastleRights |= Castling.BlackKing;
                    break;
                case 'q':
                    CastleRights |= Castling.BlackQueen;
                    break;
            }
        }
    }
}