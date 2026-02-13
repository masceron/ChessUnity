using System.Collections.Generic;
using Game.Effects;
using Game.Effects.Augmentation;
using Game.Piece.PieceLogic.Commons;

namespace Game.Augmentation.Set
{
    public class StalkerInstinct : AugmentationSet
    {
        public StalkerInstinct(bool haveBonus, PieceLogic target) : base(AugmentationSetType.StalkerInstinct, haveBonus)
        {
            BonusEffects = new List<Effect>
            {
                new StalkerInstinctEffect(target)
            };
        }
    }
}