using System;
using Game.Action;
using Game.Action.Internal;
using Game.Effects.Traits;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Augmentation
{
    public class ElusiveFinPassive : Effect, IOnApply
    {
        public ElusiveFinPassive(int duration, int strength, PieceLogic piece) : base(duration, strength, piece,
            "effect_elusive_fin_passive")
        { }

        public void OnApply()
        {
            var already = Piece.Effects.FirstOrDefault(e => e.EffectName == "effect_evasion");
            if (already != null)
            {
                already.Strength = Math.Min(100, already.Strength + 10);
            }
            else
            {
                ActionManager.EnqueueAction(new ApplyEffect(new Evasion(-1, 10, Piece)));
            }
        }
    }
}