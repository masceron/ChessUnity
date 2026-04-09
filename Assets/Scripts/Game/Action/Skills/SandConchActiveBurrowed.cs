using MemoryPack;
using Game.Action.Internal;
using Game.Effects.States;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [MemoryPackable]
    public partial class SandConchActiveBurrowed : Action, ISkills
    {
        [MemoryPackConstructor]
        private SandConchActiveBurrowed() { }

        public SandConchActiveBurrowed(PieceLogic maker) : base(maker)
        {
        }
        
        protected override void ModifyGameState()
        {
            var maker = GetMakerAsPiece();
            ActionManager.EnqueueAction(new ApplyEffect(new Burrowed(4, maker), maker));
        }

        public int AIPenaltyValue(PieceLogic maker)
        {
            throw new System.NotImplementedException();
        }
    }
}