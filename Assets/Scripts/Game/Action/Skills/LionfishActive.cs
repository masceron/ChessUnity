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
    public partial class LionfishActive : Action, ISkills
    {
        [MemoryPackConstructor]
        private LionfishActive()
        {
        }

        public LionfishActive(int maker) : base(maker)
        {
        }

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            var maker = GetMaker() as PieceLogic;
            if (maker == null) return 0;
            return pieceAI.Color != maker.Color ? -20 : 0;
        }

        protected override void ModifyGameState()
        {
            var (rank, file) = RankFileOf(GetFrom());
            var caller = GetMaker() as PieceLogic;

            for (var rankOff = rank - 1; rankOff <= rank + 1; rankOff++)
            {
                if (!VerifyBounds(rankOff)) continue;

                for (var fileOff = file - 1; fileOff <= file + 1; fileOff++)
                {
                    if (rankOff == rank && fileOff == file) continue;
                    var p = PieceOn(IndexOf(rankOff, fileOff));
                    if (p == null || p.Color == caller.Color) continue;

                    ActionManager.EnqueueAction(new ApplyEffect(new Poison(1, p), GetMaker() as PieceLogic));
                }
            }

            SetCooldown(GetMaker() as PieceLogic, ((IPieceWithSkill)GetMaker()).TimeToCooldown);
        }
    }
}