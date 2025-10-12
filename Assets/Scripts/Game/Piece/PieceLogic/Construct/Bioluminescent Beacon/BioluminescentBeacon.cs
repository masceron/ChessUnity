
using Game.Action;
using Game.Action.Internal;

namespace Game.Piece.PieceLogic.Construct
{
    /// <summary>
    /// Bioluminescent Beacon Construct
    /// </summary>
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]

    public class BioluminescentBeacon : PieceLogic
    {
        public BioluminescentBeacon(PieceConfig cfg) : base(cfg, null, null)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new BioluminescentBeaconPassive(this)));
        }
    }

}
