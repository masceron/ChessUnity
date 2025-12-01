using Game.Action;
using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
using UnityEngine;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class UndyingDevotion: Effect
    {
        public UndyingDevotion(PieceLogic piece) : base(-1, 1, piece, "effect_undying_devotion")
        {

        }

        /// <summary>
        /// Khi đồng minh chết, sẽ apply OneMoreTurn effect để quân đồng minh sống sót thêm 1 turn nữa
        /// </summary>
        /// <param name="action">Toàn bộ những Action kế thừa từ ICaptures sẽ được truyền vào hàm này </param>
        public override void OnCallPieceAction(Action.Action action)
        {
            if (action == null || PieceOn(action.Target).Color != Piece.Color || !action.Succeed) return;
            action.Succeed = false;
            Debug.Log("[UndyingDevotion] Failed capture");
            ActionManager.EnqueueAction(new ApplyEffect(new OneMoreTurn(PieceOn(action.Target))));
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 150;
        }
    }
}