using Game.Piece.PieceLogic.Commons;
using MemoryPack;

namespace Game.Action.Skills
{
    public class PorcupineCrabActive : Action, ISkills
    {
        [MemoryPackConstructor]
        private PorcupineCrabActive()
        {
        }
        
        public PorcupineCrabActive(int maker, int target) : base(maker)
        {
            Maker = maker;
            Target = target;
        }   
        
        protected override void ModifyGameState()
        {
            throw new System.NotImplementedException();
        }

        public int AIPenaltyValue(PieceLogic maker)
        {
            throw new System.NotImplementedException();
        }
    }
}