using System.Collections.Generic;
using Game.Action;
using Game.Action.Internal;
using Game.Effects;

namespace Game.Augmentation.Set
{
    public class AugmentationSet
    {
        public List<Effect> BonusEffects;
        public bool HaveBonus;
        public AugmentationSetType Type;

        public AugmentationSet(AugmentationSetType type, bool haveBonus)
        {
            Type = type;
            HaveBonus = haveBonus;
        }

        public int RequiredPieces
        {
            get
            {
                return Type switch
                {
                    AugmentationSetType.None => -1,
                    AugmentationSetType.StalkerInstinct => 4,
                    AugmentationSetType.ScrapCollector => 3,
                    AugmentationSetType.Spectre => 2,
                    _ => -1
                };
            }
        }

        public void ApplyBonusEffects()
        {
            foreach (var e in BonusEffects) ActionManager.EnqueueAction(new ApplyEffect(e));
        }
    }

    public enum AugmentationSetType
    {
        None,
        StalkerInstinct,
        ScrapCollector,
        Spectre
    }
}