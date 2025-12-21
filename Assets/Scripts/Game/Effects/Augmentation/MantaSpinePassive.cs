using Game.Action;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
using Game.Action.Quiets;
namespace Game.Effects.Augmentation
{
    public class MantaSpinePassive : Effect
    {
        public MantaSpinePassive(PieceLogic piece) : base(-1, 1, piece, "effect_manta_spine_passive")
        {
        }

        public override void OnCallPieceAction(Action.Action action)
        {
            if (action == null || action.Target != Piece.Pos || !action.Succeed || (action.Flag & ActionFlag.Unblockable) != 0) return;
            action.Succeed = false;
            var targetPiece = PieceOn(action.Target);
            if (targetPiece != null)
            {
                var (rank, file) = RankFileOf(action.Target);
                var piece = PieceOn(action.Target);
                var color = piece.Color;
                var push = color ? -1 : 1;
                var rankOff = rank;
                
                while (rankOff + push != rank + push * 4    ) {
                
                    var curPos = IndexOf(rankOff + push, file);
                    if (!VerifyIndex(curPos) || !IsActive(curPos) || PieceOn(curPos) != null)
                    {
                        break;
                    }

                    rankOff += push;
                }
                
                ActionManager.EnqueueAction(new NormalMove(action.Target, IndexOf(rankOff, file)));

            }
        }
    }
}