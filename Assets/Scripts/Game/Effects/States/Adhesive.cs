using System.Collections.Generic;
using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Action.Internal.Pending.Piece;
using Game.Action.Quiets;
using Game.Common;
using Game.Piece.PieceLogic.Commons;
using Game.Tile;
using Game.Triggers;
using UX.UI.Ingame;

namespace Game.Effects.States
{
    /// <summary>
    ///     State: <b>Adhesive</b><br/>
    ///     Khi quân này Capture vào quân địch hoặc Formation có State <see cref="StateType.None"/>:<br/>
    ///     - Quân này biến mất tạm thời (di chuyển sang bên cạnh bàn cờ).<br/>
    ///     - State của target (quân hoặc formation) chuyển sang <see cref="StateType.Attached"/>.<br/>
    ///     - Khi target bị ăn/phá hủy: quân Adhesive xuất hiện lại ở vị trí ngẫu nhiên trống xung quanh.<br/>
    ///     Quân Adhesive chỉ thực hiện hành động này lên các quân có State là <see cref="StateType.None"/>.<br/><br/>
    ///     Khác với <see cref="Parasite"/>: Adhesive có thể bám vào cả Piece lẫn Formation.<br/>
    ///     Nếu ô target vừa có Piece vừa có Formation (cả 2 đều State None), sẽ hiện popup 2 nút để người chơi chọn.
    /// </summary>
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Adhesive : StateEffect, IBeforePieceActionTrigger, IOnMoveGenTrigger
    {
        public override StateType StateType => StateType.Adhesive;

        public BeforeActionPriority Priority => BeforeActionPriority.Declaration;

        public Adhesive(PieceLogic piece) : base(-1, 0, piece, "effect_adhesive")
        {
        }

        /// <summary>
        ///     1. Guard: caller != Piece → return.<br/>
        ///     2. Xóa hết <see cref="ICaptures"/> khỏi <paramref name="actions"/>.<br/>
        ///     3. Tạo list capture mới gọi <see cref="PieceLogic.Captures"/> với excludeEmptyTile=false (lấy cả ô trống).<br/>
        ///     4. Lọc lại: ô không có Piece lẫn Formation → xóa; có cả 2 (đều StateNone) → đổi thành <see cref="AdhesiveCapturePending"/>; chỉ 1 trong 2 → để nguyên.<br/>
        ///     5. Merge kết quả vào <paramref name="actions"/>.
        /// </summary>
        public void OnCallMoveGen(PieceLogic caller, List<Action.Action> actions)
        {
            // 1. Guard
            if (caller != Piece) return;

            // 2. Xóa hết ICaptures khỏi danh sách gốc
            actions.RemoveAll(a => a is ICaptures);

            // 3. Tạo list capture mới — gọi Piece.Captures với excludeEmptyTile = false
            //    để lấy TẤT CẢ ô trong tầm ăn kể cả ô trống
            var captureList = new List<Action.Action>();
            Piece.Captures(captureList, Piece.Pos, false);

            // 4. Lọc và biến đổi từng action trong captureList
            for (var i = captureList.Count - 1; i >= 0; i--)
            {
                var action    = captureList[i];
                var target    = action.Target;
                var piece     = BoardUtils.PieceOn(target);
                var formation = BoardUtils.HasFormation(target) ? BoardUtils.GetFormation(target) : null;

                var hasPiece             = piece != null;                                              // có quân bất kể state
                var canAttachPiece       = hasPiece     && piece.CurrentState     == StateType.None;  // có thể attach vào piece
                var canAttachFormation   = formation    != null && formation.CurrentState == StateType.None; // có thể attach vào formation

                if (!hasPiece && !canAttachFormation)
                {
                    // Ô trống hoàn toàn (hoặc formation có state khác None) → loại
                    captureList.RemoveAt(i);
                } else if (!hasPiece && canAttachFormation)
                {
                    continue;
                } else if (hasPiece && !canAttachFormation)
                {
                    continue;
                } else if (canAttachPiece && canAttachFormation)
                {
                    captureList[i] = new AdhesiveCapturePending(Piece.Pos, target, Piece, piece, formation);
                }
                
            }

            actions.AddRange(captureList);
        }

        /// <summary>
        ///     Chỉ xử lý trường hợp Piece-only (ô target có Piece StateNone nhưng không có Formation StateNone).
        ///     Hoặc trường hợp chỉ có Formation (StateNone).
        /// </summary>
        public void OnCallBeforePieceAction(Action.Action action)
        {
            if (action is not ICaptures) return;
            if (action.Maker != Piece.Pos) return;

            var target      = action.Target;
            var targetPiece = BoardUtils.PieceOn(target);

            var hasTargetPiece = targetPiece != null;
            var hasFormation = BoardUtils.HasFormation(target);
            if (!hasTargetPiece && !hasFormation) return;
            ApplySkill(action);

            if (hasTargetPiece && !hasFormation)
            {
                // Chỉ attach khi piece đang ở StateType.None
                if (targetPiece.CurrentState != StateType.None) return;
                action.Result = ResultFlag.Infest;
                ActionManager.EnqueueAction(new ApplyEffect(new Attached(targetPiece, Piece)));
                ActionManager.EnqueueAction(new MoveToAdhesive(Piece.Pos, target, attachToFormation: false));
            } 
            else if (hasFormation && !hasTargetPiece)
            {
                action.Result = ResultFlag.Infest;
                var formation = BoardUtils.GetFormation(target);
                if (formation.CurrentState != StateType.None) return;
                formation.SetState(StateType.Attached);
                var adhesive = Piece; // capture cho lambda
                formation.OnRemoveFormation += (formation) =>
                {
                    formation.ClearState();
                    Attached.SpawnAdhesiveAround(formation.Pos, null, adhesive);
                };
                ActionManager.EnqueueAction(new MoveToAdhesive(Piece.Pos, target, attachToFormation: true));
            }
            
        }

        protected virtual void ApplySkill(Action.Action action)
        {
        }
    }
}
