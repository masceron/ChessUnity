using Game.Action;
using Game.Action.Internal;
using Game.Effects.Traits;
using Game.Movesets;

namespace Game.Piece.PieceLogic.Elites
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class MorayEel: PieceLogic
    {
        public MorayEel(PieceConfig cfg) : base(cfg, MorayEelMoves.Quiets, QueenMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Ambush(this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new MorayEelCamouflage(this)));
        }
    }
}