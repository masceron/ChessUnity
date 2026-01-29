using System.Collections.Generic;
using Game.Augmentation.Set;
using Game.Effects;
using Game.Piece.PieceLogic.Commons;

namespace Game.Augmentation
{
    public class CursedModule : Augmentation
    {
        public CursedModule() : base(AugmentationName.CursedModule, AugmentationRarity.Cursed, AugmentationSlot.Neural, null, null)
        {
            PassiveEffects = new List<Effect>();
        }
        
        public override void SetTarget(PieceLogic target) 
        {
            Target = target;
            Set = new AugmentationSet(AugmentationSetType.None, false);
            PassiveEffects.Add(new Effects.Augmentation.CursedModulePassive(Target));
            
        }
    }
}