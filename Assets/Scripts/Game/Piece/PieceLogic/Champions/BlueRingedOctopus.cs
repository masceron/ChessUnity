
using Game.Action;
using Game.Action.Internal;
using Game.Effects.Buffs;
using Game.Effects.Traits;
using Game.Movesets;

namespace Game.Piece.PieceLogic.Champions
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class BlueRingedOctopus: PieceLogic
    {
        public BlueRingedOctopus(PieceConfig cfg) : base(cfg, QueenMoves.Quiets, RookMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Evasion(-1, 50, this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new Camouflage(this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new BlueRingedOctopusPassive(this)));

        }

    }
}