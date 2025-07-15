using Game.Board.Action;
using Game.Board.Action.Internal;
using Game.Board.General;
using Math = System.Math;

namespace Game.Board.Effects.Debuffs
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SirenDebuffer: Effect
    {
        public SirenDebuffer(PieceLogic.PieceLogic p) : base(-1, 1, p, EffectType.SirenDebuffer)
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
                    if (pOn == null) continue;
                    if (pOn.color != Piece.color)
                    {
                        ActionManager.EnqueueAction(new SirenDebuff(Piece.pos, Piece.pos, (ushort)index));
                    }
                }
            }
        }
    }
}