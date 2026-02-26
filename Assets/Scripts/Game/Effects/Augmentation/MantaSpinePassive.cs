using System.Collections.Generic;
using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Action.Quiets;
using Game.Common;
using Game.Effects.Traits;
using Game.Piece;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using UnityEngine;
using static Game.Common.BoardUtils;

namespace Game.Effects.Augmentation
{
    public class MantaSpinePassive : Effect, IBeforePieceActionTrigger
    {
        private bool isFirstCaptured = true;

        public MantaSpinePassive(PieceLogic piece) : base(-1, 1, piece, "effect_manta_spine_passive")
        {
        }

        public BeforeActionPriority Priority => BeforeActionPriority.Mitigation;

        public void OnCallBeforePieceAction(Action.Action action)
        {
            // if (action.Target != Piece.Pos || 
            //     action.Maker == Piece.Pos || 
            //     (action.Flag & ActionFlag.Unblockable) != 0) 
            //     return;
            // if (action is not ICaptures) return;
            // var hasBlockingEffect = Piece.Effects.Any(e => 
            //     e.EffectName == "effect_shield" || 
            //     e.EffectName == "effect_hardened_shield" || 
            //     e.EffectName == "effect_carapace");

            // if (hasBlockingEffect) return;

            // if (action.Result != ResultFlag.Success) return;
            //action.Result = ResultFlag.Dodged;

            if (action is not ICaptures || action.Target != Piece.Pos || action.Maker == Piece.Pos) return;

            if (isFirstCaptured)
            {
                isFirstCaptured = false;
                action.Result = ResultFlag.Blocked;
                MovePieceAndMakeIllusion(action);
            }
        }

        private void MovePieceAndMakeIllusion(Action.Action action)
        {
            var targetPiece = PieceOn(action.Target);
            if (targetPiece != null)
            {
                var (rank, file) = RankFileOf(action.Target);
                var piece = PieceOn(action.Target);
                var color = piece.Color;
                var push = color ? -1 : 1;
                var rankOff = rank;

                while (rankOff + push != rank + push * 4)
                {
                    var curPos = IndexOf(rankOff + push, file);
                    if (!VerifyIndex(curPos) || !IsActive(curPos) || PieceOn(curPos) != null) break;

                    rankOff += push;
                }

                var indexList = new List<int>();
                foreach (var (r, l) in MoveEnumerators.AroundUntil(rankOff, file, 1))
                {
                    var index = IndexOf(r, l);
                    if (PieceOn(index) != null || !VerifyIndex(index) || !IsActive(index)) continue;
                    indexList.Add(index);
                }

                if (indexList.Count > 0)
                {
                    var countToSelect = Mathf.Min(2, indexList.Count);
                    var selectedIndices = new List<int>();
                    var available = new List<int>(indexList);

                    for (var i = 0; i < countToSelect; i++)
                    {
                        if (available.Count == 0) break;
                        var randomIdx = Random.Range(0, available.Count);
                        selectedIndices.Add(available[randomIdx]);
                        available.RemoveAt(randomIdx);
                    }

                    ActionManager.EnqueueAction(new NormalMove(action.Target, IndexOf(rankOff, file)));

                    foreach (var selectedIndex in selectedIndices)
                    {
                        var config = new PieceConfig(piece.Type, Piece.Color, selectedIndex);
                        ActionManager.EnqueueAction(new SpawnPieceWithEffect(config,
                            new Illusion(PieceOn(selectedIndex))));
                    }
                }
            }
        }
    }
}