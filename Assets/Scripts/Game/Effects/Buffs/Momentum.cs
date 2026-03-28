using Game.Action.Skills;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using static Game.Common.BoardUtils;

namespace Game.Effects.Buffs
{
    public class Momentum : Effect, IAfterPieceActionTrigger
    {
        public Momentum(int duration, PieceLogic piece) : base(duration, -1, piece, "effect_momentum")
        {
        }

        public AfterActionPriority Priority => AfterActionPriority.Buff;

        public void OnCallAfterPieceAction(Action.Action action)
        {
            if (action is not ISkills) return;
            var caster = action.GetMaker();
            if (caster.Color == Piece.Color && Piece.SkillCooldown > 0) Piece.SkillCooldown--;
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 20;
        }
    }
}