using Game.Effects.Buffs;
using Game.Effects;
using System.Collections.Generic;
using Game.Piece.PieceLogic.Commons;

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
