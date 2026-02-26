using System.Collections.Generic;
using Game.Augmentation.Set;
using Game.Effects;
using Game.Effects.Traits;
using Game.Piece.PieceLogic.Commons;

namespace Game.Augmentation
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class GluttonousJaw : Augmentation
    {
        public GluttonousJaw() : base(AugmentationName.GluttonousJaw, AugmentationRarity.Heroic,
            AugmentationSlot.Chassis, null, null)
        {
            PassiveEffects = new List<Effect>();
        }

        public override void SetTarget(PieceLogic target)
        {
            Target = target;
            Set = new AugmentationSet(AugmentationSetType.None, false);
            PassiveEffects.Add(new GluttonousJawPassive(-1, -1, Target));
        }
    }
}