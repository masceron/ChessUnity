using Game.Augmentation.Set;
using Game.Effects.Buffs;
using Game.Effects;
using Game.Piece.PieceLogic;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Augmentation.Set
{
    public class StalkerInstinct : AugmentationSet
    {
        public StalkerInstinct(bool haveBonus, PieceLogic target) : base(AugmentationSetType.StalkerInstinct, haveBonus)
        {
            BonusEffects = new List<Effect>
            {
                new Shield(target)
            };
        }
    }

}
