using static Game.Common.BoardUtils;
using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;
using Game.Action.Quiets;
using Game.Effects.Debuffs;

namespace Game.Action.Skills
{
    public class FlowerhornCichlidActive : Action, ISkills
    {
        public FlowerhornCichlidActive(int maker, int target) : base(maker)
        {
            Target = target;
        }
        protected override void ModifyGameState()
        {
            var makerColor = PieceOn(Maker).Color;
            var direction = makerColor ? 1 : -1;
            var pieceOn = PieceOn(Target);
            if (pieceOn != null)
            {
                var finalRankOfTarget = RankOf(Target);
                if (makerColor)
                {
                    var knockbackCount = 0;
                    while (IsActive(IndexOf(finalRankOfTarget + 1, FileOf(Target))) && knockbackCount < 2)
                    {
                        finalRankOfTarget++;
                        knockbackCount++;
                    }
                }
                else
                {
                    var knockbackCount = 0;
                    while (IsActive(IndexOf(finalRankOfTarget - 1, FileOf(Target))) && knockbackCount < 2)
                    {
                        finalRankOfTarget--;
                        knockbackCount++;
                    }
                }
                var finalRankOfMaker = finalRankOfTarget == RankOf(Target) ? RankOf(Target) - direction : RankOf(Target);
                ActionManager.EnqueueAction(new NormalMove(Target, IndexOf(finalRankOfTarget, FileOf(Target))));
                ActionManager.EnqueueAction(new ApplyEffect(new Stunned(1, pieceOn), PieceOn(Maker)));
                ActionManager.EnqueueAction(new NormalMove(Maker, IndexOf(finalRankOfMaker, FileOf(Target))));
            }
            else
            {
                ActionManager.EnqueueAction(new NormalMove(Maker, Target));
            }
            
        }
        public int AIPenaltyValue(PieceLogic maker)
        {
            return 0;
        }
    }
}