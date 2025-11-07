using Game.Common;


namespace Game.Effects.Others
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Charge: Effect, IEndTurnEffect
    {
        private bool color;
        private int lastSkillUses;
        public Charge(sbyte strength, bool color) : base(-1, strength, null, EffectName.Charge)
        {
            this.color = color;
            lastSkillUses = BoardUtils.SkillUseOf(color);
            EndTurnEffectType = EndTurnEffectType.EndOfAnyTurn;
        }
        public EndTurnEffectType EndTurnEffectType { get; }

        public void OnCallEnd(Action.Action lastMainAction)
        { 
            var tmp = color ? (sbyte)(BoardUtils.SkillUseOf(false) - lastSkillUses) : (sbyte)(BoardUtils.SkillUseOf(true) - lastSkillUses);
            if (tmp > 0)
            {
                Strength += tmp;
                lastSkillUses = BoardUtils.SkillUseOf(color);
            }
            if(Strength > 3) Strength = 3;
           
        }
    }
}