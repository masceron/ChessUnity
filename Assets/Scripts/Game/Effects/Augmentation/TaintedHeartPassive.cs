using System;
using System.Collections.Generic;
using Game.Action;
using Game.Action.Internal;
using Game.Common;
using Game.Effects.Buffs;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Augmentation
{
    public class TaintedHeartPassive : Effect, IApplyEffect
    {
        public TaintedHeartPassive(PieceLogic piece) : base(-1, 1, piece, "effect_tainted_heart_passive")
        {
        }
        
        public void OnCallApplyEffect(ApplyEffect applyEffect)
        {
            if (applyEffect.Effect.EffectName == "effect_poison" && applyEffect.Maker == Piece.Pos)
            {
                List<PieceLogic> targets = new();
                var (rank, file) = BoardUtils.RankFileOf(applyEffect.Effect.Piece.Pos);
                foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, 1))
                {
                    var index = BoardUtils.IndexOf(rankOff, fileOff);
                    var pOn = BoardUtils.PieceOn(index);
                    if (pOn == null || pOn.Color == Piece.Color) return;
                    targets.Add(pOn);
                }

                if (targets.Count > 0)
                {
                    var target = targets[UnityEngine.Random.Range(0, targets.Count)];
                    ActionManager.EnqueueAction(new ApplyEffect(new Poison(1, target), Piece));
                }
                
            }
        }
    }
}