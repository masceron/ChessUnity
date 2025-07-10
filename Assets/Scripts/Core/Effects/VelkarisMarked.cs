using Core.Piece;

namespace Core.Effect
{
    public class VelkarisMarked: Effect
    {
        private readonly Velkaris marker;
        public VelkarisMarked(sbyte d, byte s, Velkaris marker) : base(d, s)
        {
            this.marker = marker;
        }

        public override void Destruct()
        {
            marker.SkillCooldown = 0;
            marker.Marked = null;
        }
    }
}