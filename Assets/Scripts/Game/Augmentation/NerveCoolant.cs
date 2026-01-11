using System.Collections.Generic;
using Game.Augmentation.Set;
using Game.Effects;
using Game.Effects.Augmentation;
using Game.Piece.PieceLogic.Commons;

namespace Game.Augmentation
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class NerveCoolant : Augmentation
    {
        public NerveCoolant() : base(AugmentationName.NerveCoolant, AugmentationRarity.Heroic, AugmentationSlot.Neural, null, null)
        {
            PassiveEffects = new List<Effect>();
        }
        
        public override void SetTarget(PieceLogic target)
        {
            Target = target;
            PassiveEffects.Add(new NerveCoolantPassive(-1, -1, Target));
        }
    }
}