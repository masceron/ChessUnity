using Game.Movesets;

namespace Game.Piece.PieceLogic.Commons
{
    public interface IPieceWithSkill
    {
        protected internal sbyte TimeToCooldown { get; set; }
        SkillsDelegate Skills { get; }
    }
}