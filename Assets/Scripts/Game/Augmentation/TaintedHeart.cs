using System.Collections.Generic;
using Game.Augmentation.Set;
using Game.Effects;
using Game.Piece.PieceLogic.Commons;

namespace Game.Augmentation
{
    public class TaintedHeart : Augmentation
    {
        public TaintedHeart() : base(AugmentationName.TaintedHeart, AugmentationRarity.Legendary, AugmentationSlot.Blood, null, null)
        {
            PassiveEffects = new List<Effect>();
        }
        
        public override void SetTarget(PieceLogic target)
        {
            Target = target;
            Set = new AugmentationSet(AugmentationSetType.None,false);
            
        }
    }
}