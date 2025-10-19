using Game.Action;
using Game.Action.Internal;

namespace Game.Piece.PieceLogic.Construct.PollutedRock
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]

    public class PollutedRock : PieceLogic
    {
        public PollutedRock(PieceConfig cfg) : base(cfg, null, null)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new PollutedRockPassive(this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new Effects.Traits.Construct(this)));
        }
    }

}