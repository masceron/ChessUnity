using System.Collections.Generic;
using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Action.Internal.Pending.Piece;
using Game.Action.Quiets;
using Game.Common;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

namespace Game.Effects.States
{
    /// <summary>
    ///     State: <b>Adhesive</b><br/>
    ///     Quân này có thể:<br/>
    ///     - Ăn quân <b>địch</b> bình thường (NormalCapture).<br/>
    ///     - Ăn quân <b>đồng minh</b> có State <see cref="StateType.None"/> để <b>attach</b> (ký sinh).<br/>
    ///     - Ăn vào <b>Formation bất kỳ</b> để attach lên formation.<br/>
    ///     - Nếu ô target có cả ally StateNone lẫn Formation: popup <see cref="AdhesiveCapturePending"/> để người chơi chọn.<br/><br/>
    ///     Khi host bị ăn/phá hủy: quân Adhesive xuất hiện lại ở ô trống xung quanh.<br/>
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
        ///     Sinh thêm move:
        ///     <list type="bullet">
        ///         <item>Giữ nguyên ICaptures lên quân địch (không có formation trên ô đó).</item>
        ///         <item>Thêm SymbioticCapture lên quân đồng minh StateNone (dùng flip-color).</item>
        ///         <item>Thêm capture lên ô có formation (bất kỳ):</item>
        ///         <item>  - Ally StateNone + Formation → <see cref="AdhesiveCapturePending"/> (thay thế action ally).</item>
        ///         <item>  - Không có ally (hoặc chỉ có địch) + Formation → giữ nguyên action capture lên ô đó.</item>
        ///     </list>
        /// </summary>
        public void OnCallMoveGen(PieceLogic caller, List<Action.Action> actions)
        {
            if (caller != Piece) return;

            // ─── Bước A: Lấy captures lên đồng minh (flip-color như Symbiotic) ───
            var allies = BoardUtils.FindAllies(caller.Color);
            var flippedAllies = new List<PieceLogic>();

            foreach (var ally in allies)
            {
                if (ally != caller)
                {
                    ally.Color = !ally.Color;
                    flippedAllies.Add(ally);
                }
            }

            var allyCaptures = new List<Action.Action>();
            caller.Captures(allyCaptures, caller.Pos, true); // excludeEmptyTile = true

            foreach (var ally in flippedAllies)
                ally.Color = !ally.Color;

            // Lọc: chỉ giữ target là đồng minh thực sự ở StateNone
            allyCaptures.RemoveAll(a =>
            {
                var p = BoardUtils.PieceOn(a.Target);
                return p == null || p.Color != caller.Color || p.CurrentState != StateType.None;
            });

            // ─── Bước B: Lấy captures lên ô có formation (excludeEmptyTile = false) ───
            var withFormationCaptures = new List<Action.Action>();
            caller.Captures(withFormationCaptures, caller.Pos, false);

            var formationActions = new List<Action.Action>();
            var formationTargets = new HashSet<int>(); // để tránh add trùng

            foreach (var action in withFormationCaptures)
            {
                var target = action.Target;
                if (!BoardUtils.HasFormation(target)) continue;

                var formation = BoardUtils.GetFormation(target);
                var targetPiece = BoardUtils.PieceOn(target);
                var isAllyStateNone = targetPiece != null
                                      && targetPiece.Color == caller.Color
                                      && targetPiece.CurrentState == StateType.None;

                formationTargets.Add(target);

                if (isAllyStateNone)
                {
                    // Ô có ally StateNone + Formation → pending popup
                    formationActions.Add(new AdhesiveCapturePending(Piece.Pos, target, Piece, targetPiece, formation));
                    // Xóa ally-capture riêng lẻ (pending đã gộp cả 2)
                    allyCaptures.RemoveAll(a => a.Target == target);
                }
                else
                {
                    // Ô có formation nhưng không có ally StateNone (trống, có địch, hoặc ally ở state khác) 
                    // → thêm action attach formation (OnCallBeforePieceAction sẽ intercept để attach)
                    formationActions.Add(action);
                }
            }

            // Xóa các captures địch bình thường trên những ô có formation
            // (vì formation action đã đại diện cho ô đó)
            actions.RemoveAll(a => a is ICaptures && formationTargets.Contains(a.Target));

            actions.AddRange(allyCaptures);
            actions.AddRange(formationActions);
        }

        /// <summary>
        ///     Intercept ICaptures khi:<br/>
        ///     - <b>Ally target StateNone</b> → attach lên ally.<br/>
        ///     - <b>Formation trên ô (không có ally)</b> → attach lên formation.<br/>
        ///     - <b>Quân địch thuần (không có formation)</b> → không intercept, NormalCapture chạy bình thường.<br/>
        /// </summary>
        public void OnCallBeforePieceAction(Action.Action action)
        {
            if (action is not ICaptures) return;
            if (action.Maker != Piece.Pos) return;

            var target = action.Target;
            var targetPiece = BoardUtils.PieceOn(target);
            var hasFormation = BoardUtils.HasFormation(target);
            var hasTargetPiece = targetPiece != null;

            var isAlly = hasTargetPiece && targetPiece.Color == Piece.Color;
            var isEnemy = hasTargetPiece && targetPiece.Color != Piece.Color;

            // Ô chỉ có quân địch, không có formation → để NormalCapture chạy bình thường
            if (isEnemy && !hasFormation) return;

            // Ô không có gì cả → không xử lý
            if (!hasTargetPiece && !hasFormation) return;

            if (isAlly && !hasFormation)
            {
                // Attach lên ally StateNone
                if (targetPiece.CurrentState != StateType.None) return;

                ApplySkill(action);
                action.Result = ResultFlag.Infest;
                ActionManager.EnqueueAction(new ApplyEffect(new Attached(targetPiece, Piece)));
                ActionManager.EnqueueAction(new MoveToAdhesive(Piece.Pos, target, attachToFormation: false));
            }
            else if (hasFormation && !isAlly)
            {
                // Attach lên formation (không kill địch nếu có, action gốc bị Infest skip)
                ApplySkill(action);
                action.Result = ResultFlag.Infest;
                var formation = BoardUtils.GetFormation(target);
                formation.SetState(StateType.Attached);
                var adhesive = Piece;
                formation.OnRemoveFormation += (f) =>
                {
                    f.ClearState();
                    Attached.SpawnAdhesiveAround(f.Pos, null, adhesive);
                };
                ActionManager.EnqueueAction(new MoveToAdhesive(Piece.Pos, target, attachToFormation: true));
            }
            // Case isAlly + hasFormation được xử lý qua AdhesiveCapturePending → không rơi vào đây
        }

        protected virtual void ApplySkill(Action.Action action)
        {
        }
    }
}
