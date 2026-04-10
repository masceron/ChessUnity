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
    public partial class LionfishActive : Action, ISkills, ILocaltionTarget
    {
        [MemoryPackConstructor]
        private LionfishActive()
        {
        }

        public LionfishActive(PieceLogic maker) : base(maker)
        {
        }

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            var maker = GetMakerAsPiece();
            if (maker == null) return 0;
            return pieceAI.Color != maker.Color ? -20 : 0;
        }

        protected override void ModifyGameState()
        {
            var (rank, file) = RankFileOf(GetFrom());
            var caller = GetMakerAsPiece();

            for (var rankOff = rank - 1; rankOff <= rank + 1; rankOff++)
            {
                if (!VerifyBounds(rankOff)) continue;

                for (var fileOff = file - 1; fileOff <= file + 1; fileOff++)
                {
                    if (rankOff == rank && fileOff == file) continue;
                    var target = IndexOf(rankOff, fileOff);
                    var pieceTarget = PieceOn(target);
                    if (pieceTarget == null || pieceTarget.Color == caller.Color) continue;

                    ActionManager.EnqueueAction(new LionfishActiveImpact(Maker, target));
                }
            }

            ActionManager.EnqueueAction(new CooldownSkill(GetMakerAsPiece()));
        }
    }
}