using Game.Action;
using Game.Action.Internal;
using Game.Effects.Traits;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]

    public class PollutedRock : Commons.PieceLogic
    {
        public PollutedRock(PieceConfig cfg) : base(cfg)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new PollutedRockPassive(this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new Construct(this)));
        }
    }

}