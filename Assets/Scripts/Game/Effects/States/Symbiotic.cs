using System.Collections.Generic;
using Game.Action.Captures;
using Game.Common;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

namespace Game.Effects.States
{
    /// <summary>
    ///     State: <b>Symbiotic</b><br/>
    ///     Quân này khi thực hiện action ăn quân vào quân cờ đồng minh, nó sẽ tạo ra một dây nối giữa bản thân và quân được nối.
    /// </summary>
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Symbiotic : StateEffect, IOnMoveGenTrigger
    {
        public override StateType StateType => StateType.Symbiotic;

        public Symbiotic(PieceLogic piece) : base(-1, 0, piece, "effect_symbiotic")
        {
        }

        public bool IsTethered { get; set; } = false;

        public void OnCallMoveGen(PieceLogic caller, List<Action.Action> actions)
        {
            if (caller != Piece) return;
            if (IsTethered) return;

            var allies = BoardUtils.FindAllies(caller.Color);
            var flippedAllies = new List<PieceLogic>();

            // 1. Đổi màu tạm thời các đồng minh để hàm Captures nhận diện là mục tiêu hợp lệ
            foreach (var ally in allies)
            {
                if (ally != caller)
                {
                    ally.Color = !ally.Color;
                    flippedAllies.Add(ally);
                }
            }

            // 2. Lấy danh sách nước đi ăn quân giả lập, lúc này đồng minh sẽ bị tính là địch
            var extraCaptures = new List<Action.Action>();
            caller.Captures(extraCaptures, caller.Pos, true);

            // 3. Trả lại màu ngay lập tức
            foreach (var ally in flippedAllies)
            {
                ally.Color = !ally.Color;
            }

            // 4. Lọc những action hướng vào đồng minh để thêm SymbioticCapture
            foreach (var action in extraCaptures)
            {
                if (action is ICaptures)
                {
                    var targetPiece = action.GetTarget();
                    // Kiểm tra chắc chắn là đồng minh thật sự (vì trong extraCaptures có thể lẫn cả kẻ địch)
                    if (targetPiece != null && targetPiece.Color == caller.Color && targetPiece != caller)
                    {
                        if (targetPiece.CurrentState == StateType.None)
                        {
                            actions.Add(new SymbioticCapture(caller.Pos, action.GetTargetPos()));
                        }
                    }
                }
            }
        }
    }
}
