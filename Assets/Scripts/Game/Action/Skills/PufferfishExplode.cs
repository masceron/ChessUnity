using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class PufferfishExplode: Action, ISkills
    {
        public int AIPenaltyValue(PieceLogic p)
        {
            return 0;
        }
        public PufferfishExplode(int maker) : base(maker)
        {
            Target = (ushort)maker;
        }

        protected override void Animate()
        {
            
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new KillPiece(Maker));
            var (rank, file) = RankFileOf(Maker);
            var caller = PieceOn(Maker);

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
                        ActionManager.EnqueueAction(new ApplyEffect(new Poison(1, p), PieceOn(Maker)));
                    }
                }
            }
        }
    }
}