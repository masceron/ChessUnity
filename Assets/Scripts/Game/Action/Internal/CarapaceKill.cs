using Game.Managers;

namespace Game.Action.Internal
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class CarapaceKill : Action, IInternal
    {
        public CarapaceKill(int maker, int to) : base(maker)
        {
            Maker = maker;
            Target = to;
        }

        protected override void Animate()
        {
            PieceManager.Ins.Destroy(Target);
        }

        protected override void ModifyGameState()
        {
            MatchManager.Ins.GameState.Kill(Target);
        }
    }
}