using Game.Managers;
using Game.Action.Internal.Pending;
using UnityEngine;
namespace Game.Action.Captures
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class FrienziedCapture: Action, IPendingAble
    {
        public FrienziedCapture(int f, int t) : base(f, true)
        {
            Maker = f;
            Target = t;
        }
        protected override void Animate()
        {
            
        }

        protected override void ModifyGameState()
        {
        }
        public void CompleteAction()
        {
            Debug.Log("Complete FrienziedCapture");
            PieceManager.Ins.Destroy(Target);
            PieceManager.Ins.Move(Maker, Target);
            MatchManager.Ins.GameState.Kill(Target);
            MatchManager.Ins.GameState.Move(Maker, Target);
            Maker = Target;
        }
    }
}