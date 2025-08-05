namespace Game.Piece.PieceLogic
{
    public interface IPieceWithSkill
    {
        protected internal sbyte TimeToCooldown { get; set; }
    }
}