    using Game.Piece.PieceLogic.Commons;
using Game.Effects;

namespace Game.Effects.Buffs
{
    public class Momentum: Effect, ISkillUsedEffect
    {
        public Momentum(PieceLogic piece) : base(-1, 1, piece, "effect_momentum")
        {}
    
        public override int GetValueForAI()
        {
            return base.GetValueForAI();
        }

        public void OnCallSkillUsed(Action.Action skill)
        {
            Piece.SkillCooldown--;
        }
    }
}