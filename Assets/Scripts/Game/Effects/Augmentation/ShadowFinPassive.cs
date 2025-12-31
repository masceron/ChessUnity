using Game.Action;
using Game.Action.Internal;
using Game.Effects.Buffs;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Augmentation
{
    public class ShadowFinPassive : Effect
    {
        public ShadowFinPassive(PieceLogic piece) : base(-1, 1, piece, "effect_shadow_fin_passive")
        {
        }

        public override void OnApply()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Camouflage(Piece)));
        }
    }
}