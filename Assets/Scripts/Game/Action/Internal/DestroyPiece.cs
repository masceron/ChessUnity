using Game.Managers;
using Game.Piece.PieceLogic.Commons;

namespace Game.Action.Internal
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class DestroyPiece : Action, IInternal
    {
        public DestroyPiece(PieceLogic maker) : base(maker)
        {
        }

        protected override void Animate()
        {
        }

        protected override void ModifyGameState()
        {
            PieceManager.Ins.Destroy(GetFrom());
            MatchManager.Ins.GameState.Destroy(GetMakerAsPiece());
        }
    }
}