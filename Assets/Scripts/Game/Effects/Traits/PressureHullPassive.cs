using Game.Action;
using Game.Action.Internal;
using Game.Effects.Buffs;
using Game.Piece.PieceLogic.Commons;
using Game.Effects.Traits;

namespace Game.Effects.Augmentation
{
    public class PressureHullPassive : Effect, IOnApply
    {
        public PressureHullPassive(sbyte duration, sbyte strength, PieceLogic piece) : base(duration, strength, piece, "effect_pressure_hull_passive")
        { }

        public void OnApply()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Demolisher(Piece)));
            ActionManager.EnqueueAction(new ApplyEffect(new Shield(Piece)));
        }
    }
}