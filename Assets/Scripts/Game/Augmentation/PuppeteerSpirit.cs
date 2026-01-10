using System.Collections.Generic;
using Game.Augmentation.Set;
using Game.Effects;
using Game.Effects.Augmentation;
using Game.Piece.PieceLogic.Commons;
using UnityEditor;

namespace Game.Augmentation
{
    public class PuppeteerSpirit : Augmentation
    {
        public PuppeteerSpirit() : base(AugmentationName.PuppeteerSpirit, AugmentationRarity.Corrupted, AugmentationSlot.Neural, null, null)
        {
            PassiveEffects = new List<Effect>();
        }

        public override void SetTarget(PieceLogic target)
        {
            Target = target;
            Set = new AugmentationSet(AugmentationSetType.None,false);
            PassiveEffects.Add(new PuppeteerSpiritPassive(Target));
        }
    }
}