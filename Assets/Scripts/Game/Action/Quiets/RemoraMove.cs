using Game.Managers;
using static Game.Common.BoardUtils;

namespace Game.Action.Quiets
{
    public class RemoraMove: Action, IQuiets
    {
        public RemoraMove(int maker, int to) : base(maker)
        {
            Target = to;
        }

        protected override void Animate()
        {
            PieceManager.Ins.Move(Maker, Target);
        }

        protected override void ModifyGameState()
        {
            Move(Maker, Target);
            Maker = Target;
        }
    }
}