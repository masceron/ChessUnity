using Game.Action.Skills;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Effects.Buffs
{
    public class Momentum: Effect, IAfterPieceActionEffect
    {
        public Momentum(sbyte duration, PieceLogic piece) : base(duration, -1, piece, "effect_momentum")
        {}

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 20;
        }

        public void OnCallAfterPieceAction(Action.Action action)
        {
            if (action is not ISkills) return;
            var caster = PieceOn(action.Maker);
            if (caster.Color == Piece.Color && caster.SkillCooldown > 0)
            {
                caster.SkillCooldown--;
            }
        }
    }
}