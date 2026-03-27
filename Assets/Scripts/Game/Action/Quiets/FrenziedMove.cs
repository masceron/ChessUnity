using Game.Managers;
using MemoryPack;
using UnityEngine;

namespace Game.Action.Quiets
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class FrenziedMove : Action, IDontEndTurn
    {
        [MemoryPackConstructor]
        private FrenziedMove()
        {
        }

        public FrenziedMove(int maker, int target) : base(maker)
        {
            Target = target;
        }

        protected override void Animate()
        {
            PieceManager.Ins.Move(Maker, Target);
        }

        protected override void ModifyGameState()
        {
            MatchManager.Ins.GameState.Move(Maker, Target);
        }

        public void CompleteActionForAI()
        {
            //Implement for AI automatically
        }
    }
}