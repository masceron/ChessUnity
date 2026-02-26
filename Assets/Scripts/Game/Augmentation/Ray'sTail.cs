using System.Collections.Generic;
using Game.Augmentation.Set;
using Game.Effects;
using Game.Effects.Augmentation;
using Game.Piece.PieceLogic.Commons;

namespace Game.Augmentation
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class RaySTail : Augmentation
    {
        public RaySTail() : base(AugmentationName.RaysTail, AugmentationRarity.Rare, AugmentationSlot.Fin, null, null)
        {
            PassiveEffects = new List<Effect>();
        }

        public override void SetTarget(PieceLogic target)
        {
            Target = target;
            Set = new AugmentationSet(AugmentationSetType.StalkerInstinct, true);
            PassiveEffects.Add(new RaySTailPassive(Target));
        }
    }
}