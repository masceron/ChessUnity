using System.Collections.Generic;
using Game.Augmentation.Set;
using Game.Effects;
using Game.Effects.Augmentation;
using Game.Piece.PieceLogic.Commons;
using Game.Action.Internal;

namespace Game.Augmentation
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class MantaSpine : Augmentation
    {
        public MantaSpine() : base(AugmentationName.MantaSpine, AugmentationRarity.Rare, AugmentationSlot.Fin, null, null)
        {
            PassiveEffects = new List<Effect>();
        }
        
        public override void SetTarget(PieceLogic target)
        {

            Target = target;
            Set = new AugmentationSet(AugmentationSetType.None,false);
            PassiveEffects.Add(new MantaSpinePassive(Target));
        }
    }
}