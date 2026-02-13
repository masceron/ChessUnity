using Game.Managers;

namespace Game.Action.Internal
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class DestroyPiece : Action, IInternal
    {
        public DestroyPiece(int maker) : base(maker)
        {
        }

        protected override void Animate()
        {
        }

        protected override void ModifyGameState()
        {
            PieceManager.Ins.Destroy(Maker);
            MatchManager.Ins.GameState.Destroy(Maker);
        }
    }
}