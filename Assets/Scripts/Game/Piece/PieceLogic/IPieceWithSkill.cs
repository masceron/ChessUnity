using Game.Movesets;

namespace Game.Piece.PieceLogic
{
    public interface IPieceWithSkill
    {
        protected internal sbyte TimeToCooldown { get; set; }
        SkillsDelegate Skills { get; }
    }
}