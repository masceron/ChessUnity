namespace Game.Relics
{
    public enum RelicType
    {
        CommonPearl, BlackPearl
    }
    
    public abstract class RelicLogic
    {
        public readonly RelicType Type;

        public abstract void Activate();
    }
}