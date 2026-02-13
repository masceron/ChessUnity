using System.Collections.Generic;
using Game.Effects;
using Game.Effects.Augmentation;
using Game.Piece.PieceLogic.Commons;

namespace Game.Augmentation
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class BubbleFin : Augmentation
    {
        public BubbleFin() : base(AugmentationName.BubbleFin, AugmentationRarity.Basic, AugmentationSlot.Fin, null,
            null)
        {
            PassiveEffects = new List<Effect>();
        }

        public override void SetTarget(PieceLogic target)
        {
            Target = target;
            PassiveEffects.Add(new BubbleFinPassive(-1, -1, Target));
        }
    }
}