using Game.Movesets;

namespace Game.Piece.PieceLogic.Commons
{
    public interface IPieceWithSkill
    {
        protected internal int TimeToCooldown { get; set; }
        SkillsDelegate Skills { get; }
    }
}