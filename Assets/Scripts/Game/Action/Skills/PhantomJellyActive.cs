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
    public partial class PhantomJellyActive : Action, ISkills
    {
        [MemoryPackConstructor]
        private PhantomJellyActive()
        {
        }

        public PhantomJellyActive(int maker) : base(maker)
        {
        }

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            var maker = GetMaker() as PieceLogic;
            if (maker == null || pieceAI == null) return 0;
            if (pieceAI.Color != maker.Color) return -30;
            return 0;
        }

        protected override void Animate()
        {
        }

        protected override void ModifyGameState()
        {
            var (rank, file) = RankFileOf(GetMakerPos());
            var caller = GetMaker() as PieceLogic;

            for (var i = -3; i <= 3; i++)
            {
                if (!VerifyBounds(rank + i)) continue;
                for (var j = -3; j <= 3; j++)
                {
                    if (!VerifyBounds(file + j)) continue;

                    var idx = IndexOf(rank + i, file + j);

                    var p = PieceOn(idx);

                    if (p != null && p.Color != caller.Color)
                        ActionManager.EnqueueAction(new ApplyEffect(new Fear(-1, p), GetMaker() as PieceLogic));
                }
            }
        }
    }
}