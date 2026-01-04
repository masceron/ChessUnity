using Game.Action;
using Game.Action.Internal;
using Game.Effects.Buffs;
using Game.Effects.Traits;
using Game.Movesets;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class BlueRingedOctopus: Commons.PieceLogic
    {
        public BlueRingedOctopus(PieceConfig cfg) : base(cfg, QueenMoves.Quiets, RookMoves.Captures)
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Evasion(-1, 50, this)));
            ActionManager.EnqueueAction(new ApplyEffect(new Camouflage(this)));
            ActionManager.EnqueueAction(new ApplyEffect(new BlueRingedOctopusPassive(this)));

        }

    }
}