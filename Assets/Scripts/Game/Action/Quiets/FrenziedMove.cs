using Game.Managers;
using MemoryPack;

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

        public FrenziedMove(int maker, int target) : base(maker, target, TargetingType.LocationTargeting)
        {
        }

        protected override void Animate()
        {
            PieceManager.Ins.Move(GetFrom(), GetTargetPos());
        }

        protected override void ModifyGameState()
        {
            MatchManager.Ins.GameState.Move(GetMaker(), GetTargetPos());
        }

        public void CompleteActionForAI()
        {
            //Implement for AI automatically
        }
    }
}