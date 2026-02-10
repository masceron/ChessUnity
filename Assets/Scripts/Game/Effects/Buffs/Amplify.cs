using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Buffs
{
    public class Amplify: Effect, IEffectStatModifier
    {
        public Amplify(sbyte duration, sbyte strength, PieceLogic piece) : base(duration, strength, piece, "effect_amplify")
        {}

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