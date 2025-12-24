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
            bool makerColor = PieceOn(Maker).Color;
            int direction = makerColor ? 1 : -1;
            PieceLogic pieceOn = PieceOn(Target);
            if (pieceOn != null)
            {
                int finalRankOfTarget = RankOf(Target);
                if (makerColor)
                {
                    int knockbackCount = 0;
                    while (IsActive(IndexOf(finalRankOfTarget + 1, FileOf(Target))) && knockbackCount < 2)
                    {
                        finalRankOfTarget++;
                        knockbackCount++;
                    }
                }
                else
                {
                    int knockbackCount = 0;
                    while (IsActive(IndexOf(finalRankOfTarget - 1, FileOf(Target))) && knockbackCount < 2)
                    {
                        finalRankOfTarget--;
                        knockbackCount++;
                    }
                }
                int finalRankOfMaker = finalRankOfTarget == RankOf(Target) ? RankOf(Target) - direction : RankOf(Target);
                ActionManager.EnqueueAction(new NormalMove(Target, IndexOf(finalRankOfTarget, RankOf(Target))));
                ActionManager.EnqueueAction(new ApplyEffect(new Stunned(1, pieceOn)));
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