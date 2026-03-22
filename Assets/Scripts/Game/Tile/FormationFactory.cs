
namespace Game.Tile
{
    public static class FormationFactory
    {
        public static Formation CreateInstance(FormationType type, bool color, int duration)
        {
            Formation returnedFormation =  type switch
            {
                FormationType.AnchorIce => new AnchorIce(color),
                FormationType.BubbleVent => new BubbleVent(color),
                _ => new FogOfWar(color)
            };
            returnedFormation.SetDuration(duration);
            return returnedFormation;
        }
    }
}