using Game.Common;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;

namespace Game.Action.Quiets
{
    public class MoveToParasitic : Action
    {
        public MoveToParasitic(int maker, int target) : base((PieceLogic)maker, (PieceLogic)target)
        {
        }

        protected override void Animate()
        {
        }

        protected override void ModifyGameState()
        {
            var hostLogic = GetTarget();
            PieceManager.Ins.MoveToParasitic(GetFrom(), GetTargetPos(), hostLogic);

            var parasite = GetMaker() as PieceLogic;
            if (parasite == null) return;

            BoardUtils.PieceBoard()[GetFrom()] = null;
            parasite.Pos = -9999;
        }
    }
}