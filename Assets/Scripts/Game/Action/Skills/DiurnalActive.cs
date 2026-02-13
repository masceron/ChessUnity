using MemoryPack;
using Game.Action.Quiets;
using Game.Piece.PieceLogic.Commons;

namespace Game.Action.Skills
{
    [MemoryPackable]
    public partial class DiurnalActive : Action, ISkills
    {
        [MemoryPackConstructor]
        private DiurnalActive() { }

        public DiurnalActive(int maker, int target) : base(maker)
        {
            Target = target;
        }

        public int AIPenaltyValue(PieceLogic maker)
        {
            throw new System.NotImplementedException();
        }

        protected override void ModifyGameState()
        {
            
            ActionManager.EnqueueAction(new NormalMove(Maker, Target));
        }
    }
}
