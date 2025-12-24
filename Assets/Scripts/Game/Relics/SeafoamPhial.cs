using System.Collections.Generic;
using System.Linq;
using Game.Action.Internal.Pending;
using Game.Action.Internal.Pending.Relic;
using Game.Common;
using Game.Effects;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Relics.Commons;
using UX.UI.Ingame;

namespace Game.Relics
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
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
                foreach (var piece in MatchManager.Ins.GameState.PieceBoard)
                {
                    if (piece == null || piece.Color != Color) continue;
                    TileManager.Ins.MarkAsMoveable(piece.Pos);
                    var pending = new SeafoamPhialPending(this, piece.Pos, piece.Color);
                    BoardViewer.ListOf.Add(pending);
                }
                BoardViewer.Selecting = -2;
                BoardViewer.SelectingFunction = 4;
            }
        }

        public override void ActiveForAI()
        {
            var allPieces = MatchManager.Ins.GameState.PieceBoard;
            var bestPieces = new List<PieceLogic>();
            int maxDebuff = -1;

            // Find allied pieces with the most debuffs
            foreach (var piece in allPieces)
            {
                if (piece != null && piece.Color == Color)
                {
                    int debuffCount = BoardUtils.EffectWithEffectCategory(piece, EffectCategory.Debuff).Count;
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
            }

            PieceLogic targetPiece = null;

            // If none found, default to caster (can be changed later)
            if (bestPieces.Count == 0)
            {
                // targetPiece = PieceOn(Maker);
            }
            else if (bestPieces.Count == 1)
            {
                targetPiece = bestPieces[0];
            }
            else
            {
                // From bestPieces choose the one with lowest AI value; if tie pick random
                int minScore = bestPieces.Min(p => p.GetValueForAI());
                var lowestScorePieces = bestPieces.Where(p => p.GetValueForAI() == minScore).ToList();

                if (lowestScorePieces.Count > 0)
                {
                    int randomIndex = UnityEngine.Random.Range(0, lowestScorePieces.Count);
                    targetPiece = lowestScorePieces[randomIndex];
                }
            }

            if (targetPiece != null)
            {
                var pending = new SeafoamPhialPending(this, targetPiece.Pos, targetPiece.Color);
                if (pending is IPendingAble p)
                {
                    p.CompleteAction();
                }
                else
                {
                    BoardViewer.Ins.ExecuteAction(pending);
                }
            }

        }
    }
}