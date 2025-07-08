using Board.Action;
using Math = System.Math;

namespace Core.Triggers
{
    public class SirenDebuffer: Trigger
    {
        public SirenDebuffer(GameState gameState, PieceData p) : base(gameState, p)
        {
            CalculateEffectRange(p.Pos);
        }

        private void CalculateEffectRange(int pos)
        {
            var rank = pos / GameState.MaxFile;
            var file = pos % GameState.MaxFile;
            
            rankStart = Math.Max(0, rank - 4);
            rankEnd = Math.Min(rank + 4, GameState.MaxRank);
            fileStart = Math.Max(0, file - 4);
            fileEnd = Math.Min(file + 4, GameState.MaxFile);
        }

        private int rankStart;
        private int rankEnd;
        private int fileStart;
        private int fileEnd;

        public override bool CallTrigger()
        {
            if (GameState.LastMovedPiece == Piece && GameState.LastMove.DoesMoveChangePos())
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
                        ActionManager.Execute(GameState, new SirenDebuff(Piece.Pos, index));
                    }
                }
            }
            
            return false;
        }
    }
}