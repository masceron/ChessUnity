using Game.Board.Action.Internal;
using Game.Board.Effects.Debuffs;
using Game.Board.General;
using static Game.Common.BoardUtils;

namespace Game.Board.Action.Skills
{
    public class PufferfishExplode: Action, ISkills
    {
        public PufferfishExplode(int caller) : base(caller, true)
        {
            To = (ushort)caller;
        }

        protected override void Animate()
        {
            
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new DestroyPiece(Caller));
            var (rank, file) = RankFileOf(Caller);
            var caller = MatchManager.Ins.GameState.PieceBoard[Caller];

            for (var i = -1; i <= 1; i++)
            {
                if (!VerifyBounds(rank + i)) continue;
                for (var j = -1; j <= 1; j++)
                {
                    if (!VerifyBounds(file + j)) continue;

                    var idx = IndexOf(rank + i, file + j);

                    var p = MatchManager.Ins.GameState.PieceBoard[idx];

                    if (p != null && p.Color != caller.Color)
                    {
                        ActionManager.EnqueueAction(new ApplyEffect(new Poison(1, p)));
                    }
                }
            }
        }
    }
}