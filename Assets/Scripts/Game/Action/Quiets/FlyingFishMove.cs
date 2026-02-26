using Game.Managers;
using MemoryPack;

namespace Game.Action.Quiets
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class FlyingFishMove : Action, IQuiets
    {
        public int From;

        [MemoryPackConstructor]
        private FlyingFishMove()
        {
        }

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