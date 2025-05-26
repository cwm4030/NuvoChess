namespace NuvoChess.BoardState;

public ref struct Board(Span<Piece> pieces, Span<byte> squares, Span<byte> attackCheckPinMap)
{
    public byte Stm { get; set; }
    public byte WhitePieceCount { get; set; }
    public byte BlackPieceCount { get; set; }
    public Span<Piece> Pieces { get; set; } = pieces;
    public Span<byte> Squares { get; set; } = squares;
    public Span<byte> AttackDefendPinMap { get; set; } = attackCheckPinMap;
    public byte Checks { get; set; }
    public byte CastleRights { get; set; }
    public byte EpSquare { get; set; }
    public byte EpPawnSquare { get; set; }
    public int HalfMove { get; set; }
    public int FullMove { get; set; }

    public void SetFromFen(string fen)
    {
        var fenStrs = fen.Split(' ').Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();
        var fenPieces = fenStrs.Length >= 1 ? fenStrs[0] : string.Empty;
        var stm = (fenStrs.Length >= 2 ? fenStrs[1] : string.Empty).ToLower();
        var cr = fenStrs.Length >= 3 ? fenStrs[2] : string.Empty;
        var ep = (fenStrs.Length >= 4 ? fenStrs[3] : string.Empty).ToLower();
        var hm = (fenStrs.Length >= 5 ? fenStrs[4] : string.Empty).ToLower();
        var fm = (fenStrs.Length >= 6 ? fenStrs[5] : string.Empty).ToLower();

        ClearPiecesAndSquares();
        SetPiecesAndSquares(fenPieces);
        SetSideToMove(stm);
        SetCastleRights(cr);
        SetEpSquare(ep);
        HalfMove = short.TryParse(hm, out var halfMove) ? halfMove : 0;
        FullMove = short.TryParse(fm, out var fullMove) ? fullMove : 1;
    }

    public readonly void PrintBoard()
    {
        var backgroundColor = ConsoleColor.DarkBlue;
        var sideToMove = Stm == PieceType.WhitePiece ? "White" : "Black";
        var castleRights = Castling.ToString(CastleRights);
        var enPassantSquare = EpSquare == 0 ? "-" : SquareIndex.SquareIndexToName[EpSquare];
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
                var square = Squares[onBoardIndex];
                var piece = Pieces[square].PieceType;
                var fen = Fen.PieceToFen.GetValueOrDefault(piece, ' ');

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

    public readonly void PrintSimpleBoard()
    {
        var backgroundColor = ConsoleColor.DarkBlue;
        var sideToMove = Stm == PieceType.WhitePiece ? "White" : "Black";
        var castleRights = Castling.ToString(CastleRights);
        var enPassantSquare = EpSquare == 0 ? "-" : SquareIndex.SquareIndexToName[EpSquare];
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
                var square = Squares[onBoardIndex];
                var piece = Pieces[square].PieceType;
                var fen = Fen.PieceToSimpleFen.GetValueOrDefault(piece, ' ');

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

    public readonly void PrintAttackDefendPinMap()
    {
        var backgroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine($"Checks: {Checks}");
        Console.WriteLine("Attack Check Pin Map:");
        for (var i = 0; i < 8; i++)
        {
            Console.Write($"   {8 - i} ");
            for (var j = 0; j < 8; j++)
            {
                var index = i * 8 + j;
                var squareIndex = SquareIndex.OnBoardSquares[index];
                var attackPin = AttackDefendPin.ToString(AttackDefendPinMap[squareIndex]);
                backgroundColor = SetConsoleColor(true, backgroundColor);
                Console.Write($"{attackPin}");
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
            Pieces[i].PieceType = PieceType.CapturedPiece;
            Pieces[i].SquareIndex = 0;
        }
        Pieces[PieceType.EmptySquare].PieceType |= PieceType.EmptySquare;
        Pieces[PieceType.GuardSquare].PieceType |= PieceType.GuardSquare;

        for (var i = 0; i < Squares.Length; i++)
        {
            if (!SquareIndex.SquareIndexToName.ContainsKey((byte)i))
                Squares[i] = PieceType.GuardSquare;
            else
                Squares[i] = PieceType.EmptySquare;
        }
    }

    private void SetPiecesAndSquares(string pieces)
    {
        var index = 0;
        foreach (var c in pieces)
        {
            if (index >= SquareIndex.OnBoardSquares.Length) break;
            var squareIndex = SquareIndex.OnBoardSquares[index];

            if (!Fen.FenToPiece.TryGetValue(c, out var p)) continue;
            var (piece, isPiece) = p;
            if (isPiece)
            {
                SetPieceAndSquare(piece, squareIndex);
                index += 1;
            }
            else
            {
                index += piece;
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
        Stm = stm == "w" ? PieceType.WhitePiece : PieceType.BlackPiece;
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

    private void SetEpSquare(string ep)
    {
        EpSquare = SquareIndex.SquareNameToIndex.GetValueOrDefault(ep, (byte)0);
        if (EpSquare != 0)
        {
            EpPawnSquare = Stm == PieceType.WhitePiece ? (byte)(EpSquare + 16) : (byte)(EpSquare - 16);
        }
        else
        {
            EpPawnSquare = 0;
        }
    }
}