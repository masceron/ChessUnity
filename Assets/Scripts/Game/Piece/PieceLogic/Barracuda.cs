using Game.Action;
using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Effects.Traits;
using Game.Movesets;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Barracuda: Commons.PieceLogic
    {
        public Barracuda(PieceConfig cfg) : base(cfg, BarracudaMoves.Quiets, BarracudaMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Evasion(-1, 25, this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new Surpass(this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new Ambush(this)));
        }
    }
}