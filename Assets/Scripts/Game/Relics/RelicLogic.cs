namespace Game.Relics
{
    public enum RelicType
    {
        CommonPearl, BlackPearl, EyeOfMimic, FrostSigil,
        RottingScythe, SeafoamPhial
    }
    
    public abstract class RelicLogic
    {
        public RelicType Type { get; set; }  
        protected sbyte TimeCooldown;
        public sbyte currentCooldown { get; protected set; }
        public bool Color; // false for white, true for black

        protected RelicLogic(RelicConfig cfg)
        {
            Type = cfg.Type;
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
    }
}