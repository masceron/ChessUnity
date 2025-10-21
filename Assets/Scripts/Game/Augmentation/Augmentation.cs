using Game.Effects;
using Game.Piece.PieceLogic;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Augmentation
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public abstract class Augmentation
    {
        public AugmentationType Type;
        public AugmentationRarity Rarity;
        public AugmentationSlot Slot;
        public AugmentationSetInfo Set;
        public List<Effect> PassiveEffects;

        protected PieceLogic Target;

        public Augmentation(AugmentationType type, AugmentationRarity rarity, AugmentationSlot slot, AugmentationSetInfo set, List<Effect> passiveEffects)
        {
            Type = type;
            Rarity = rarity;
            Slot = slot;
            Set = set;
            PassiveEffects = passiveEffects;
        }

        public virtual void SetTarget(PieceLogic target)
        {
            Target = target;
        }
    }

    public enum AugmentationType
    {
        OpticBoost,
        NeuralBoost,
        BloodBoost,
        FinBoost,
        ChassisBoost
    }

    public enum AugmentationRarity
    {
        Basic,
        Rare,
        Epic,
        Heroic,
        Legendary,
        Cursed,
        Corrupted
    }

    public enum AugmentationSlot
    {
        Optic,
        Neural,
        Blood,
        Fin,
        Chassis
    }
}

