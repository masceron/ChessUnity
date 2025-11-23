using Game.Managers;
using UnityEngine;
using Game.Action.Internal.Pending; 
namespace Game.Action.Quiets
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class FrienziedMove: Action, IPendingAble
    {
        public FrienziedMove(int f, int t) : base(f, true)
        {
            Maker = (ushort)f;
            Target = (ushort)t;
        }

        protected override void Animate()
        {
            PieceManager.Ins.Move(Maker, Target);
        }

        protected override void ModifyGameState()
        {
        }
        public void CompleteAction()
        {
            Debug.Log("Complete FrienziedMove");
            PieceManager.Ins.Move(Maker, Target);
            MatchManager.Ins.GameState.Move(Maker, Target);
            Maker = Target;
        }

        public void CompleteActionForAI()
        {
            //Implement for AI automatically
        }
    }
}