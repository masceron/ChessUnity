using System;
using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [MemoryPackable]
    public partial class MegalodonActive : Action, ISkills
    {
        [MemoryPackInclude] private int _firstTargetPos;

        [MemoryPackInclude] private int _secondTargetPos;

        [MemoryPackConstructor]
        private MegalodonActive()
        {
        }

        public MegalodonActive(int maker, int firstTargetPos, int secondTargetPos) : base(maker)
        {
            _firstTargetPos = firstTargetPos;
            _secondTargetPos = secondTargetPos;
        }

        public int AIPenaltyValue(PieceLogic maker)
        {
            throw new NotImplementedException();
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new KillPiece(_firstTargetPos));
            ActionManager.EnqueueAction(new KillPiece(_secondTargetPos));
            SetCooldown(GetMaker(), ((IPieceWithSkill)GetMaker()).TimeToCooldown);
        }
    }
}