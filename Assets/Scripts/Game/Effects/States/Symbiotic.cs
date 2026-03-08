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

            for (int i = 0; i < actions.Count; i++)
            {
                var action = actions[i];
                if (action is ICaptures)
                {
                    var targetPiece = BoardUtils.PieceOn(action.Target);
                    // Nếu là quân đồng minh
                    if (targetPiece != null && targetPiece.Color == caller.Color)
                    {
                        // Chỉ nối được những quân ở State: None
                        if (targetPiece.CurrentState == StateType.None)
                        {
                            // Thay đổi capture thành kiểu ăn cộng sinh
                            actions[i] = new SymbioticCapture(caller.Pos, action.Target);
                        }
                    }
                }
            }
        }
    }
}
