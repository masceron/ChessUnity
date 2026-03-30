using System;
using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [MemoryPackable]
    public partial class EpauletteSharkActive : Action, ISkills
    {
        [MemoryPackConstructor]
        private EpauletteSharkActive()
        {
        }

        public EpauletteSharkActive(PieceLogic maker, PieceLogic target) : base(maker, target)
        {
        }

        public int AIPenaltyValue(PieceLogic maker)
        {
            throw new NotImplementedException();
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new KillPiece(GetTarget() as PieceLogic));
            SetCooldown(GetMaker() as PieceLogic, ((IPieceWithSkill)GetMaker()).TimeToCooldown);
        }
    }
}