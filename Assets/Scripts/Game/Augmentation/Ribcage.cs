using System.Collections.Generic;
using Game.Augmentation.Set;
using Game.Effects;
using Game.Effects.Augmentation;
using Game.Piece.PieceLogic.Commons;

namespace Game.Augmentation
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Ribcage : Augmentation
    {
        public Ribcage() : base(AugmentationName.Ribcage, AugmentationRarity.Heroic, AugmentationSlot.Chassis, null, null)
        {
            PassiveEffects = new List<Effect>();
        }
        
        public override void SetTarget(PieceLogic target)
        {
            Target = target;
            PassiveEffects.Add(new RibcagePassive(-1, -1, Target));
        }
    }
}