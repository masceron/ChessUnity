using Game.Managers;
using Game.Piece.PieceLogic.Commons;
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

        public FlyingFishMove(int maker, int target) : base((PieceLogic)maker, (PieceLogic)target)
        {
            From = maker;
        }

        protected override void Animate()
        {
            PieceManager.Ins.Move(GetFrom(), GetTargetPos());
        }

        protected override void ModifyGameState()
        {
            MatchManager.Ins.GameState.Move(GetMaker(), GetTargetPos());
        }
    }
}