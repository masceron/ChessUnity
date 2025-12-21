namespace Game.Relics.Commons
{
    public abstract class RelicLogic
    {
        public string type;
        protected sbyte TimeCooldown;
        public sbyte currentCooldown { get; protected set; }
        public bool Color; // false for white, true for black

        protected RelicLogic(RelicConfig cfg)
        {
            Color = cfg.Color;
            TimeCooldown = cfg.TimeCooldown;
            currentCooldown = 0;
        }

        public abstract void Activate();

        public void SetCooldown()
        {
            currentCooldown = TimeCooldown;
        }

        public void PassTurn()
        {
            if (currentCooldown > 0) currentCooldown--;
        }

        public virtual void ActiveForAI()
        {
            
        }
    }

    public static class RelicMaker
    {
        public static RelicLogic Get(RelicConfig cfg)
        {
            return RelicFactory.CreateLogicInstance(cfg.Type, cfg);
        }
    }
}