using static Game.Common.BoardUtils;
using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;
using Game.Action.Quiets;
using Game.Effects.Debuffs;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class FrilledSharkActive: Action, ISkills
    {
        int drank, dfile;
        public FrilledSharkActive(int from, int drank, int dfile) : base(from)
        {
            this.dfile = dfile;
            this.drank = drank;
            Target = IndexOf(RankOf(Maker) + drank * 4, FileOf(Maker) + dfile * 4);
        }
        protected override void ModifyGameState()
        {
            var rank = RankOf(Maker);
            var file = FileOf(Maker);
            for (var i = 0; i < 4; ++i)
            {
                rank += drank;
                file += dfile;
                var pieceOn = PieceOn(IndexOf(rank, file));
                if (pieceOn != null && pieceOn.Color != PieceOn(Maker).Color)
                {
                    ActionManager.EnqueueAction(new ApplyEffect(new Fear(2, pieceOn), PieceOn(Maker)));
                }
            }
            ActionManager.EnqueueAction(new NormalMove(Maker, Target));
        }
        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            var maker = PieceOn(Maker);
            if (maker == null) return 0;
            return pieceAI.Color != maker.Color ? 20 : 0;
        }
    }
}