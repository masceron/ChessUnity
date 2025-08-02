namespace Game.Board.Piece.PieceLogic
{
    public interface IPieceWithSkill
    {
        protected internal sbyte TimeToCooldown { get; set; }
    }
}