using System.Collections.Generic;
using Game.Augmentation.Set;
using Game.Effects;
using Game.Effects.Augmentation;
using Game.Piece.PieceLogic.Commons;

namespace Game.Augmentation
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class EeriePresence : Augmentation
    {
        public EeriePresence() : base(AugmentationName.EeriePresence, AugmentationRarity.Heroic,
            AugmentationSlot.Neural, null, null)
        {
            PassiveEffects = new List<Effect>();
        }

        public override void SetTarget(PieceLogic target)
        {
            Target = target;
            Set = new AugmentationSet(AugmentationSetType.Spectre, true);
            PassiveEffects.Add(new EeriePresencePassive(-1, -1, Target));
        }
    }
}