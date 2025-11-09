using Game.Effects;
using Game.Piece.PieceLogic;
using System.Collections.Generic;
using Game.Augmentation.Set;
using Game.Effects.Augmentation;

namespace Game.Augmentation
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class TidalRetina : Augmentation
    {
        public TidalRetina() : base(AugmentationName.TidalRetina, AugmentationRarity.Basic, AugmentationSlot.Optic, null, null)
        {
            PassiveEffects = new List<Effect>();
        }

        public override void SetTarget(PieceLogic target)
        {
            Target = target;
            Set = new AugmentationSet(AugmentationSetType.None,false);
            PassiveEffects.Add(new TidalRetinaPassive(-1, 1, target));
        }
    }

}
