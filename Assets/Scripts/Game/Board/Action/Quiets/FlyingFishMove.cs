using Game.Board.Action.Internal;
using Game.Board.Effects.Debuffs;
using Game.Board.General;
using Game.Board.Piece;
using static Game.Common.BoardUtils;

namespace Game.Board.Action.Quiets
{
    public class FlyingFishMove: Action, IQuiets
    {
        public FlyingFishMove(int caller, int to) : base(caller, true)
        {
            From = (ushort)caller;
            To = (ushort)to;
        }

        protected override void Animate()
        {
            PieceManager.Ins.Move(Caller, To);
        }

        protected override void ModifyGameState()
        {
            var (rankFrom, fileFrom) = RankFileOf(From);
            var (rankTo, fileTo) = RankFileOf(To);
            var board = PieceBoard();
            var caller = board[From];

            var rankDir = rankTo == rankFrom ? 0 : rankTo > rankFrom ? 1 : -1;
            var fileDir = fileTo == fileFrom ? 0 : fileTo > fileFrom ? 1 : -1;

            while (rankFrom != rankTo || fileFrom != fileTo)
            {
                rankFrom += rankDir;
                fileFrom += fileDir;

                var p = board[IndexOf(rankFrom, fileFrom)];
                if (p == null || p.Color == caller.Color) continue;
                
                ActionManager.EnqueueAction(new ApplyEffect(new Slow(2, 1, p)));
                break;
            }
            
            MatchManager.Ins.GameState.Move(From, To);

            Caller = To;
        }
    }
}