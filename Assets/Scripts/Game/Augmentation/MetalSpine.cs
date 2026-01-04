using System.Collections.Generic;
using Game.Augmentation.Set;
using Game.Effects;
using Game.Effects.Augmentation;
using Game.Piece.PieceLogic.Commons;

namespace Game.Augmentation
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class MetalSpine : Augmentation
    {
        public MetalSpine() : base(AugmentationName.MetalSpine, AugmentationRarity.Basic, AugmentationSlot.Chassis, null, null)
        {
            PassiveEffects = new List<Effect>();
        }
        
        public override void SetTarget(PieceLogic target)
        {
            Target = target;
            Set = new AugmentationSet(AugmentationSetType.ScrapCollector,false);
            PassiveEffects.Add(new MetalSpinePassive(-1, -1, Target));
        }
    }
}