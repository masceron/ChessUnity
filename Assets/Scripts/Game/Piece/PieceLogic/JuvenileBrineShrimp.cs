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
            SetStat(SkillStat.Strength, Strength1);
            SetStat(SkillStat.Strength, Strength2);
            SetStat(SkillStat.Duration, Duration1);
            SetStat(SkillStat.Duration, Duration2);

            ActionManager.ExecuteImmediately(new ApplyEffect(new Haste(GetStat(SkillStat.Duration), GetStat(SkillStat.Strength), this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new LongReach(this, GetStat(SkillStat.Duration, 2), GetStat(SkillStat.Strength, 2))));
        }
    }
}