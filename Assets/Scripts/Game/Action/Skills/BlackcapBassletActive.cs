using Game.Action.Internal;
using MemoryPack;
using Game.Action.Quiets;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [MemoryPackable]
    public partial class BlackcapBassletActive : Action, ISkills
    {
        [MemoryPackConstructor]
        private BlackcapBassletActive() { }

        public BlackcapBassletActive(PieceLogic maker, int target) : base(maker, target)
        {
        }
        
        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new NormalMove(GetMakerAsPiece(), GetTargetPos()));
            ActionManager.EnqueueAction(new CooldownSkill(GetMakerAsPiece()));
        }

        public int AIPenaltyValue(PieceLogic maker)
        {
            throw new System.NotImplementedException();
        }
    }
}