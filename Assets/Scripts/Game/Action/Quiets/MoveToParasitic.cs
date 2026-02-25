using Game.Common;
using Game.Managers;

namespace Game.Action.Quiets
{
    public class MoveToParasitic : Action, IDontEndTurn
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
            PieceManager.Ins.MoveToParasitic(Maker, Target);

            var parasite = BoardUtils.PieceOn(Maker);
            if (parasite != null)
            {
                BoardUtils.PieceBoard()[Maker] = null;
                parasite.Pos = -9999;
            }
        }
    }

}
