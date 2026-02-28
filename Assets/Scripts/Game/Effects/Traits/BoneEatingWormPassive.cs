using System;
using System.Collections.Generic;
using Game.Action;
using Game.Action.Internal;
using Game.Action.Quiets;
using Game.Common;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using Game.Managers;
using static Game.Common.BoardUtils;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class BoneEatingWormPassive : Effect, IAfterPieceActionTrigger, IDeadTrigger
    {
        public BoneEatingWormPassive(int radius, int counter, int stack, PieceLogic piece) 
            : base(-1, 1, piece, "effect_giant_larvacean_passive")
        {
            SetStat(EffectStat.Radius, radius);
            SetStat(EffectStat.Counter, counter);
            SetStat(EffectStat.Unit, stack);
        }

        public AfterActionPriority Priority => AfterActionPriority.Debuff;

        public void OnCallAfterPieceAction(Action.Action action)
        {
            if (action is not IQuiets || action.Result != ResultFlag.Success) return;
            
            var movingPiece = PieceOn(action.Maker);
            if (movingPiece == null || movingPiece.Color == Piece.Color) return;
            
            var radius = GetStat(EffectStat.Radius);
            var distance = Distance(Piece.Pos, action.Target);
            if (distance > radius) return;
            
            var counter = GetStat(EffectStat.Counter);
            var stack = GetStat(EffectStat.Unit);
            
            var removeShieldChance = 100 + counter;
            if (MatchManager.Roll(removeShieldChance))
            {
                RemoveShieldEffects(movingPiece);
            }
            
            var poisonChance = 3 + counter;
            if (MatchManager.Roll(poisonChance))
            {
                ActionManager.EnqueueAction(new ApplyEffect(new Poison(stack, movingPiece), Piece));
            }
        }

        public void OnCallDead(PieceLogic pieceToDie)
        {
            if (pieceToDie == null || pieceToDie.Color == Piece.Color) return;
            
            var currentCounter = GetRawStat(EffectStat.Counter);
            UnityEngine.Debug.Log($"GiantLarvaceanPassive: currentCounter = {currentCounter}");
            SetStat(EffectStat.Counter, currentCounter + 2);
        }

        private void RemoveShieldEffects(PieceLogic targetPiece)
        {
            var effectsToRemove = new List<Effect>();
            
            foreach (var effect in targetPiece.Effects)
            {
                var effectName = effect.EffectName;
                if (effectName == "effect_shield" || 
                    effectName == "effect_hardened_shield" || 
                    effectName == "effect_carapace")
                {
                    effectsToRemove.Add(effect);
                }
            }
            
            foreach (var effect in effectsToRemove)
            {
                ActionManager.EnqueueAction(new RemoveEffect(effect));
            }
        }

    }
}
