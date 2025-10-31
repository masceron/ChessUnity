using Game.Action;
using Game.Action.Internal;
using Game.Effects;
using Game.Effects.Buffs;
using Game.Piece.PieceLogic;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Augmentation.Set
{
    public class AugmentationSet
    {
        public AugmentationSetType Type;
        public bool HaveBonus;

        public List<Effect> BonusEffects;

        public int RequiredPieces
        {
            get
            {
                return Type switch
                {
                    AugmentationSetType.None => -1,
                    AugmentationSetType.StalkerInstinct => 4,
                    _ => -1
                };
            }
        }

        public AugmentationSet(AugmentationSetType type, bool haveBonus)
        {
            Type = type;
            HaveBonus = haveBonus;
        }

        public void ApplyBonusEffects()
        {
            foreach (Effect e in BonusEffects)
            {
                ActionManager.EnqueueAction(new ApplyEffect(e));
            }
        }
    }

    public enum AugmentationSetType
    {
        None,
        StalkerInstinct
    }
}
