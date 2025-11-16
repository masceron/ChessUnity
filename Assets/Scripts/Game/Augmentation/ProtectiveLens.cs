using System.Collections.Generic;
using Game.Augmentation.Set;
using Game.Effects;
using Game.Effects.Augmentation;
using Game.Piece.PieceLogic.Commons;

namespace Game.Augmentation
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ProtectiveLens : Augmentation
    {
        public ProtectiveLens() : base(AugmentationName.ProtectiveLens, AugmentationRarity.Epic, AugmentationSlot.Optic, null, null)
        {
            PassiveEffects = new List<Effect>();
        }
        
        public override void SetTarget(PieceLogic target)
        {
            Target = target;
            Set = new AugmentationSet(AugmentationSetType.None,false);
            PassiveEffects.Add(new ProtectiveLensPassive(-1, -1, Target));
        }
    }
}