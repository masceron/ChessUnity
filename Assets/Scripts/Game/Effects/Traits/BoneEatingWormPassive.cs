using System.Collections.Generic;
using Game.Action;
using Game.Action.Internal;
using Game.Action.Quiets;
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
            : base(-1, 1, piece, "effect_bone_eating_worm_passive")
        {
            SetStat(EffectStat.Radius, radius);
            SetStat(EffectStat.Counter, counter);
            SetStat(EffectStat.Unit, stack);
        }

        public AfterActionPriority Priority => AfterActionPriority.Debuff;

        public void OnCallAfterPieceAction(Action.Action action)
        {
            if (Piece == null) return;
            if (action == null || action.Result != ResultFlag.Success) return;
            if (action is not IQuiets) return;

            int radius = GetStat(EffectStat.Radius);


            var movedPiece = PieceOn(action.Maker);
            if (movedPiece != null && movedPiece != Piece && movedPiece.Color != Piece.Color)
            {
                int prevPos = movedPiece.PreviousMoves.Count > 0
                    ? movedPiece.PreviousMoves[movedPiece.PreviousMoves.Count - 1]
                    : -1;

                int distToNewPos = Distance(movedPiece.Pos, Piece.Pos);
                int distToOldPos = prevPos == -1 ? int.MaxValue : Distance(prevPos, Piece.Pos);

                if (distToNewPos <= radius && distToOldPos > radius)
                {
                    UnityEngine.Debug.Log("Apply Passive Effects to " );
                    TryApplyPassiveEffects(movedPiece);
                }
            }


            if (action.GetMaker() != Piece || Piece.PreviousMoves.Count <= 0) return;

            int oldPos = Piece.PreviousMoves[Piece.PreviousMoves.Count - 1];
            for (int i = 0; i < BoardSize; i++)
            {
                if (!IsActive(i)) continue;

                var enemy = PieceOn(i);
                if (enemy == null || enemy.Color == Piece.Color) continue;

                int distToNewPos = Distance(enemy.Pos, Piece.Pos);
                int distToOldPos = Distance(enemy.Pos, oldPos);

                if (distToNewPos <= radius && distToOldPos > radius)
                {
                    UnityEngine.Debug.Log("Apply Passive Effects to " );
                    TryApplyPassiveEffects(enemy);
                }
            }
        }

        public void OnCallDead(PieceLogic pieceToDie)
        {
            if (pieceToDie == null || pieceToDie.Color == Piece.Color) return;
            
            var currentCounter = GetRawStat(EffectStat.Counter);
            SetStat(EffectStat.Counter, currentCounter + 2);
        }

        private void TryApplyPassiveEffects(PieceLogic target)
        {
            if (target == null) return;

            int counter = GetStat(EffectStat.Counter);
            int stack = GetStat(EffectStat.Unit);

            int removeShieldChance = 5 + counter;
            if (MatchManager.Roll(removeShieldChance)) RemoveShieldEffects(target);

            int poisonChance = 3 + counter;
            if (MatchManager.Roll(poisonChance))
            {
                ActionManager.EnqueueAction(new ApplyEffect(new Poison(stack, target), Piece));
            }
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
