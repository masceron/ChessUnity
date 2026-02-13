using MemoryPack;
using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
using Game.Common;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class SunfishActive : Action, ISkills
    {
        [MemoryPackConstructor]
        private SunfishActive() { }

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            var maker = PieceOn(Maker);
            if (maker == null || pieceAI == null) return 0;
            return pieceAI.Color != maker.Color ? -10 : 0;
        }
        
        public SunfishActive(int maker, int target) : base(maker)
        {
            Maker = maker;
            Target = target;
        }

        protected override void ModifyGameState()
        {
            foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(RankOf(Maker), FileOf(Maker), 4))
            {
                var index = IndexOf(rankOff, fileOff);
                if (!VerifyIndex(index) || !IsActive(index)) continue;
                var targetPiece = PieceOn(index);
                if (targetPiece != null && ColorOfSquare(index) && targetPiece.Color != PieceOn(Maker).Color)
                {
                    ActionManager.EnqueueAction(new ApplyEffect(new Blinded(2, 100, targetPiece), PieceOn(Maker)));
                }
            }
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }
    }
}