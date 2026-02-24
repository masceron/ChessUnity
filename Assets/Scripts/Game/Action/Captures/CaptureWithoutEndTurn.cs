using Game.Managers;
using MemoryPack;
using UnityEngine;

namespace Game.Action.Captures
{
    /// <summary>
    ///     Capture action dùng cho quân có State <see cref="Game.Effects.States.PieceStateType.Cooperative"/>.
    ///     Implement <see cref="IDontEndTurn"/> để action này không kết thúc turn của người chơi.
    /// </summary>
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class CaptureWithoutEndTurn : Action, IDontEndTurn, ICaptures
    {
        [MemoryPackConstructor]
        private CaptureWithoutEndTurn()
        {
        }

        public CaptureWithoutEndTurn(int maker, int target) : base(maker)
        {
            Target = target;
        }

        protected override void Animate()
        {
        }

        protected override void ModifyGameState()
        {
            Debug.Log("Complete CooperativeCapture");
            PieceManager.Ins.Destroy(Target);
            PieceManager.Ins.Move(Maker, Target);
            MatchManager.Ins.GameState.Kill(Target);
            MatchManager.Ins.GameState.Move(Maker, Target);
            Maker = Target;
        }
    }
}
