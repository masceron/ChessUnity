using System.Collections.Generic;
using Game.Augmentation.Set;
using Game.Effects;
using Game.Effects.Augmentation;
using Game.Piece.PieceLogic.Commons;

namespace Game.Augmentation
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class MetalScope : Augmentation
    {
        public MetalScope() : base(AugmentationName.MetalScope, AugmentationRarity.Basic, AugmentationSlot.Optic, null, null)
        {
            PassiveEffects = new List<Effect>();
        }
        
        public override void SetTarget(PieceLogic target)
        {
            Target = target;
            Set = new AugmentationSet(AugmentationSetType.ScrapCollector,false);
            PassiveEffects.Add(new MetalScopePassive(-1, -1, Target));
        }
    }
}