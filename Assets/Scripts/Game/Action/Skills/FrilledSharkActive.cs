using Game.Action.Internal;
using Game.Action.Quiets;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class FrilledSharkActive : Action, ISkills
    {
        private readonly int dfile;

        private readonly int drank;

        [MemoryPackConstructor]
        private FrilledSharkActive()
        {
        }

        public FrilledSharkActive(int from, int drank, int dfile) : base(from,
            IndexOf(RankOf(from) + drank * 4, FileOf(from) + dfile * 4))
        {
            this.dfile = dfile;
            this.drank = drank;
        }

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            var maker = GetMaker();
            if (maker == null) return 0;
            return pieceAI.Color != maker.Color ? 20 : 0;
        }

        protected override void ModifyGameState()
        {
            var rank = RankOf(GetFrom());
            var file = FileOf(GetFrom());
            for (var i = 0; i < 4; ++i)
            {
                rank += drank;
                file += dfile;
                var pieceOn = PieceOn(IndexOf(rank, file));
                if (pieceOn != null && pieceOn.Color != GetMaker().Color)
                    ActionManager.EnqueueAction(new ApplyEffect(new Fear(2, pieceOn), GetMaker()));
            }

            ActionManager.EnqueueAction(new NormalMove(GetFrom(), GetTargetPos()));
        }
    }
}