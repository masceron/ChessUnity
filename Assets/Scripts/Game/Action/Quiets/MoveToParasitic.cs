using Game.Common;
using Game.Managers;

namespace Game.Action.Quiets
{
    public class MoveToParasitic : Action
    {
        public MoveToParasitic(int maker, int target) : base(maker, target, TargetingType.LocationTargeting)
        {
        }

        protected override void Animate()
        {
        }

        protected override void ModifyGameState()
        {
            var hostLogic = GetTarget();
            PieceManager.Ins.MoveToParasitic(GetFrom(), GetTargetPos(), hostLogic);

            var parasite = GetMaker();
            if (parasite == null) return;

            BoardUtils.PieceBoard()[GetFrom()] = null;
            parasite.Pos = -9999;
        }
    }
}