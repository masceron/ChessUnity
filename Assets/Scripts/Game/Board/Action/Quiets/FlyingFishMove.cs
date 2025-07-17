using Game.Board.Action.Internal;
using Game.Board.Effects.Debuffs;
using Game.Board.General;

namespace Game.Board.Action.Quiets
{
    public class FlyingFishMove: Action, IQuiets
    {
        public FlyingFishMove(int caller, int to) : base(caller, true)
        {
            From = (ushort)caller;
            To = (ushort)to;
        }

        public override void ApplyAction(GameState state)
        {
            MatchManager.PieceManager.Move(Caller, To);
            ModifyGameState(state);
        }

        public override void ModifyGameState(GameState state)
        {
            var maxLen = MatchManager.MaxLength;
            
            var rankFrom = From / maxLen;
            var fileFrom = From % maxLen;
            var rankTo = To / maxLen;
            var fileTo = To % maxLen;
            var board = MatchManager.GameState.MainBoard;
            var caller = board[From];

            var rankDir = rankTo == rankFrom ? 0 : rankTo > rankFrom ? 1 : -1;
            var fileDir = fileTo == fileFrom ? 0 : fileTo > fileFrom ? 1 : -1;

            while (rankFrom != rankTo || fileFrom != fileTo)
            {
                rankFrom += rankDir;
                fileFrom += fileDir;

                var p = board[rankFrom * maxLen + fileFrom];
                if (p == null || p.color == caller.color) continue;
                
                ActionManager.EnqueueAction(new ApplyEffect(new Slow(1, 1, p)));
                break;
            }
            
            MatchManager.GameState.Move(From, To);
        }
    }
}