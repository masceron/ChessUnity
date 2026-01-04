using Game.Action;
using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Effects.Traits;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Effects.Augmentation
{
    public class PredatorEyesPassive : Effect
    {
        public PredatorEyesPassive(sbyte duration, sbyte strength, PieceLogic piece) : base(duration, strength, piece,
            "effect_predator_eyes_passive")
        { }

        public override void OnApply()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Ambush(Piece)));
        }
    }
}