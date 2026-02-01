using Game.Action.Internal.Pending.Relic;
using Game.Effects;
using Game.Managers;
using Game.Relics.Commons;
using UX.UI.Ingame;
using ZLinq;

namespace Game.Relics
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class CommonPearl : RelicLogic
    {
        public CommonPearl(RelicConfig cfg) : base(cfg)
        {
            TimeCooldown = cfg.TimeCooldown;
            CurrentCooldown = 0;
        }

        public override void Activate()
        {
            if (CurrentCooldown == 0)
            {
                foreach (var piece in MatchManager.Ins.GameState.PieceBoard)
                {
                    if (piece == null && piece.Color != Color) continue;
                    TileManager.Ins.MarkAsMoveable(piece.Pos);
                    var pending = new CommonPearlPending(this, piece.Pos);
                    BoardViewer.ListOf.Add(pending);
                }
                BoardViewer.Selecting = -2;
                BoardViewer.SelectingFunction = 4;
            }
        }

        public override void ActiveForAI()
        {
            UnityEngine.Debug.Log("CompleteActionForAI");
            var allPieces = MatchManager.Ins.GameState.PieceBoard;
            var listPieces = allPieces.Where(p => p != null && p.Color == Color && !p.Effects.Any(e => e.EffectName == "effect_extremophile")).ToList();
            int minValue = int.MaxValue;
            foreach (var piece in listPieces)
            {
                if (piece.GetValueForAI() < minValue)
                {
                    minValue = piece.GetValueForAI();
                }
            }
            var bestPieceValues = listPieces.Where(p => p.GetValueForAI() == minValue).ToList();
            int minBuff = bestPieceValues.Min(p => p.Effects.Count(e => e.Category == EffectCategory.Buff));
            var bestPiece = bestPieceValues.Where(p => p.Effects.Count(e => e.Category == EffectCategory.Buff) == minBuff).ToList();
            if (bestPiece.Count == 0) return;
            var random = new System.Random();
            var selectedPiece = bestPiece[random.Next(bestPiece.Count)];

            var pending = new CommonPearlPending(this, selectedPiece.Pos);
            BoardViewer.Ins.ExecuteAction(pending);
        }
    }
}