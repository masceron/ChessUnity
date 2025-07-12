using Game.Board.Action;
using Game.Board.Action.Internal;
using Game.Board.General;
using Math = System.Math;

namespace Game.Board.Triggers
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SirenDebuffer: Trigger
    {
        public SirenDebuffer(GameState gameState, PieceLogic.PieceLogic p) : base(gameState, p, ObserverType.EndTurn, 3)
        {
            CalculateEffectRange(p.Pos);
        }

        private void CalculateEffectRange(int pos)
        {
            var rank = pos / GameState.MaxFile;
            var file = pos % GameState.MaxFile;
            
            rankStart = Math.Max(0, rank - 4);
            rankEnd = Math.Min(rank + 4, GameState.MaxRank - 1);
            fileStart = Math.Max(0, file - 4);
            fileEnd = Math.Min(file + 4, GameState.MaxFile - 1);
        }

        private int rankStart;
        private int rankEnd;
        private int fileStart;
        private int fileEnd;

        public override void OnCall(Action.Action action)
        {
            if (action != null && action.Caller == Piece.Pos && action.DoesMoveChangePos)
            {
                CalculateEffectRange(Piece.Pos);
            }

            for (var r = rankStart; r <= rankEnd; r++)
            {
                var rowIndex = r * GameState.MaxFile;
                for (var f = fileStart; f <= fileEnd; f++)
                {
                    var index = rowIndex + f;
                    var pOn = GameState.MainBoard[index];
                    if (pOn == null || pOn == Piece) continue;
                    if (pOn.Color != Piece.Color)
                    {
                        ActionManager.Execute(new SirenDebuff(Piece.Pos, Piece.Pos, (ushort)index));
                    }
                }
            }
        }
    }
}