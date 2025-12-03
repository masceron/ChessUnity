using Game.Managers;
using static Game.Common.BoardUtils;
namespace Game.Effects.RegionalEffect
{
    public class BenthicStorm : RegionalEffect
    {
        private const int TurnToActive = 10;
        private int numTurns;
        
        private int startingSizeX;
        private int startingSizeY;
            
        public BenthicStorm() : base(RegionalEffectType.BenthicStorm)
        {
            numTurns = 0;
            startingSizeX = (MaxLength - MatchManager.Ins.StartingSize.x) / 2;
            startingSizeY = (MaxLength - MatchManager.Ins.StartingSize.y) / 2;
        }

        protected override void ApplyEffect(int currentTurn)
        {
            if (numTurns == TurnToActive)
            {
                
                numTurns = 0;
            }

            numTurns++;
        }
    }
}