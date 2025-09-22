using Game.Action.Internal;
using Game.Common;
using Game.Managers;
using static Game.Common.BoardUtils;

namespace Game.Action.Quiets
{
    public class RemoraMove: Action, IQuiets
    {
        public RemoraMove(int maker, int to) : base(maker, true)
        {
            Target = (ushort)to;
        }

        protected override void ModifyGameState()
        {
            PieceManager.Ins.Move(Maker, Target);
            Move(Maker, Target);
            var color = PieceOn(Target).Color;

            foreach (var (rank, file) in MoveEnumerators.Around(RankOf(Target), FileOf(Target), 1))
            {
                var idx = IndexOf(rank, file);
                var pOn = PieceOn(idx);
                if (pOn == null) continue;
                
                if (pOn.Color == color)
                {
                    ActionManager.EnqueueAction(new Purify(Target, idx));
                }
                else
                {
                    ActionManager.EnqueueAction(new Nullify(Target, idx));
                }
            }

            Maker = Target;
        }
    }
}