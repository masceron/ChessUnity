using MemoryPack;

namespace Game.Save.Stage.Campaigns
{
    [MemoryPackable]
    public partial struct CampaignStage
    {
        public readonly GameConfig GameConfig;
        public readonly Army.Army[] EnemyConfig;

        public CampaignStage(GameConfig gameConfig, Army.Army[] enemyConfig)
        {
            GameConfig = gameConfig;
            EnemyConfig = enemyConfig;
        }
    }
}