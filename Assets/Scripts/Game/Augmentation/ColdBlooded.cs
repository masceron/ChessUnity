using System.Collections.Generic;
using Game.Augmentation.Set;
using Game.Effects;
using Game.Effects.Augmentation;
using Game.Piece.PieceLogic.Commons;

namespace Game.Augmentation
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ColdBlooded : Augmentation
    {
        public ColdBlooded() : base(AugmentationName.ColdBlooded, AugmentationRarity.Epic, AugmentationSlot.Blood, null, null)
        {
            PassiveEffects = new List<Effect>();
        }
        
        public override void SetTarget(PieceLogic target)
        {
            Target = target;
            Set = new AugmentationSet(AugmentationSetType.StalkerInstinct, true);
            PassiveEffects.Add(new ColdBloodedPassive(-1, -1, Target));
        }
    }
}