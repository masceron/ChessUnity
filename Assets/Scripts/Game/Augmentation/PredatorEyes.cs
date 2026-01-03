using System.Collections.Generic;
using Game.Augmentation.Set;
using Game.Effects;
using Game.Effects.Augmentation;
using Game.Piece.PieceLogic.Commons;

namespace Game.Augmentation
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class PredatorEyes : Augmentation
    {
        public PredatorEyes() : base(AugmentationName.PredatorEyes, AugmentationRarity.Rare, AugmentationSlot.Optic, null, null)
        {
            PassiveEffects = new List<Effect>();
        }
        
        public override void SetTarget(PieceLogic target)
        {
            Target = target;
            Set = new AugmentationSet(AugmentationSetType.Spectre,false);
            PassiveEffects.Add(new PredatorEyesPassive(-1, -1, Target));
        }
    }
}