using System.Collections.Generic;
using Game.Augmentation.Set;
using Game.Effects;
using Game.Effects.Augmentation;
using Game.Piece.PieceLogic.Commons;

namespace Game.Augmentation
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class MetalMind : Augmentation
    {
        public MetalMind() : base(AugmentationName.MetalMind, AugmentationRarity.Basic, AugmentationSlot.Neural, null, null)
        {
            PassiveEffects = new List<Effect>();
        }
        
        public override void SetTarget(PieceLogic target)
        {
            Target = target;
            Set = new AugmentationSet(AugmentationSetType.ScrapCollector, false);
            PassiveEffects.Add(new MetalMindPassive(-1, -1, Target));
        }
    }
}