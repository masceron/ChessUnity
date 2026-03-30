using System.Collections.Generic;
using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Common;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using UnityEngine;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ThreadPipefish : Commons.PieceLogic
    {
        public ThreadPipefish(PieceConfig cfg) : base(cfg, UpDoorMoves.Quiets, UpDoorMoves.Captures)
        {
         //Làm lại  
        }
    }
}