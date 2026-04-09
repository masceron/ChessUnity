using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Effects.SpecialAbility;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class MarineFlatworm : Commons.PieceLogic, IPieceWithSkill
    {
        public MarineFlatworm(PieceConfig cfg) : base(cfg, BishopMoves.Quiets, KingMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new MarineFlatwormPassive(this)));

            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown > 0) return;

                if (isPlayer) list.Add(new MarineFlatwormActive(this, Pos));
            };
        }

        int IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}