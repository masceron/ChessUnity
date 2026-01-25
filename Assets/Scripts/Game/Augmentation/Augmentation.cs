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
        TidalRetina = 1,
        ProtectiveLens = 2,
        HemolymphFilter = 3,
        AbyssalTapetum = 4,
        ArcherfishAccuracy = 5,
        RaysTail = 6,
        ColdBlooded = 7,
        MantaSpine = 8,
        MetalSpine = 9,
        MetalScope = 10,
        MetalMind = 11,
        MetalRegulator = 12,
        EeriePresence = 14,
        PredatorEyes = 15,
        ShadowFin = 16,
        TaintedHeart = 17,
        CrownOfSilence = 18,
        LeviathanScale = 19,
        BubbleFin = 20,
        ElusiveFin = 21,
        NerveCoolant = 22,
        BarnacleArmor = 23,
        Ribcage = 24,
        KineticDiffuser = 25,
        CovetLens = 26,
        PuppeteerSpirit = 27,
        PressureMembrane = 28,
        ReefPlating = 29,
        UrchinPlating = 31,
        EchoLocator = 32,
        DuelistReflex = 33,
        BrokenRelocator = 34,
        PressureHull = 35,
        GluttonousJaw = 36,
        StressRegulator = 38,
        FocusModule = 39,
        FungalSac = 40,
        AdaptiveGland = 41,

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

