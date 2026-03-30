using Game.Action.Internal;
using Game.Action.Quiets;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [MemoryPackable]
    public partial class FlowerhornCichlidActive : Action, ISkills
    {
        [MemoryPackConstructor]
        private FlowerhornCichlidActive()
        {
        }

        public FlowerhornCichlidActive(PieceLogic maker, int target) : base(maker, target)
        {
        }

        public int AIPenaltyValue(PieceLogic maker)
        {
            return 0;
        }

        protected override void ModifyGameState()
        {
            var makerColor = GetMakerAsPiece().Color;
            var direction = makerColor ? 1 : -1;
            var pieceOn = GetTargetAsPiece();
            if (pieceOn != null)
            {
                var finalRankOfTarget = RankOf(GetTargetPos());
                if (makerColor)
                {
                    var knockbackCount = 0;
                    while (IsActive(IndexOf(finalRankOfTarget + 1, FileOf(GetTargetPos()))) && knockbackCount < 2)
                    {
                        finalRankOfTarget++;
                        knockbackCount++;
                    }
                }
                else
                {
                    var knockbackCount = 0;
                    while (IsActive(IndexOf(finalRankOfTarget - 1, FileOf(GetTargetPos()))) && knockbackCount < 2)
                    {
                        finalRankOfTarget--;
                        knockbackCount++;
                    }
                }

                var finalRankOfMaker =
                    finalRankOfTarget == RankOf(GetTargetPos())
                        ? RankOf(GetTargetPos()) - direction
                        : RankOf(GetTargetPos());
                ActionManager.EnqueueAction(new NormalMove(PieceOn(IndexOf(finalRankOfTarget, FileOf(GetTargetPos()))),
                    IndexOf(finalRankOfTarget, FileOf(GetTargetPos()))));
                ActionManager.EnqueueAction(new ApplyEffect(new Stunned(1, pieceOn),
                    GetMakerAsPiece()));
                ActionManager.EnqueueAction(
                    new NormalMove(GetMakerAsPiece(), IndexOf(finalRankOfMaker, FileOf(GetTargetPos()))));
            }
            else
            {
                ActionManager.EnqueueAction(new NormalMove(GetMakerAsPiece(), GetTargetPos()));
            }
        }
    }
}