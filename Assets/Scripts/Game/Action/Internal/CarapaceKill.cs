using Game.Managers;

namespace Game.Action.Internal
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class CarapaceKill : Action, IInternal
    {
        public CarapaceKill(int maker, int to) : base(maker, to)
        {
        }

        protected override void Animate()
        {
            PieceManager.Ins.Destroy(GetTargetPos());
        }

        protected override void ModifyGameState()
        {
            MatchManager.Ins.GameState.Kill(GetTarget());
        }
    }
}