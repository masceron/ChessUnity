using System.Collections.Generic;
using Game.Augmentation.Set;
using Game.Effects;
using Game.Effects.Augmentation;
using Game.Piece.PieceLogic.Commons;

namespace Game.Augmentation
{
    public class KineticDiffuser : Augmentation
    {
        public KineticDiffuser() : base(AugmentationName.KineticDiffuser, AugmentationRarity.Heroic, AugmentationSlot.Chassis, null, null)
        {
            
        }

        public override void SetTarget(PieceLogic target)
        {
            Target = target;
            Set = new AugmentationSet(AugmentationSetType.None,false);
            PassiveEffects.Add(new KineticDiffuserPassive(Target));
        }
    }
}