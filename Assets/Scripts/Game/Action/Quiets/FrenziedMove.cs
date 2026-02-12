using MemoryPack;
using Game.Managers;
using UnityEngine;

namespace Game.Action.Quiets
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class FrenziedMove: Action, IDontEndTurn
    {
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
            Debug.Log("Complete FrenziedMove");
            MatchManager.Ins.GameState.Move(Maker, Target);
            Maker = Target;
        }

        public void CompleteActionForAI()
        {
            //Implement for AI automatically
        }
    }
}