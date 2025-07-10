namespace Core.Effect
{
    public class Slow: Effect
    {
        public Slow(sbyte d) : base(d, 1)
        {}

        public override void Destruct()
        {
        }
    }
}