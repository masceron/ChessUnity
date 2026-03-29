using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

namespace Game.Effects.Buffs
{
    public class Multicast : Effect, ISkillStatModifierTrigger
    {
        public Multicast(PieceLogic piece, int number, int duration) : base(duration, number, piece, "effect_multicast")
        {
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() - 60;
        }
        public int Modify(SkillStat stat)
        {
            if (stat == SkillStat.Unit || stat == SkillStat.Target)
            {
                return Strength;
            }
            return 0;
        }
    }
}