using Game.Action;
using Game.Action.Internal;
using Game.Effects.Others;
using Game.Effects.Traits;
using Game.Movesets;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Helicoprion: Commons.PieceLogic
    {
        public Helicoprion(PieceConfig cfg): base(cfg, RookMoves.Quiets, RookMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new FreeMovement(this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new DestroyEnemyWhenMove(this, 1)));
        }
    }
}