using Game.Managers;
using Game.Tile;
using static Game.Common.BoardUtils;
namespace Game.Effects.RegionalEffect
{
    public class BenthicStorm : RegionalEffect
    {
        private int startingSizeX;
        private int startingSizeY;
        private const int TurnToActive = 10;
        private int numTurns;

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
                var idx = UnityEngine.Random.Range(IndexOf(startingSizeX, startingSizeY), 
                    IndexOf(startingSizeX + MatchManager.Ins.StartingSize.x - 1, 
                        startingSizeY + MatchManager.Ins.StartingSize.y));
        
                Formation siltCloud = new SiltCloud(false);
                for (var i = 0; i < 2; ++i)
                {
                    for (var j = 0; j < 2; ++j)
                    {
                        var newRank = i + RankOf(idx);
                        var newFile = j + FileOf(idx);

                        if (!VerifyBounds(newRank) || !VerifyBounds(newFile)) continue;
                        
                        SetFormation(IndexOf(newRank, newFile), siltCloud);
                        
                    }
                }
                numTurns = 0;
            }
            numTurns++;
        }
    }
}
