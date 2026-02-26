using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

namespace Game.Effects.Buffs
{
    public class Amplify : Effect, IEffectStatModifierTrigger
    {
        public Amplify(int duration, int strength, PieceLogic piece) : base(duration, strength, piece, "effect_amplify")
        {
        }

        public int Modify(EffectStat skillStat)
        {
            return skillStat == EffectStat.Radius ? Strength : 0;
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() - 40;
        }
    }
}