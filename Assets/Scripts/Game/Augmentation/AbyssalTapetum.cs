using System.Collections.Generic;
using Game.Augmentation.Set;
using Game.Effects;
using Game.Effects.Augmentation;
using Game.Piece.PieceLogic.Commons;

namespace Game.Augmentation
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class AbyssalTapetum : Augmentation
    {
        private readonly static List<PieceLogic> affectedPieces = new();
        public AbyssalTapetum() : base(AugmentationName.AbyssalTapetum, AugmentationRarity.Cursed, AugmentationSlot.Optic, null, null)
        {
            PassiveEffects = new List<Effect>();
        }

        public override void SetTarget(PieceLogic target)
        {
            Target = target;
            affectedPieces.Add(target);
            Set = new AugmentationSet(AugmentationSetType.StalkerInstinct, true);
            PassiveEffects.Add(new AbyssalTapetumPassive(Target));
        }
        public static bool IsSomePiecesHaveThisAugmentation(bool side)
        {
            foreach (PieceLogic pieceLogic in affectedPieces)
            {
                if (pieceLogic.Color == side)
                {
                    return true;
                }
            }
            return false;
        }
    }
}