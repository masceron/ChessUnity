using Game.Action;


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
            lastSkillUses = color ? ActionManager.WhiteSkillUses : ActionManager.BlackSkillUses;
            EndTurnEffectType = EndTurnEffectType.EndOfAnyTurn;
        }
        public EndTurnEffectType EndTurnEffectType { get; }

        public void OnCallEnd(Action.Action lastMainAction)
        { 
            sbyte tmp = color ? (sbyte)(ActionManager.WhiteSkillUses - lastSkillUses) : (sbyte)(ActionManager.BlackSkillUses - lastSkillUses);
            if (tmp > 0)
            {
                Strength += tmp;
                lastSkillUses = color ? ActionManager.WhiteSkillUses : ActionManager.BlackSkillUses;
            }
            if(Strength > 3) Strength = 3;
           
        }
    }
}