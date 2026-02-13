
using Game.Effects.Triggers;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Buffs
{
    public class LongCast: Effect, ISkillStatModifierTrigger
    {
        public LongCast(int duration, int strength, PieceLogic piece) : base(duration, strength, piece, "effect_longcast")
        {}

        public int Modify(SkillStat stat)
        {
            if (stat == SkillStat.Range)
            {
                return Strength;
            }
            return 0;
        }
        public override int GetValueForAI()
        {
            return base.GetValueForAI() - 40;
        }
    }
}