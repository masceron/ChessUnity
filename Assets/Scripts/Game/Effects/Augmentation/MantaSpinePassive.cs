using System.Collections.Generic;
using Game.Action;
using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
using Game.Action.Quiets;
using Game.Common;
using Game.Effects.Traits;
using UnityEngine;
using Game.Piece;
namespace Game.Effects.Augmentation
{
    public class MantaSpinePassive : Effect
    {
        public MantaSpinePassive(PieceLogic piece) : base(-1, 1, piece, "effect_manta_spine_passive")
        {
        }

        public override void OnCallPieceAction(Action.Action action)
        {
            Debug.Log("MantaSpinePassive OnCallPieceAction");
            if (action == null || action.Target != Piece.Pos || !action.Succeed || (action.Flag & ActionFlag.Unblockable) != 0) return;
            action.Succeed = false;
            var targetPiece = PieceOn(action.Target);
            if (targetPiece != null)
            {
                var (rank, file) = RankFileOf(action.Target);
                var piece = PieceOn(action.Target);
                var color = piece.Color;
                var push = color ? -1 : 1;
                var rankOff = rank;
                
                while (rankOff + push != rank + push * 4) {
                
                    var curPos = IndexOf(rankOff + push, file);
                    if (!VerifyIndex(curPos) || !IsActive(curPos) || PieceOn(curPos) != null)
                    {
                        break;
                    }

                    rankOff += push;
                }
                var indexList = new List<int>();
                foreach (var (r, l) in MoveEnumerators.AroundUntil(rankOff, file, 1))
                {
                    var index = IndexOf(r, l);
                    if (PieceOn(index) != null || !VerifyIndex(index) || !IsActive(index))
                    {
                        continue;
                    }
                    indexList.Add(index);
                }
                if (indexList.Count > 0)
                {
                    int countToSelect = Mathf.Min(2, indexList.Count);
                    var selectedIndices = new List<int>();
                    var available = new List<int>(indexList);
                    
                    for (int i = 0; i < countToSelect; i++)
                    {
                        if (available.Count == 0) break;
                        int randomIdx = Random.Range(0, available.Count);
                        selectedIndices.Add(available[randomIdx]);
                        available.RemoveAt(randomIdx);
                    }
                    ActionManager.EnqueueAction(new NormalMove(action.Target, IndexOf(rankOff, file)));


                    foreach (var selectedIndex in selectedIndices)
                    {
                        var config = new PieceConfig(piece.Type, Piece.Color, (ushort)selectedIndex);
                        ActionManager.EnqueueAction(new SpawnPieceWithEffect(config, new Illusion(PieceOn(selectedIndex))));
                        
                    }
                }

                
            }
        }
    }
}