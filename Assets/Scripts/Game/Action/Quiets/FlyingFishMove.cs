using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Managers;
using static Game.Common.BoardUtils;

namespace Game.Action.Quiets
{
    public class FlyingFishMove: Action, IQuiets
    {
        public FlyingFishMove(int maker, int to) : base(maker, true)
        {
            Maker = (ushort)maker;
            Target = (ushort)to;
        }

        protected override void Animate()
        {
            PieceManager.Ins.Move(Maker, Target);
        }

        protected override void ModifyGameState()
        {
            var (rankFrom, fileFrom) = RankFileOf(Maker);
            var (rankTo, fileTo) = RankFileOf(Target);
            var board = PieceBoard();
            var caller = board[Maker];

            var rankDir = rankTo == rankFrom ? 0 : rankTo > rankFrom ? 1 : -1;
            var fileDir = fileTo == fileFrom ? 0 : fileTo > fileFrom ? 1 : -1;

            while (rankFrom != rankTo || fileFrom != fileTo)
            {
                rankFrom += rankDir;
                fileFrom += fileDir;

                var p = board[IndexOf(rankFrom, fileFrom)];
                if (p == null || p.Color == caller.Color) continue;
                
                ActionManager.EnqueueAction(new ApplyEffect(new Slow(1, 1, p)));
                break;
            }
            
            MatchManager.Ins.GameState.Move(Maker, Target);

            Maker = Target;
        }
    }
}