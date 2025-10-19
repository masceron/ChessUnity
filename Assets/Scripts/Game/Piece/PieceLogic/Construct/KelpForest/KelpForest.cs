using Game.Action;
using Game.Action.Internal;

namespace Game.Piece.PieceLogic.Construct.KelpForest
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class KelpForest : PieceLogic
    {
        public KelpForest(PieceConfig cfg) : base(cfg)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new KelpForestPassive(this, 6)));
        }
    }
}