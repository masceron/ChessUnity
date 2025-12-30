using Game.Action.Skills;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Effects.Buffs
{
    public class Momentum: Effect, ISkillUsedEffect
    {
        public Momentum(sbyte duration, PieceLogic piece) : base(duration, -1, piece, "effect_momentum")
        {}

        public override void OnCallPieceAction(Action.Action action)
        {
            var piece = PieceOn(action.Maker);

            if (piece == null || piece.Color != Piece.Color) return;

            if (action is ISkills)
            {
                ((IPieceWithSkill)Piece).TimeToCooldown--;
            }
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 20;
        }

        public void OnCallSkillUsed(Action.Action skill)
        {
            if (skill is ISkills)
            {
                ((IPieceWithSkill)Piece).TimeToCooldown--;
            }
        }
    }
}