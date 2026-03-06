using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using UnityEngine;

namespace Game.Effects.States
{
    /// <summary>
    ///     Base class cho tất cả State Effect.
    ///     Kế thừa <see cref="Game.Effects.Effect"/> và implement <see cref="IStateful"/>.
    ///     <para>
    ///         Một quân cờ chỉ được mang đúng 1 <see cref="StateEffect"/> tại 1 thời điểm.
    ///         Dùng <see cref="PieceLogic.SetState"/> / <see cref="PieceLogic.ClearState"/> để quản lý.
    ///     </para>
    /// </summary>
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public abstract class StateEffect : Effect, IStateful, IOnApplyTrigger
    {
        public abstract StateType StateType { get; }

        protected StateEffect(int duration, int strength, PieceLogic piece, string name)
            : base(duration, strength, piece, name)
        {
        }

        public virtual void OnApply()
        {
            Debug.Log("Apply state " + this.StateType);
            Piece.SetState(this);
        }
    }
}
