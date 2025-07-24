using Game.Board.Action;
using Game.Board.Action.Internal;
using Game.Board.General;
using Game.Board.Piece.PieceLogic;
using static Game.Common.BoardUtils;

namespace Game.Board.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SirenDebuffer: Effect
    {
        public SirenDebuffer(PieceLogic p) : base(-1, 1, p, EffectType.SirenDebuffer)
        {
            CalculateEffectRange(p.pos);
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

        public override void OnCall(Action.Action action)
        {
            if (action != null && action.Caller == Piece.pos && action.DoesMoveChangePos)
            {
                CalculateEffectRange(Piece.pos);
            }

            for (var r = rankStart; r <= rankEnd; r++)
            {
                var rowIndex = RowIndex(r);
                for (var f = fileStart; f <= fileEnd; f++)
                {
                    var index = rowIndex + f;
                    var pOn = MatchManager.gameState.MainBoard[index];
                    if (pOn == null) continue;
                    if (pOn.color != Piece.color)
                    {
                        ActionManager.EnqueueAction(new SirenDebuff(Piece.pos, Piece.pos, (ushort)index));
                    }
                }
            }
        }

        public override string Description()
        {
            return MatchManager.assetManager.EffectData[EffectName].description;
        }
    }
}