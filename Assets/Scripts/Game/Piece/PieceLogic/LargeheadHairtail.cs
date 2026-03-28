using Game.Action;
using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Effects.SpecialAbility;
using Game.Effects.Traits;
using Game.Movesets;

namespace Game.Piece.PieceLogic
{
    public class LargeheadHairtail : Commons.PieceLogic
    {
        private const int EvasionChance = 10;
        
        public LargeheadHairtail(PieceConfig cfg) : base(cfg, PufferfishMoves.Quiets, PufferfishMoves.Captures)
        { 
            ActionManager.ExecuteImmediately(new ApplyEffect(new Evasion(-1, EvasionChance, this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new LargeheadHairtailPassive(this)));
        }
    }
}