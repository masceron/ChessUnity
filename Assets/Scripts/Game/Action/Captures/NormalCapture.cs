using Game.Managers;

namespace Game.Action.Captures
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class NormalCapture: Action, ICaptures
    {
        public NormalCapture(int f, int t) : base(f)
        {
            Maker = f;
            Target = t;
        }
        protected override void Animate()
        {
            
        }

        protected override void ModifyGameState()
        {
            PieceManager.Ins.Destroy(Target);
            PieceManager.Ins.Move(Maker, Target);
            MatchManager.Ins.GameState.Kill(Target);
            MatchManager.Ins.GameState.Move(Maker, Target);
            Maker = Target;
        }
    }
}