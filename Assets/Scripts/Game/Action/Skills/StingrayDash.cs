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
    public partial class StingrayDash : Action, ISkills
    {
        [MemoryPackConstructor]
        private StingrayDash()
        {
        }

        public StingrayDash(int maker, int to) : base((PieceLogic)maker, (PieceLogic)to)
        {
        }

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            var maker = GetMakerAsPiece();
            if (maker == null) return 0;
            return pieceAI.Color != maker.Color ? -30 : 0;
        }


        protected override void ModifyGameState()
        {
            var (rankFrom, fileFrom) = RankFileOf(GetFrom());
            var (rankTo, fileTo) = RankFileOf(GetTargetPos());
            var board = PieceBoard();
            var caller = GetMakerAsPiece();

            var rankDir = rankTo == rankFrom ? 0 : rankTo > rankFrom ? 1 : -1;
            var fileDir = fileTo == fileFrom ? 0 : fileTo > fileFrom ? 1 : -1;

            while (rankFrom != rankTo || fileFrom != fileTo)
            {
                rankFrom += rankDir;
                fileFrom += fileDir;

                var p = board[IndexOf(rankFrom, fileFrom)];
                if (p == null || p.Color == caller.Color) continue;

                ActionManager.EnqueueAction(new ApplyEffect(new Slow(1, 1, p), caller));
                ActionManager.EnqueueAction(new ApplyEffect(new Poison(1, p), caller));
            }

            Move(caller, GetTargetPos());
            SetCooldown(caller, ((IPieceWithSkill)caller).TimeToCooldown);
        }
    }
}