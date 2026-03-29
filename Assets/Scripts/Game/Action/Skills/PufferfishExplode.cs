using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class PufferfishExplode : Action, ISkills
    {
        [MemoryPackConstructor]
        private PufferfishExplode()
        {
        }

        public PufferfishExplode(int maker) : base(maker)
        {
        }

        public int AIPenaltyValue(PieceLogic p)
        {
            return 0;
        }

        protected override void Animate()
        {
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new KillPiece(GetFrom()));
            var (rank, file) = RankFileOf(GetFrom());
            var caller = GetMaker() as PieceLogic;

            for (var i = -1; i <= 1; i++)
            {
                if (!VerifyBounds(rank + i)) continue;
                for (var j = -1; j <= 1; j++)
                {
                    if (!VerifyBounds(file + j)) continue;

                    var idx = IndexOf(rank + i, file + j);

                    var p = PieceOn(idx);

                    if (p != null && p.Color != caller.Color)
                        ActionManager.EnqueueAction(new ApplyEffect(new Poison(1, p), GetMaker() as PieceLogic));
                }
            }
        }
    }
}