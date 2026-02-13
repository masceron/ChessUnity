using Game.Action.Skills;
using Game.Effects.Triggers;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Effects.Buffs
{
    public class Momentum: Effect, IAfterPieceActionTrigger
    {
        public Momentum(int duration, PieceLogic piece) : base(duration, -1, piece, "effect_momentum")
        {}

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 20;
        }

        public AfterActionPriority Priority => AfterActionPriority.Buff;

        public void OnCallAfterPieceAction(Action.Action action)
        {
            if (action is not ISkills) return;
            var caster = PieceOn(action.Maker);
            if (caster.Color == Piece.Color && Piece.SkillCooldown > 0)
            {
                Piece.SkillCooldown--;
            }
        }
    }
}