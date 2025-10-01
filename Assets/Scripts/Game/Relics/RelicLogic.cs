namespace Game.Relics
{
    public enum RelicType
    {
        CommonPearl, BlackPearl, EyeOfMimic
    }
    
    public abstract class RelicLogic
    {
        protected RelicType Type;
        protected sbyte TimeCooldown;
        protected sbyte currentCooldown;
        protected bool Color; // true for white, false for black

        protected RelicLogic(RelicConfig cfg)
        {
            Type = cfg.Type;
            Color = cfg.Color;
            currentCooldown = 0;
        }

        public abstract void Activate();
        public void PassTurn()
        {
            if (currentCooldown > 0) currentCooldown--;
        }
    }
}