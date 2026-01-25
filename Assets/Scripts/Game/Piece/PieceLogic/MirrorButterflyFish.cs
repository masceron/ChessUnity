using Game.Action;
using Game.Action.Internal;
using Game.Effects.Buffs;
using Game.Effects.Others;
using Game.Effects.Traits;
using Game.Movesets;
using UnityEngine.Lumin;

namespace Game.Piece.PieceLogic
{
    public class MirrorButterflyFish : Commons.PieceLogic
    {
        private const int EvasionProbability = 20;
        public MirrorButterflyFish(PieceConfig cfg) : base(cfg, SmallPredatorMoves.Quiets, SmallPredatorMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Evasion(-1, EvasionProbability, this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new MirrorButterflyFishPassive(this)));
        }
    }
}