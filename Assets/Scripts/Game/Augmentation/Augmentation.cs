using Game.Effects;
using Game.Piece.PieceLogic;
using System.Collections.Generic;
using Game.Augmentation.Set;
using Game.Action;
using Game.Action.Internal;

namespace Game.Augmentation
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public abstract class Augmentation
    {
        public AugmentationName Name;
        public AugmentationRarity Rarity;
        public AugmentationSlot Slot;
        public AugmentationSet Set;
        public List<Effect> PassiveEffects;

        protected PieceLogic Target;

        public Augmentation(AugmentationName name, AugmentationRarity rarity, AugmentationSlot slot, AugmentationSet set, List<Effect> passiveEffects)
        {
            Name = name;
            Rarity = rarity;
            Slot = slot;
            Set = set;
            PassiveEffects = passiveEffects;
        }

        public virtual void SetTarget(PieceLogic target)
        {
            Target = target;
        }

        public void ApplyPassiveEffects()
        {
            foreach (Effect e in PassiveEffects)
            {
                ActionManager.EnqueueAction(new ApplyEffect(e));
            }
        }
    }

    public enum AugmentationName
    {
        TidalRetina,
        ProtectiveLens,
        HemolymphFilter,
        AbyssalTapetum,
        ArcherfishAccuracy,
        RaysTail,
        ColdBlooded
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

