using Game.Action.Internal.Pending.Relic;
using Game.Action.Relics;
using Game.Managers;
using Game.Relics.Commons;
using UX.UI.Ingame;
using System.Linq;
using Game.Effects;
using Game.Piece.PieceLogic.Commons;

namespace Game.Relics
{
    public class BlackPearl : RelicLogic, IRelicAction
    {
        public BlackPearl(RelicConfig cfg) : base(cfg)
        {
            TimeCooldown = cfg.TimeCooldown;
            CurrentCooldown = 0;
        }

        public override void Activate()
        {
            if (CurrentCooldown != 0) return;
            
            foreach (var piece in MatchManager.Ins.GameState.PieceBoard)
            {
                if (piece == null) continue;
                TileManager.Ins.MarkAsMoveable(piece.Pos);
                var pending = new BlackPearlPending(this, piece.Pos);
                BoardViewer.ListOf.Add(pending);
            }
            BoardViewer.Selecting = -2;
            BoardViewer.SelectingFunction = 4;
        }

        public override void ActiveForAI()
        {
            var allPieces = MatchManager.Ins.GameState.PieceBoard;
            var listPiecesA = allPieces.Where(p => p != null && p.Color == Color && !p.Effects.Any(e => e.EffectName == "effect_extremophile")).ToList();
            var listPiecesB = allPieces.Where(p => p != null && p.Color != Color && !p.Effects.Any(e => e.EffectName == "effect_extremophile")).ToList();

            if (listPiecesA.Count == 0 || listPiecesB.Count == 0) return;
            int minValueA = listPiecesA.Min(p => p.GetValueForAI());

            int maxValueB = listPiecesB.Max(p => p.GetValueForAI());

            var bestPiecesValuesA = listPiecesA.Where(p => p.GetValueForAI() == minValueA).ToList();
            int minBuffA = bestPiecesValuesA.Min(p => p.Effects.Count(e => e.Category == EffectCategory.Buff));
            var bestPiecesA = bestPiecesValuesA.Where(p => p.Effects.Count(e => e.Category == EffectCategory.Buff) == minBuffA).ToList();
            var bestPiecesValuesB = listPiecesB.Where(p => p.GetValueForAI() == maxValueB).ToList();
            var bestPiecesB = bestPiecesValuesB.Where(p => p.Effects.Count(e => e.Category == EffectCategory.Debuff) <= 5).ToList();

            PieceLogic selectedPiece = null;
            if (bestPiecesB.Count != 0) {
                var random = new System.Random();
                selectedPiece = bestPiecesB[random.Next(bestPiecesB.Count)];
            } else if (bestPiecesA.Count != 0) {
                var random = new System.Random();
                selectedPiece = bestPiecesA[random.Next(bestPiecesA.Count)];
            } else {
                return;
            }
            if (selectedPiece == null) return;
            var pending = new BlackPearlPending(this, selectedPiece.Pos);
            BoardViewer.Ins.ExecuteAction(pending);
        }
    }
}