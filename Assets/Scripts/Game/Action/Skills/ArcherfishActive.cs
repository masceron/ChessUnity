using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class ArcherfishActive : Action, ISkills
    {
        [MemoryPackConstructor]
        private ArcherfishActive()
        {
        }

        public ArcherfishActive(PieceLogic maker, PieceLogic target) : base(maker, target)
        {
        }

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            if (GetMakerAsPiece() is not PieceLogic maker || pieceAI == null) return 0;

            // Skill này chỉ tác động lên quân địch.
            // Nếu quân của AI (pieceAI) khác màu với người tạo skill, nó sẽ bị phạt.
            return pieceAI.Color != maker.Color ? -15 : 0;
        }

        protected override void Animate()
        {
        }

        protected override void ModifyGameState()
        {
            // Gây hiệu ứng Blind và Marked lên 1 quân địch trong bán kính 4 ô
            ActionManager.EnqueueAction(new ApplyEffect(new Blinded(2, 100, GetTargetAsPiece()),
                GetMakerAsPiece()));
            ActionManager.EnqueueAction(new ApplyEffect(new Marked(2, GetTargetAsPiece()),
                GetMakerAsPiece()));

            SetCooldown(GetMakerAsPiece(), ((IPieceWithSkill)GetMakerAsPiece()).TimeToCooldown);
        }

        // public void CompleteActionForAI()
        // {
        //     var (rank, file) = RankFileOf(Maker);
        //     var listPieces = GetPiecesInRadius(rank, file,4, p => p != null && p.Color != ((PieceLogic)GetMakerAsPiece()).Color)
        //         .Where(p => p.Effects.Any(p => p.EffectName != "effect_extremophile"
        //                                   && p.EffectName != "effect_blind" &&
        //                                   p.EffectName != "effect_marked")).ToList();

        //     if (listPieces.Count == 0) return;
        //     listPieces.Sort((a, b) => a.GetValueForAI().CompareTo(b.GetValueForAI()));

        //     var idx = UnityEngine.Random.Range(0, listPieces.Count);
        //     ActionManager.EnqueueAction(new ApplyEffect(new Blinded(2, 100, listPieces[idx]), GetMakerAsPiece()));
        //     ActionManager.EnqueueAction(new ApplyEffect(new Marked(2, listPieces[idx]), GetMakerAsPiece()));

        //     SetCooldown(GetMakerAsPiece(), ((IPieceWithSkill)GetMakerAsPiece()).TimeToCooldown);
        // }
    }
}