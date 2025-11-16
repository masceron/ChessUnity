using Game.Effects;
using System.Collections.Generic;
using Game.Augmentation.Set;
using Game.Effects.Augmentation;
using Game.Piece.PieceLogic.Commons;

namespace Game.Augmentation
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ArcherfishAccuracy : Augmentation
    {
        public ArcherfishAccuracy() : base(AugmentationName.ArcherfishAccuracy, AugmentationRarity.Basic, AugmentationSlot.Optic, null, null)
        {
            PassiveEffects = new List<Effect>();
        }

        public override void SetTarget(PieceLogic target)
        {
            Target = target;
            Set = new AugmentationSet(AugmentationSetType.StalkerInstinct,false);
            PassiveEffects.Add(new ArcherfishAccuracyPassive(-1, 1, target));
        }
    }

}
