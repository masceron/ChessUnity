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
        private const int Duration = 2;

        [MemoryPackInclude]
        private int dfile;

        [MemoryPackInclude]

        private int drank;

        [MemoryPackConstructor]
        private FrilledSharkActive()
        {
        }

        public FrilledSharkActive(PieceLogic from, int target, int drank, int dfile) : base(from, target)
        {
            this.dfile = dfile;
            this.drank = drank;
        }

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            if (GetMakerAsPiece() is not PieceLogic maker) return 0;
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
                if (pieceOn != null && pieceOn.Color != GetMakerAsPiece().Color)
                    ActionManager.EnqueueAction(new ApplyEffect(new Fear(Duration, pieceOn), GetMakerAsPiece()));
            }

            ActionManager.EnqueueAction(new NormalMove(GetMakerAsPiece(), GetTargetPos()));
            ActionManager.EnqueueAction(new CooldownSkill(GetMakerAsPiece()));
        }
    }
}