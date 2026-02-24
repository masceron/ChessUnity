using Game.Common;
using Game.Managers;
using MemoryPack;
using UnityEngine;

namespace Game.Action.Quiets
{
    /// <summary>
    ///     Move action dùng cho quân có State <see cref="Game.Effects.States.PieceStateType.Cooperative"/>.
    ///     Implement <see cref="IDontEndTurn"/> để action này không kết thúc turn của người chơi.
    /// </summary>
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class MoveWithoutEndturn : Action, IDontEndTurn, IQuiets
    {
        [MemoryPackConstructor]
        private MoveWithoutEndturn()
        {
        }

        public MoveWithoutEndturn(int maker, int target) : base(maker)
        {
            Target = target;
        }

        protected override void Animate()
        {
            PieceManager.Ins.Move(Maker, Target);
        }

        protected override void ModifyGameState()
        {
            Debug.Log("Complete CooperativeMove");
            BoardUtils.Move(Maker, Target);
            MatchManager.Ins.GameState.Move(Maker, Target);
            Maker = Target;
        }
    }
}
