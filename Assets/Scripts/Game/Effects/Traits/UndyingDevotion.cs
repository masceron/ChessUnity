using Game.Action;
using Game.Action.Internal;
using Game.Piece.PieceLogic;
using Game.Managers;
using static Game.Common.BoardUtils;
using UnityEngine;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class UndyingDevotion: Effect
    {
        public UndyingDevotion(PieceLogic piece) : base(-1, 1, piece, EffectName.UndyingDevotion)
        {

        }

        /// <summary>
        /// Khi đồng minh chết, sẽ apply OneMoreTurn effect để quân đồng minh sống sót thêm 1 turn nữa
        /// </summary>
        /// <param name="action">Toàn bộ những Action kế thừa từ ICaptures sẽ được truyền vào hàm này </param>
        public override void OnCall(Action.Action action)
        {
            if (action == null || PieceOn(action.Target).Color != Piece.Color || action.Result != ActionResult.Succeed) return;
            action.Result = ActionResult.Failed;
            Debug.Log("Failed capture");
            ActionManager.EnqueueAction(new ApplyEffect(new OneMoreTurn(PieceOn(action.Target))));
        }
    }
}