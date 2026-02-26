using System.Collections.Generic;
using Game.Augmentation.Set;
using Game.Common;
using Game.Effects;
using Game.Effects.Augmentation;
using Game.Piece.PieceLogic.Commons;

namespace Game.Augmentation
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class CrownOfSilence : Augmentation
    {
        public CrownOfSilence() : base(AugmentationName.CrownOfSilence, AugmentationRarity.Rare, AugmentationSlot.Fin,
            null, null)
        {
            PassiveEffects = new List<Effect>();
        }

        public override void SetTarget(PieceLogic target)
        {
            Target = target;
            Set = new AugmentationSet(AugmentationSetType.None, false);
            var enemyCommander = BoardUtils.GetCommanderOf(!Target.Color);
            if (enemyCommander != null) PassiveEffects.Add(new CrownOfSilencePassive(enemyCommander));
        }
    }
}