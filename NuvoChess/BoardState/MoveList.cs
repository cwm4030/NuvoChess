namespace NuvoChess.BoardState;

public ref struct MoveList(Span<Move> moves)
{
    public Span<Move> Moves { get; set; } = moves;
    public int MoveCount { get; set; }

    public void Add(int fromSquare, int toSquare, byte promotionPiece)
    {
        Moves[MoveCount].FromSquare = fromSquare;
        Moves[MoveCount].ToSquare = toSquare;
        Moves[MoveCount].PromotionPiece = promotionPiece;
        MoveCount += 1;
    }

    public readonly void PrintMoves()
    {
        for (var i = 0; i < MoveCount; i++)
        {
            var fromSquare = SquareIndex.SquareIndexToName.GetValueOrDefault((byte)Moves[i].FromSquare) ?? "-";
            var toSquare = SquareIndex.SquareIndexToName.GetValueOrDefault((byte)Moves[i].ToSquare) ?? "-";
            var promotionPiece = string.Empty;
            if (Moves[i].PromotionPiece != 0)
                promotionPiece = Fen.PieceToSimpleFen.TryGetValue((byte)(Moves[i].PromotionPiece | PieceType.WhitePiece), out var pp)
                        ? new string([pp])
                        : string.Empty;
            Console.WriteLine($"{i + 1}. {fromSquare}{toSquare}{promotionPiece}");
        }
    }
}