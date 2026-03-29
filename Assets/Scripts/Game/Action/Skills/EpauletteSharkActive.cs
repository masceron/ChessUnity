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

        public EpauletteSharkActive(int maker, int target) : base(maker, target)
        {
        }

        public int AIPenaltyValue(PieceLogic maker)
        {
            throw new NotImplementedException();
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new KillPiece(GetTargetPos()));
            SetCooldown(GetMaker() as PieceLogic, ((IPieceWithSkill)GetMaker() as PieceLogic).TimeToCooldown);
        }
    }
}