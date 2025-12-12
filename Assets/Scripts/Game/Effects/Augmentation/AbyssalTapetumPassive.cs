using System.Collections.Generic;
using Game.Action;
using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
namespace Game.Effects.Augmentation
{
    public class AbyssalTapetumPassive : Effect
    {
        public AbyssalTapetumPassive(PieceLogic piece) : base(-1, 1, piece, "effect_abyssal_tapetum_passive")
        {
            
        }

    }
}