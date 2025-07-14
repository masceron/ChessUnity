using Game.Board.Action;
using Game.Board.Action.Internal;
using Game.Board.General;
using Math = System.Math;

namespace Game.Board.Triggers
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SirenDebuffer: Trigger
    {
        public SirenDebuffer(PieceLogic.PieceLogic p) : base(p, ObserverType.EndTurn, ObserverPriority.Debuff)
        {
            CalculateEffectRange(p.pos);
        }

        private void CalculateEffectRange(int pos)
        {
            var rank = pos / MatchManager.MaxFile;
            var file = pos % MatchManager.MaxFile;
            
            rankStart = Math.Max(0, rank - 4);
            rankEnd = Math.Min(rank + 4, MatchManager.MaxRank - 1);
            fileStart = Math.Max(0, file - 4);
            fileEnd = Math.Min(file + 4, MatchManager.MaxFile - 1);
        }

        private int rankStart;
        private int rankEnd;
        private int fileStart;
        private int fileEnd;

        public override void OnCall(Action.Action action)
        {
            if (action != null && action.Caller == Piece.pos && action.DoesMoveChangePos)
            {
                CalculateEffectRange(Piece.pos);
            }

            for (var r = rankStart; r <= rankEnd; r++)
            {
                var rowIndex = r * MatchManager.MaxFile;
                for (var f = fileStart; f <= fileEnd; f++)
                {
                    var index = rowIndex + f;
                    var pOn = MatchManager.GameState.MainBoard[index];
                    if (pOn == null || pOn == Piece) continue;
                    if (pOn.color != Piece.color)
                    {
                        ActionManager.TakeAction(new SirenDebuff(Piece.pos, Piece.pos, (ushort)index));
                    }
                }
            }
        }
    }
}