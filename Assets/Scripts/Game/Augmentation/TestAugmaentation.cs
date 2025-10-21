using Game.Augmentation;
using Game.Effects;
using Game.Piece.PieceLogic;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Augmentation
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class TestAugmaentation : Augmentation
    {
        public TestAugmaentation() : base(AugmentationType.OpticBoost, AugmentationRarity.Basic, AugmentationSlot.Optic, null, null)
        {
            
            //PassiveEffects = passiveEffects;
        }

        public override void SetTarget(PieceLogic target)
        {
            Target = target;
            AugmentationSetInfo testSet = new AugmentationSetInfo
            {
                Type = AugmentationSetType.One,
                HaveBonus = true,
                RequiredPieces = 5,
                SetEffects = new List<Effect>
                {
                    new Game.Effects.Buffs.Shield(Target)
                }
            };
            Set = testSet;
        }
    }

}
