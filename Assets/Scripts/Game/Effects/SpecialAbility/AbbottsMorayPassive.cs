using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Action.Quiets;
using Game.Effects.Buffs;
using Game.Effects.Traits;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using static Game.Common.BoardUtils;

namespace Game.Effects.SpecialAbility
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class AbbottsMorayPassive : Effect, IAfterPieceActionTrigger
    {
        public AbbottsMorayPassive(PieceLogic piece) : base(-1, 1, piece, "effect_abbotts_moray_passive")
        {
            SetStat(EffectStat.Radius, 3);
            SetStat(EffectStat.Duration, 2);
        }

        public AfterActionPriority Priority => AfterActionPriority.Buff;

        private static bool HasAmbushTrait(PieceLogic target)
        {
            if (target == null) return false;
            foreach (var e in target.Effects)
                if (e is Ambush) return true;
            return false;
        }

        public void OnCallAfterPieceAction(global::Game.Action.Action action)
        {
            if (Piece == null) return;
            if (action is not { Result: ResultFlag.Success }) return;
            if (action is not IQuiets && action is not ICaptures) return;

            var radius = GetStat(EffectStat.Radius);
            var duration = GetStat(EffectStat.Duration);

            var movedPiece = action.GetMakerAsPiece();
            if (movedPiece != null && movedPiece != Piece && movedPiece.Color == Piece.Color && HasAmbushTrait(movedPiece))
            {
                var prevPos = movedPiece.PreviousMoves.Count > 0
                        ? movedPiece.PreviousMoves[movedPiece.PreviousMoves.Count - 1]
                        : -1;
                if (Distance(movedPiece.Pos, Piece.Pos) <= radius)
                {
                    

                    if (prevPos == -1 || Distance(prevPos, Piece.Pos) > radius)
                    {
                        ActionManager.EnqueueAction(new ApplyEffect(new Rally(duration, movedPiece)));
                        UnityEngine.Debug.Log("Apply Rally effect to ");
                    }
                }
                if (Distance(prevPos, Piece.Pos) <= radius)
                {
                    if (Distance(movedPiece.Pos, Piece.Pos) > radius)
                    {
                        foreach (var effect in movedPiece.Effects)
                        {
                            if (effect is Rally)
                            {
                                ActionManager.EnqueueAction(new RemoveEffect(effect));
                                UnityEngine.Debug.Log("Remove Rally effect from ");
                            }
                        }
                    }
                }
            }

            if (Piece.PreviousMoves.Count <= 0) return;

            var oldPos = Piece.PreviousMoves[Piece.PreviousMoves.Count - 1];
            for (var i = 0; i < BoardSize; i++)
            {
                if (!IsActive(i)) continue;

                var ally = PieceOn(i);
                if (ally == null || ally == Piece) continue;
                if (ally.Color != Piece.Color) continue;
                if (!HasAmbushTrait(ally)) continue;

                var distToNewPos = Distance(i, Piece.Pos);
                var distToOldPos = Distance(i, oldPos);

                if (distToNewPos <= radius && distToOldPos > radius)
                {
                    ActionManager.EnqueueAction(new ApplyEffect(new Rally(duration, ally)));
                    UnityEngine.Debug.Log("Apply Rally effect to " );
                }
                else if (distToNewPos > radius && distToOldPos <= radius)
                {
                    foreach (var effect in ally.Effects)
                    {
                        if (effect is Rally)
                        {
                            ActionManager.EnqueueAction(new RemoveEffect(effect));
                            UnityEngine.Debug.Log("Remove Rally effect from ");
                        }
                    }
                }
            }
        }
    }
}


