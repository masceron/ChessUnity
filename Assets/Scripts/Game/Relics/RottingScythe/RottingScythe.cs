namespace Game.Relics.RottingScythe
{
    public class RottingScythe : RelicLogic
    {
        public RottingScythe(RelicConfig cfg) : base(cfg)
        {
            Type = cfg.Type;
            Color = cfg.Color;
            TimeCooldown = cfg.TimeCooldown;
            currentCooldown = 0;
        }

        public override void Activate()
        {
            if (currentCooldown == 0)
            {
                
            }
        }
    }
}