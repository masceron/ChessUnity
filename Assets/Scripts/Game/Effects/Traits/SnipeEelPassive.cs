using System.Linq;
using Game.Action;
using Game.Action.Internal;
using Game.Common;
using Game.Piece.PieceLogic;
using UnityEngine;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SnipeEelPassive : Effect
    {
        public SnipeEelPassive(PieceLogic piece) : base(-1, -1, piece, EffectName.SnipeEelPassive)
        {
            
        }

        
    }
}