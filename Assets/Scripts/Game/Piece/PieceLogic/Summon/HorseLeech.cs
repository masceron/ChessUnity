using Game.Action;
using Game.Action.Internal;
using Game.Effects.Buffs;
using Game.Movesets;

namespace Game.Piece.PieceLogic.Summon
{
    public class HorseLeech : PieceLogic
    {
        public HorseLeech(PieceConfig cfg) : base(cfg, KingMoves.Quiets, HorseLeechMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Piercing(-1, this)));

        }

    }
}