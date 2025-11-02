using Game.Action;
using Game.Action.Internal;
using Game.Effects.Condition;
using Game.Effects.RegionalEffect;
using Game.Effects.Traits;
using Game.Movesets;
using UnityEngine;

namespace Game.Piece.PieceLogic.Swarm
{
    public class Lizardfish : PieceLogic
    {
        public Lizardfish(PieceConfig cfg) : base(cfg, SmallPredatorMoves.Quiets, SmallPredatorMoves.Captures)
        {
            // đúng ra là phải dùng type Benthic storm nhưng chưa có nên để tạm là Whirpoll để test
            ActionManager.ExecuteImmediately(new ApplyEffect(new NativeGround(this, RegionalEffectType.Whirpool)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new Ambush(this)));
        }
    }
}

