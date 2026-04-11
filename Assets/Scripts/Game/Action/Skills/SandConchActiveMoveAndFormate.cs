using MemoryPack;
using Game.Action.Internal;
using Game.Action.Quiets;
using Game.Effects.States;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Tile;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [MemoryPackable]
    public partial class SandConchActiveMoveAndFormate : Action, ISkills
    {
        [MemoryPackConstructor]
        private SandConchActiveMoveAndFormate() { }

        public SandConchActiveMoveAndFormate(PieceLogic maker, int target) : base(maker, target)
        {
        }
        
        protected override void ModifyGameState()
        {
            var maker = GetMakerAsPiece();
            var burrowed = maker.Effects.FirstOrDefault(e => e is Burrowed);
            
            ActionManager.EnqueueAction(new NormalMove(maker, GetTargetPos()));
            //TODO: Fix Burrowed to work with SandConch
            
            FormationManager.Ins.SetFormation(GetTargetPos(), new SiltCloud(false));
            if (burrowed != null) ActionManager.EnqueueAction(new ApplyEffect(new NoneState(maker)));
            ActionManager.EnqueueAction(new CooldownSkill(maker));
        }

        public int AIPenaltyValue(PieceLogic maker)
        {
            throw new System.NotImplementedException();
        }
    }
}