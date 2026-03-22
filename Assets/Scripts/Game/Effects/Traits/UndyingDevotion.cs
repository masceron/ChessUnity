using Game.Action;
using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using UnityEngine;
using static Game.Common.BoardUtils;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class UndyingDevotion : Effect, IBeforePieceActionTrigger
    {
        public UndyingDevotion(PieceLogic piece) : base(-1, 1, piece, "effect_undying_devotion")
        {
        }

        public BeforeActionPriority Priority => BeforeActionPriority.Mitigation;

        /// <summary>
        ///     Khi đồng minh chết, sẽ apply OneMoreTurn effect để quân đồng minh sống sót thêm 1 turn nữa
        /// </summary>
        /// <param name="action">Toàn bộ những Action kế thừa từ ICaptures sẽ được truyền vào hàm này </param>
        public void OnCallBeforePieceAction(Action.Action action)
        {
            // Cân nhắc làm lại cơ chế
            if (action is not KillPiece killAction) return;
            if (PieceOn(killAction.Maker).Color != Piece.Color || killAction.Result != ResultFlag.Success) return;
            killAction.Result = ResultFlag.SurvivedHit;
            Debug.Log("[UndyingDevotion] Failed capture");
            ActionManager.EnqueueAction(new ApplyEffect(new OneMoreTurn(PieceOn(killAction.Maker)), Piece));
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 150;
        }
    }
}