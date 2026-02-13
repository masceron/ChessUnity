using MemoryPack;
using Game.Managers;

namespace Game.Action.Quiets
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class FlyingFishMove: Action, IQuiets
    {
        [MemoryPackConstructor]
        private FlyingFishMove() { }

        public int From;
        public FlyingFishMove(int maker, int target) : base(maker)
        {
            From = maker;
            Target = target;
        }

        protected override void Animate()
        {
            PieceManager.Ins.Move(Maker, Target);
        }

        protected override void ModifyGameState()
        {
            MatchManager.Ins.GameState.Move(Maker, Target);
            Maker = Target;
        }
    }
}