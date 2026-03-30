using System;
using Game.Piece.PieceLogic.Commons;
using Game.Tile;
using MemoryPack;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [MemoryPackable]
    public partial class PencilUrchinActive : Action, ISkills
    {
        [MemoryPackConstructor]
        private PencilUrchinActive()
        {
        }

        public PencilUrchinActive(PieceLogic maker, int target) : base(maker, target)
        {
        }

        public int AIPenaltyValue(PieceLogic maker)
        {
            throw new NotImplementedException();
        }

        protected override void ModifyGameState()
        {
            SetFormation(GetTargetPos(), new UrchinField(false, GetMakerAsPiece().Color));
            SetCooldown(GetMakerAsPiece(), ((IPieceWithSkill)GetMakerAsPiece()).TimeToCooldown);
        }
    }
}