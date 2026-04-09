using Game.Common;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;

namespace Game.Action.Internal
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class DestroyParasitePiece : Action, IInternal
    {
        private readonly PieceLogic _parasite;
        private readonly PieceLogic _hostLogic;

        public DestroyParasitePiece(PieceLogic parasite, PieceLogic hostLogic) : base(null)
        {
            _parasite  = parasite;
            _hostLogic = hostLogic;
        }

        protected override void Animate()
        {
        }

        protected override void ModifyGameState()
        {
            // Dọn data: xóa observers của parasite khỏi TriggerHooks
            _parasite.Effects.ForEach(BoardUtils.GetTriggerHooks().RemoveObserver);

            // Destroy visual: xóa gameObject parasite (đang parented vào host)
            PieceManager.Ins.DestroyParasite(_hostLogic);
        }
    }
}
