using System.Collections.Generic;
using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Action.Quiets;
using Game.Common;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using static Game.Common.BoardUtils;

namespace Game.Effects.SpecialAbility
{
    public class PlumedSeaFirPassive : Effect
    {
        public PlumedSeaFirPassive(PieceLogic piece) : base(-1, 1, piece, "effect_plumed_sea_fir_passive")
        {
            SetStat(EffectStat.Counter, 0);
        }
    }
}