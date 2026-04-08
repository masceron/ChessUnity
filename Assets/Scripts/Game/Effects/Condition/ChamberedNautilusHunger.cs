using System;
using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Effects.Buffs;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

namespace Game.Effects.Condition
{
    public class ChamberedNautilusHunger : Hunger
    {
        public ChamberedNautilusHunger(PieceLogic piece) : base(piece, "effect_chambered_nautilus_hunger")
        {
        }

        override protected void OnHungerTriggered()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Shield(Piece), Piece));
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 25;
        }
    }
}