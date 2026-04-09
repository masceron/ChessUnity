using System.Collections.Generic;
using Game.Action.Relics;
using Game.Common;
using Game.Effects;
using Game.Managers;
using Game.Relics.Commons;
using UnityEngine;
using UX.UI.Ingame;
using ZLinq;
using Random = System.Random;

namespace Game.Relics
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class CommonPearl : RelicLogic
    {
        public CommonPearl(RelicConfig cfg) : base(cfg)
        {
            TimeCooldown = cfg.TimeCooldown;
            CurrentCooldown = 0;
        }

        public override void Activate(List<Action.Action> actions)
        {
            throw new System.NotImplementedException();
        }

        public override void ActiveForAI()
        {
            Debug.Log("CompleteActionForAI");
            var allPieces = BoardUtils.PieceBoard();
            var listPieces = allPieces.Where(p =>
                p != null && p.Color == Color && !p.Effects.Any(e => e.EffectName == "effect_extremophile")).ToList();
            var minValue = int.MaxValue;
            foreach (var piece in listPieces)
                if (piece.GetValueForAI() < minValue)
                    minValue = piece.GetValueForAI();

            var bestPieceValues = listPieces.Where(p => p.GetValueForAI() == minValue).ToList();
            var minBuff = bestPieceValues.Min(p => p.Effects.Count(e => e.Category == EffectCategory.Buff));
            var bestPiece = bestPieceValues
                .Where(p => p.Effects.Count(e => e.Category == EffectCategory.Buff) == minBuff).ToList();
            if (bestPiece.Count == 0) return;
            var random = new Random();
            var selectedPiece = bestPiece[random.Next(bestPiece.Count)];

            //Làm lại
            // var pending = new CommonPearlPending(this);
            // BoardViewer.Ins.ExecuteAction(pending);
        }
    }
}