using Game.Board.Action.Internal;
using Game.Board.Effects.Debuffs;
using static Game.Common.BoardUtils;

namespace Game.Board.Action.Skills
{
    public class PufferfishExplode: Action, ISkills
    {
        public PufferfishExplode(int from) : base(from, true)
        {
            To = (ushort)from;
        }

        protected override void Animate()
        {
            
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new DestroyPiece(From));
            var (rank, file) = RankFileOf(From);
            var caller = PieceOn(From);

            for (var i = -1; i <= 1; i++)
            {
                if (!VerifyBounds(rank + i)) continue;
                for (var j = -1; j <= 1; j++)
                {
                    if (!VerifyBounds(file + j)) continue;

                    var idx = IndexOf(rank + i, file + j);

                    var p = PieceOn(idx);

                    if (p != null && p.Color != caller.Color)
                    {
                        ActionManager.EnqueueAction(new ApplyEffect(new Poison(1, p)));
                    }
                }
            }
        }
    }
}