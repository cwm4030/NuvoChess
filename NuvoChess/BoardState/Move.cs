namespace NuvoChess.BoardState;

public struct Move
{
    public int FromSquare { get; set;}
    public int ToSquare { get; set; }
    public int PromotionPiece { get; set; }
}