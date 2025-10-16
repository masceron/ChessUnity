using Game.Piece.PieceLogic;
using Game.Action;
using Game.Action.Internal;
using Game.Effects.Buffs;
using Game.Effects.Debuffs;
using Game.Managers;
using Game.Piece.PieceLogic;
using static Game.Common.BoardUtils;
using Game.Action.Skills;
using UnityEngine;
using Game.Action.Internal;


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