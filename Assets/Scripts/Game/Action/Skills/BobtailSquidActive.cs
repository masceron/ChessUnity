using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Tile;
using MemoryPack;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class BobtailSquidActive : Action, ISkills
    {
        [MemoryPackConstructor]
        private BobtailSquidActive()
        {
        }

        public BobtailSquidActive(PieceLogic maker) : base(maker)
        {
        }

        public int AIPenaltyValue(PieceLogic p)
        {
            return 0;
        }

        protected override void Animate()
        {
            // (int rank, int file) = RankFileOf(Maker);
            // PieceManager.Ins.Move(Maker, IndexOf(rank - 3, file));
        }

        protected override void ModifyGameState()
        {
            var maker = GetMakerAsPiece();
            SetCooldown(maker, ((IPieceWithSkill)maker).TimeToCooldown);
            var color = maker.Color;
            var direction = color ? -1 : +1;
            var steps = 0;
            var (oldRank, oldFile) = RankFileOf(GetFrom());

            while (IsActive(IndexOf(oldRank + (steps + 1) * direction, oldFile)) && steps < 3) steps++;
            var finalPos = IndexOf(oldRank + steps * direction, oldFile);
            Move(maker, finalPos);
            PieceManager.Ins.Move(GetFrom(), finalPos);
            for (var x = oldRank + (color ? 0 : -1); x <= oldRank + (color ? 1 : 0); ++x)
            for (var y = oldFile - 1; y <= oldFile + 1; ++y)
                if (IsActive(IndexOf(x, y)))
                {
                    Formation fogOfWar = new FogOfWar(color);
                    fogOfWar.SetDuration(3);
                    SetFormation(IndexOf(x, y), fogOfWar);
                }
        }
    }
}