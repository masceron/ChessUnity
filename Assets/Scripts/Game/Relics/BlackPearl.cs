using System;
using System.Collections.Generic;
using Game.Action.Relics;
using Game.Common;
using Game.Effects;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Relics.Commons;
using UX.UI.Ingame;
using ZLinq;

namespace Game.Relics
{
    public class BlackPearl : RelicLogic, IRelicAction
    {
        public BlackPearl(RelicConfig cfg) : base(cfg)
        {
            TimeCooldown = cfg.TimeCooldown;
            CurrentCooldown = 0;
        }

        public override void Activate(List<Action.Action> actions)
        {
            throw new NotImplementedException();
        }

        public override void ActiveForAI()
        {
            var allPieces = BoardUtils.PieceBoard();
            var listPiecesA = allPieces.Where(p =>
                p != null && p.Color == Color && !p.Effects.Any(e => e.EffectName == "effect_extremophile")).ToList();
            var listPiecesB = allPieces.Where(p =>
                p != null && p.Color != Color && !p.Effects.Any(e => e.EffectName == "effect_extremophile")).ToList();

            if (listPiecesA.Count == 0 || listPiecesB.Count == 0) return;
            var minValueA = listPiecesA.Min(p => p.GetValueForAI());

            var maxValueB = listPiecesB.Max(p => p.GetValueForAI());

            var bestPiecesValuesA = listPiecesA.Where(p => p.GetValueForAI() == minValueA).ToList();
            var minBuffA = bestPiecesValuesA.Min(p => p.Effects.Count(e => e.Category == EffectCategory.Buff));
            var bestPiecesA = bestPiecesValuesA
                .Where(p => p.Effects.Count(e => e.Category == EffectCategory.Buff) == minBuffA).ToList();
            var bestPiecesValuesB = listPiecesB.Where(p => p.GetValueForAI() == maxValueB).ToList();
            var bestPiecesB = bestPiecesValuesB
                .Where(p => p.Effects.Count(e => e.Category == EffectCategory.Debuff) <= 5).ToList();

            PieceLogic selectedPiece = null;
            if (bestPiecesB.Count != 0)
            {
                var random = new Random();
                selectedPiece = bestPiecesB[random.Next(bestPiecesB.Count)];
            }
            else if (bestPiecesA.Count != 0)
            {
                var random = new Random();
                selectedPiece = bestPiecesA[random.Next(bestPiecesA.Count)];
            }
            else
            {
                return;
            }

            if (selectedPiece == null) return;
            //Làm lại
            // var pending = new BlackPearlPending(this, selectedPiece.Pos);
            // BoardViewer.Ins.ExecuteAction(pending);
        }
    }
}