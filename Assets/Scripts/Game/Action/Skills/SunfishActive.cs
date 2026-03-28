using Game.Action.Internal;
using Game.Common;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;
using static Game.Common.BoardUtils;

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

        public SunfishActive(int maker, int target) : base(maker, target)
        {
        }

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            var maker = GetMaker();
            if (maker == null || pieceAI == null) return 0;
            return pieceAI.Color != maker.Color ? -10 : 0;
        }

        protected override void ModifyGameState()
        {
            foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(RankOf(GetFrom()), FileOf(GetFrom()), 4))
            {
                var index = IndexOf(rankOff, fileOff);
                if (!VerifyIndex(index) || !IsActive(index)) continue;
                var targetPiece = PieceOn(index);
                if (targetPiece != null && ColorOfSquare(index) && targetPiece.Color != GetMaker().Color)
                    ActionManager.EnqueueAction(new ApplyEffect(new Blinded(2, 100, targetPiece), GetMaker()));
            }

            SetCooldown(GetMaker(), ((IPieceWithSkill)GetMaker()).TimeToCooldown);
        }
    }
}