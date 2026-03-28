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

        public PencilUrchinActive(int maker, int target) : base(maker, target)
        {
            Maker = maker;
            Target = target;
        }

        public int AIPenaltyValue(PieceLogic maker)
        {
            throw new NotImplementedException();
        }

        protected override void ModifyGameState()
        {
            SetFormation(Target, new UrchinField(false, GetMaker().Color));
            SetCooldown(GetMaker(), ((IPieceWithSkill)GetMaker()).TimeToCooldown);
        }
    }
}