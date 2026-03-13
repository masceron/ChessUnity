using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SiphonophorePassive : Effect, IDeadTrigger, IMoveRangeModifierTrigger, IAttackRangeModifier, ISkillStatModifierTrigger
    {
        private int bonusCount;

        public SiphonophorePassive(PieceLogic piece) : base(-1, 1, piece, "effect_siphonophore_passive")
        {
            bonusCount = 0;
        }

        public void OnCallDead(PieceLogic pieceToDie)
        {
            if (pieceToDie == null || pieceToDie.Type != "piece_mini_siphonophore" || pieceToDie.Color != Piece.Color)
                return;
            bonusCount++;
        }

        public int ModifyMoveRange(int baseRange)
        {
            return baseRange + bonusCount;
        }

        public int ModifyAttackRange(int baseRange)
        {
            return baseRange + bonusCount;
        }

        public int Modify(SkillStat stat)
        {
            if (stat == SkillStat.Unit || stat == SkillStat.Range)
                return bonusCount;
            return 0;
        }
    }
}
