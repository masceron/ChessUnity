using Game.Common;
using Game.Managers;

namespace Game.Action.Quiets
{
    public class MoveToParasitic : Action
    {
        public MoveToParasitic(int maker, int target) : base(maker)
        {
            Target = target;
        }

        protected override void Animate()
        {
        }

        protected override void ModifyGameState()
        {
            var hostLogic = BoardUtils.PieceOn(Target);
            PieceManager.Ins.MoveToParasitic(Maker, Target, hostLogic);

            var parasite = BoardUtils.PieceOn(Maker);
            if (parasite != null)
            {
                BoardUtils.PieceBoard()[Maker] = null;
                parasite.Pos = -9999;
            }
        }
    }

}
