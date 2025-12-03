// Assets/Scripts/Game/Action/Internal/Pending/Relic/SeafoamPhialPending.cs
using System;
using System.Collections.Generic;
using System.Linq;
using Game.Effects;
using Game.Effects.Buffs;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Relics;
using UX.UI.Ingame;
using UnityEngine;
using static Game.Common.BoardUtils;

namespace Game.Action.Internal.Pending.Relic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SeafoamPhialPending : Action, IPendingAble, System.IDisposable
    {
        private SeafoamPhial seafoamPhial;

        public SeafoamPhialPending(SeafoamPhial seafoamPhial, int maker, bool pos) : base(maker, pos)
        {
            this.seafoamPhial = seafoamPhial;
            Maker = (ushort)maker;
        }

        public void CompleteAction()
        {
            ActionManager.ExecuteImmediately(new Purify(Maker, Maker));
            ActionManager.ExecuteImmediately(new ApplyEffect(new Haste(3, 1, PieceOn(Maker))));
            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;

            seafoamPhial.SetCooldown();
            MatchManager.Ins.InputProcessor.UpdateRelic(); 
        }

        public void CompleteActionForAI()
        {
            var allPieces = MatchManager.Ins.GameState.PieceBoard;
            var bestPieces = new List<PieceLogic>();
            int maxDebuff = -1;

            // Find allied pieces with the most debuffs
            foreach (var piece in allPieces)
            {
                if (piece != null && piece.Color == seafoamPhial.Color)
                {
                    int debuffCount = EffectWithEffectCategory(piece, EffectCategory.Debuff).Count;
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
                ActionManager.ExecuteImmediately(new Purify(Maker, targetPiece.Pos));
                ActionManager.ExecuteImmediately(new ApplyEffect(new Haste(3, 1, targetPiece)));

                seafoamPhial.SetCooldown();
                MatchManager.Ins.InputProcessor.UpdateRelic();
            }
        }

        protected override void ModifyGameState()
        {
        }

        public void Dispose()
        {
            seafoamPhial = null;
            BoardViewer.SelectingFunction = 0;
        }
    }
}
