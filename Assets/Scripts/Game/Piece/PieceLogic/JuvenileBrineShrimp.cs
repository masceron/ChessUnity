using Game.Action;
using Game.Action.Internal;
using Game.Action.Internal.Pending.Piece;
using Game.Effects.Buffs;
using Game.Effects.SpecialAbility;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
using UnityEngine;
using Game.Common;

namespace Game.Piece.PieceLogic
{
    public class JuvenileBrineShrimp : Commons.PieceLogic
    {
        private const int Strength1 = 1;
        private const int Strength2 = 1;
        private const int Duration1 = 10;
        private const int Duration2 = 10;
        public JuvenileBrineShrimp(PieceConfig cfg) : base(cfg, SmallChargingMoves.Quiets, None.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Haste(Duration1, Strength1, this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new LongReach(this, Duration2, Strength2)));
        }
    }
}