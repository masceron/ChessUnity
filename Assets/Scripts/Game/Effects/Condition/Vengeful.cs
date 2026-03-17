using System;
using Game.Action;
using Game.Action.Captures;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using UnityEngine;

namespace Game.Effects.Condition
{
    public class Vengeful : Effect, IDeadTrigger, IAfterPieceActionTrigger, IBeforePieceActionTrigger
    {
        protected bool wasVengeful;
        /// <summary>Quân định ăn hoặc đã ăn mình. Null nếu ko phải do ICaptures hoặc capture đã trượt/bãi bỏ.</summary>
        protected PieceLogic Killer;
        public enum VengefulType
        {
            /// <summary>Chỉ trigger khi bị ăn bởi ICaptures.</summary>
            OnCapture,
            /// <summary>Chỉ trigger khi chết vì bất kỳ lý do nào (không phải capture).</summary>
            OnDeath,
        }

        protected VengefulType type;

        public Vengeful(PieceLogic piece, VengefulType type, string effectName) : base(-1, 1, piece, effectName)
        {
            wasVengeful = false;
            this.type = type;
        }

        AfterActionPriority IAfterPieceActionTrigger.Priority => AfterActionPriority.Other;
        BeforeActionPriority IBeforePieceActionTrigger.Priority => BeforeActionPriority.Reaction;

        /// <summary>
        /// Cache lại đối tượng chuẩn bị capture mình.
        /// (Bởi vì sau khi bị capture, quân cờ sẽ chết và không nhận được event AfterPieceAction nữa)
        /// </summary>
        public void OnCallBeforePieceAction(Action.Action action)
        {
            if (type == VengefulType.OnDeath) return;
            if (action.Target == Piece.Pos && action is ICaptures)
            {
                Killer = Game.Common.BoardUtils.PieceOn(action.Maker);
            }
        }

        /// <summary>
        /// Nếu action capture kết thúc mà quân này vẫn sống (không bị chết / block thành công)
        /// thì clear Killer đi để tránh dùng sai cho lần chết sau.
        /// </summary>
        public void OnCallAfterPieceAction(Action.Action action)
        {
            if (type == VengefulType.OnDeath) return;
            if (action.Target == Piece.Pos && action is ICaptures)
            {
                Killer = null; 
            }
        }

        /// <summary>
        /// Được gọi khi quân thực sự bị chết (Kill/Destroy).
        /// </summary>
        public void OnCallDead(PieceLogic pieceToDie)
        {
            if (pieceToDie != Piece) return;

            // Nếu mode là OnCapture, bắt buộc phải có Killer (tức là đang trong quá trình bị ICaptures gọi)
            if (type == VengefulType.OnCapture && Killer == null) return;

            ApplyEffectToPiece();
        }

        protected void ApplyEffectToPiece()
        {
            if (wasVengeful) return;
            OnVengefulTrigger();
            wasVengeful = true;
        }

        protected virtual void OnVengefulTrigger() { }
    }
}
