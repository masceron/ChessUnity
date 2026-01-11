using Game.Effects;
using System.Collections.Generic;
using Game.Augmentation.Set;
using Game.Action;
using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;

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
            foreach (var e in PassiveEffects)
            {
                ActionManager.EnqueueAction(new ApplyEffect(e));
            }
        }

        public virtual int GetValueForAI() { return RarityValue(); }

        protected int RarityValue()
        {
            return Rarity switch
            {
                AugmentationRarity.Basic => 15,
                AugmentationRarity.Rare => 30,
                AugmentationRarity.Epic => 50,
                AugmentationRarity.Heroic => 70,
                AugmentationRarity.Legendary => 100,
                AugmentationRarity.Cursed => 150,
                AugmentationRarity.Corrupted => 300,
                _ => 0
            };
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
        ColdBlooded,
        MantaSpine,
        MetalSpine,
        MetalScope,
        MetalMind,
        MetalRegulator,
        CrownOfSilence,
        LeviathanScale,
        ShadowFin,
        TaintedHeart,
        EeriePresence,
        PredatorEyes,
        AdaptiveGland,
        FungalSac,
        FocusModule,
        Echolocator,
        UrchinPlating,
        StressRegulator,
        None = 10000,
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

