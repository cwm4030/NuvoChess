namespace NuvoChess.BoardState;

public ref struct Board()
{
    public byte Stm { get; set; }
    public Span<Piece> Pieces { get; set; }
    public byte WhitePieceCount { get; set; }
    public byte BlackPieceCount { get; set; }
    public Span<byte> Squares { get; set; }
    public byte CastleRights { get; set; }
    public byte EnPassantSquare { get; set; }
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

        ClearPiecesAndSquares();
        SetPieces(pieces);
        SetSideToMove(stm);
        SetCastleRights(cr);
        EnPassantSquare = SquareIndex.SquareNameToIndex.TryGetValue(ep, out var epIndex) ? epIndex : (byte)0;
        HalfMove = short.TryParse(hm, out var halfMove) ? halfMove : 0;
        FullMove = short.TryParse(fm, out var fullMove) ? fullMove : 1;
    }

    public readonly void PrintBoard()
    {
        var backgroundColor = ConsoleColor.DarkBlue;
        var sideToMove = Stm == PieceType.WhitePiece ? "White" : "Black";
        var castleRights = Castling.ToString(CastleRights);
        var enPassantSquare = EnPassantSquare == 0 ? "-" : SquareIndex.SquareIndexToName[EnPassantSquare];
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
                var index = i * 8 + j;
                var onBoardIndex = SquareIndex.OnBoardSquares[index];
                var fen = ' ';
                var square = Squares[onBoardIndex];
                var piece = (byte)0;
                if (SquareType.ContainsPiece(square))
                {
                    piece = Pieces[square].PieceType;
                    fen = Fen.PieceToFen.TryGetValue(piece, out var fenPiece) ? fenPiece : ' ';
                }

                backgroundColor = SetConsoleColor(PieceType.IsWhitePiece(piece), backgroundColor);
                Console.Write($" {fen} ");
                Console.ResetColor();
            }
            backgroundColor = SetConsoleColor(true, backgroundColor);
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

    private void ClearPiecesAndSquares()
    {
        WhitePieceCount = 1;
        BlackPieceCount = 1;
        for (var i = 0; i < Pieces.Length; i++)
        {
            Pieces[i].PieceType = 0;
            Pieces[i].SquareIndex = 0;
        }

        for (var i = 0; i < Squares.Length; i++)
        {
            if (!SquareIndex.SquareIndexToName.ContainsKey((byte)i))
                Squares[i] = SquareType.OffBoardSquare;
            else
                Squares[i] = SquareType.EmptySquare;
        }
    }

    private void SetPieces(string pieces)
    {
        var index = 0;
        foreach (var c in pieces)
        {
            if (index >= SquareIndex.OnBoardSquares.Length) break;
            var squareIndex = SquareIndex.OnBoardSquares[index];

            if (Fen.FenPieces.TryGetValue(c, out var fp))
            {
                var (fenPiece, isFenPiece) = fp;
                if (isFenPiece)
                {
                    SetPieceAndSquare(fenPiece, (byte)squareIndex);
                    index += 1;
                }
                else
                {
                    index += fenPiece;
                }
            }
        }
    }

    private void SetPieceAndSquare(byte piece, byte squareIndex)
    {
        if (PieceType.IsWhitePiece(piece))
        {
            if (PieceType.IsKingPiece(piece))
            {
                Pieces[PieceIndex.WhiteKingIndex].PieceType = piece;
                Pieces[PieceIndex.WhiteKingIndex].SquareIndex = squareIndex;
                Squares[squareIndex] = PieceIndex.WhiteKingIndex;
            }
            else
            {
                var pieceIndex = PieceIndex.WhiteKingIndex + WhitePieceCount;
                if (pieceIndex >= PieceIndex.BlackKingIndex) return;
                Pieces[pieceIndex].PieceType = piece;
                Pieces[pieceIndex].SquareIndex = squareIndex;
                Squares[squareIndex] = (byte)pieceIndex;
                WhitePieceCount += 1;
            }
        }
        else
        {
            if (PieceType.IsKingPiece(piece))
            {
                Pieces[PieceIndex.BlackKingIndex].PieceType = piece;
                Pieces[PieceIndex.BlackKingIndex].SquareIndex = squareIndex;
                Squares[squareIndex] = PieceIndex.BlackKingIndex;
            }
            else
            {
                var pieceIndex = PieceIndex.BlackKingIndex + BlackPieceCount;
                if (pieceIndex >= PieceIndex.PieceListLength) return;
                Pieces[pieceIndex].PieceType = piece;
                Pieces[pieceIndex].SquareIndex = squareIndex;
                Squares[squareIndex] = (byte)pieceIndex;
                BlackPieceCount += 1;
            }
        }
    }

    private void SetSideToMove(string stm)
    {
        if (stm == "w")
            Stm = PieceType.WhitePiece;
        else
            Stm = PieceType.BlackPiece;
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