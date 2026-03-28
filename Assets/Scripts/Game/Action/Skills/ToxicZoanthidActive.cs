using System.Collections.Generic;
using Game.Piece.PieceLogic.Commons;
using Game.Tile;
using MemoryPack;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class ToxicZoanthidActive : Action, ISkills
    {
        [MemoryPackInclude] private List<int> _targetPositions;

        [MemoryPackConstructor]
        private ToxicZoanthidActive()
        {
        }

        public ToxicZoanthidActive(int maker, List<int> targetPositions) : base(maker)
        {
            Maker = maker;
            _targetPositions = targetPositions;
        }

        public int AIPenaltyValue(PieceLogic maker)
        {
            return 0;
        }

        protected override void ModifyGameState()
        {
            var makerPiece = PieceOn(Maker);

            foreach (var pos in _targetPositions)
            {
                if (!VerifyIndex(pos) || !IsActive(pos)) continue;

                Formation anoxicPool = new AnoxicPool(makerPiece.Color);
                SetFormation(pos, anoxicPool);
            }

            SetCooldown(Maker, ((IPieceWithSkill)makerPiece).TimeToCooldown);
        }
    }
}
