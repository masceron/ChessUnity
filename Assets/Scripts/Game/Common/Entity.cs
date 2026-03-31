namespace Game.Common
{
    public abstract class Entity
    {
        public readonly int ID = BoardUtils.NextEntityID();
        public int Pos = -1;
    }
}