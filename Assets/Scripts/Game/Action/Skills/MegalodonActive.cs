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
        [MemoryPackInclude] private int _firstTargetId;

        [MemoryPackInclude] private int _secondTargetId;

        [MemoryPackConstructor]
        private MegalodonActive()
        {
        }

        public MegalodonActive(PieceLogic maker, int firstTargetId, int secondTargetId) : base(maker)
        {
            _firstTargetId = firstTargetId;
            _secondTargetId = secondTargetId;
        }

        public int AIPenaltyValue(PieceLogic maker)
        {
            throw new NotImplementedException();
        }

        protected override void ModifyGameState()
        {
            var firstTarget = GetEntityByID(_firstTargetId) as PieceLogic;
            var secondTarget = GetEntityByID(_secondTargetId) as PieceLogic;
            if (firstTarget == null || secondTarget == null) return;

            ActionManager.EnqueueAction(new KillPiece(GetMaker(), firstTarget));
            ActionManager.EnqueueAction(new KillPiece(GetMaker(), secondTarget));
            ActionManager.EnqueueAction(new CooldownSkill(GetMakerAsPiece()));
        }
    }
}