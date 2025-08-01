using System.Linq;
using Game.Board.Action;
using Game.Board.Action.Internal;
using Game.Board.General;
using Game.Board.Piece.PieceLogic;
using static Game.Common.BoardUtils;

namespace Game.Board.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SirenDebuffer: Effect, IEndTurnEffect
    {
        public SirenDebuffer(PieceLogic p) : base(-1, 1, p, EffectName.SirenDebuffer)
        {
            EndTurnEffectType = EndTurnEffectType.AtEnemyTurn;
            CalculateEffectRange(p.Pos);
        }

        private void CalculateEffectRange(int pos)
        {
            var (rank, file) = RankFileOf(pos);
            
            rankStart = ClampUp(rank - 4);
            rankEnd = ClampDown(rank + 4);
            fileStart = ClampUp(file - 4);
            fileEnd = ClampDown(file + 4);
        }

        private int rankStart;
        private int rankEnd;
        private int fileStart;
        private int fileEnd;

        public void OnCallEnd(Action.Action lastMainAction)
        {
            if (lastMainAction != null && lastMainAction.Caller == Piece.Pos && lastMainAction.DoesMoveChangePos)
            {
                CalculateEffectRange(Piece.Pos);
            }

            for (var r = rankStart; r <= rankEnd; r++)
            {
                var rowIndex = RowIndex(r);
                for (var f = fileStart; f <= fileEnd; f++)
                {
                    var index = rowIndex + f;
                    var pOn = MatchManager.Ins.GameState.PieceBoard[index];
                    if (pOn == null) continue;
                    if (pOn.Color != Piece.Color && pOn.Effects.All(e => e.EffectName != EffectName.Slow))
                    {
                        ActionManager.EnqueueAction(new SirenDebuff(Piece.Pos, Piece.Pos, (ushort)index));
                    }
                }
            }
        }

        public EndTurnEffectType EndTurnEffectType { get; set; }
    }
}