using System.Collections.Generic;
using Game.Action.Relics;
using Game.Common;
using Game.Effects;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Relics.Commons;
using UnityEngine;
using UX.UI.Ingame;
using ZLinq;

namespace Game.Relics
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SeafoamPhial : RelicLogic
    {
        public SeafoamPhial(RelicConfig cfg) : base(cfg)
        {
            CurrentCooldown = 0;
        }

        public override void Activate()
        {
            if (CurrentCooldown == 0)
            {
                foreach (var piece in BoardUtils.PieceBoard())
                {
                    if (piece == null || piece.Color != Color) continue;
                    TileManager.Ins.MarkAsMoveable(piece.Pos);
                    
                    //Làm lại
                    //var pending = new SeafoamPhialPending(this, piece);
                    // BoardViewer.ListOf.Add(pending);
                }

                BoardViewer.Selecting = -2;
                BoardViewer.SelectingFunction = 4;
            }
        }

        public override void ActiveForAI()
        {
            var allPieces = BoardUtils.PieceBoard();
            var bestPieces = new List<PieceLogic>();
            var maxDebuff = -1;

            // Find allied pieces with the most debuffs
            foreach (var piece in allPieces)
                if (piece != null && piece.Color == Color)
                {
                    var debuffCount = BoardUtils.EffectWithEffectCategory(piece, EffectCategory.Debuff).Count;
                    if (debuffCount > maxDebuff)
                    {
                        maxDebuff = debuffCount;
                        bestPieces.Clear();
                        bestPieces.Add(piece);
                    }
                    else if (debuffCount == maxDebuff)
                    {
                        bestPieces.Add(piece);
                    }
                }

            PieceLogic targetPiece = null;

            switch (bestPieces.Count)
            {
                // If none found, default to caster (can be changed later)
                case 0:
                    // targetPiece = GetMakerAsPiece();
                    break;
                case 1:
                    targetPiece = bestPieces[0];
                    break;
                default:
                {
                    // From bestPieces choose the one with lowest AI value; if tie pick random
                    var minScore = bestPieces.Min(p => p.GetValueForAI());
                    var lowestScorePieces = bestPieces.Where(p => p.GetValueForAI() == minScore).ToList();

                    if (lowestScorePieces.Count > 0)
                    {
                        var randomIndex = Random.Range(0, lowestScorePieces.Count);
                        targetPiece = lowestScorePieces[randomIndex];
                    }

                    break;
                }
            }

            if (targetPiece == null) return;
            {
                var excute = new SeafoamPhialAction(targetPiece);
                BoardViewer.Ins.ExecuteAction(excute);

                // var pending = new SeafoamPhialPending(this, targetPiece.Pos);
                // if (pending is PendingAction p)
                // {
                //     BoardViewer.Ins.ExecuteAction(await p.WaitForCompletion());
                // }
            }
        }
    }
}