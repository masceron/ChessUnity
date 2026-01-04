using Game.Managers;

namespace Game.Action.Captures
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class DestroyConstruct: Action, ICaptures
    {
        public DestroyConstruct(int maker, int to) : base(maker)
        {
            Target = (ushort)to;
        }

        protected override void Animate()
        {
            PieceManager.Ins.Destroy(Target);
        }

        protected override void ModifyGameState()
        {
            MatchManager.Ins.GameState.Kill(Maker);
            MatchManager.Ins.GameState.Kill(Target);
        }
    }
}