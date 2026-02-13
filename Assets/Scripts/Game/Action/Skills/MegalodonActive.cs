using MemoryPack;
using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [MemoryPackable]
    public partial class MegalodonActive : Action, ISkills
    {
        [MemoryPackConstructor]
        private MegalodonActive() { }

        [MemoryPackInclude]
        private int _firstTargetPos;
        [MemoryPackInclude]
        private int _secondTargetPos;
        
        public MegalodonActive(int maker, int firstTargetPos, int secondTargetPos) : base(maker)
        {
            Maker = maker;
            _firstTargetPos = firstTargetPos;
            _secondTargetPos = secondTargetPos;
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new KillPiece(_firstTargetPos));
            ActionManager.EnqueueAction(new KillPiece(_secondTargetPos));
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }

        public int AIPenaltyValue(PieceLogic maker)
        {
            throw new System.NotImplementedException();
        }
    }
}