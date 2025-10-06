using Game.Action;
using Game.Action.Internal;
using Game.Effects.Buffs;
using Game.Effects.Condition;
using Game.Movesets;

namespace Game.Piece.PieceLogic.Elites
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ChamberedNautilus : PieceLogic
    {
        public ChamberedNautilus(PieceConfig cfg) : base(cfg, BishopMoves.Quiets, BarracudaMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new ChamberedNautilusHunger(this)));
        }
    }
}