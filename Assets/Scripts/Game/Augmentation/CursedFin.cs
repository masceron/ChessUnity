using System.Collections.Generic;
using Game.Augmentation.Set;
using Game.Effects;
using Game.Effects.Augmentation;
using Game.Piece.PieceLogic.Commons;

namespace Game.Augmentation
{
    public class CursedFin : Augmentation
    {
        public CursedFin() : base(AugmentationName.CursedFin, AugmentationRarity.Cursed, AugmentationSlot.Fin, null, null)
        {
        }
        
        public override void SetTarget(PieceLogic target) 
        {
            Target = target;
            Set = new AugmentationSet(AugmentationSetType.None, false);
            PassiveEffects.Add(new CursedFinPassive(Target));
        }
    }
}