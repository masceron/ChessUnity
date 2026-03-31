using Game.Piece.PieceLogic.Commons;
using MemoryPack;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class SunfishActive : Action, ISkills
    {
        [MemoryPackConstructor]
        private SunfishActive()
        {
        }

        public SunfishActive(PieceLogic maker, int target) : base(maker, target)
        {
        }

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            var maker = GetMakerAsPiece();
            if (maker == null || pieceAI == null) return 0;
            return pieceAI.Color != maker.Color ? -10 : 0;
        }

        protected override void ModifyGameState()
        {
            //Làm lại
            // foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(RankOf(GetFrom()), FileOf(GetFrom()), 4))
            // {
            //     var index = IndexOf(rankOff, fileOff);
            //     if (!VerifyIndex(index) || !IsActive(index)) continue;
            //     var targetPiece = PieceOn(index);
            //     if (targetPiece != null && ColorOfSquare(index) && targetPiece.Color != GetMakerAsPiece().Color)
            //         ActionManager.EnqueueAction(new ApplyEffect(new Blinded(2, 100, targetPiece), GetMakerAsPiece()));
            // }
            //
            // SetCooldown(GetMakerAsPiece(), ((IPieceWithSkill)GetMakerAsPiece()).TimeToCooldown);
        }
    }
}