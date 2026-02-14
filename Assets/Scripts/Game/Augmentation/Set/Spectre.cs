using System.Collections.Generic;
using Game.Effects;
using Game.Effects.Augmentation;
using Game.Piece.PieceLogic.Commons;

namespace Game.Augmentation.Set
{
    public class Spectre : AugmentationSet
    {
        public Spectre(bool haveBonus, PieceLogic target) : base(AugmentationSetType.Spectre, haveBonus)
        {
            BonusEffects = new List<Effect>
            {
                new SpectreAugmentation(target)
            };
        }
    }
}