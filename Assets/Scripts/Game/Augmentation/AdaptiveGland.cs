using System.Collections.Generic;
using Game.Augmentation.Set;
using Game.Effects;
using Game.Piece.PieceLogic.Commons;

namespace Game.Augmentation
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class AdaptiveGland : Augmentation
    {
        public AdaptiveGland() : base(AugmentationName.AdaptiveGland, AugmentationRarity.Rare, AugmentationSlot.Fin,
            null, null)
        {
            PassiveEffects = new List<Effect>();
        }

        public override void SetTarget(PieceLogic target)
        {
            Target = target;
            Set = new AugmentationSet(AugmentationSetType.None, false);
            // PassiveEffects.Add(new AdaptiveGlandPassive(Target));
        }
    }
}