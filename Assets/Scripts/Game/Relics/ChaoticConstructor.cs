using System.Collections.Generic;
using Game.Action.Internal.Pending.Relic;
using Game.Common;
using Game.Managers;
using Game.Piece;
using Game.Piece.PieceLogic.Commons;
using Game.Relics.Commons;
using UnityEngine;
using UX.UI.Ingame;

namespace Game.Relics
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ChaoticConstructor : RelicLogic
    {
        public ChaoticConstructor(RelicConfig config) : base(config)
        {
            type = config.Type;
            Color = config.Color;
            TimeCooldown = 2;
            CurrentCooldown = 0;
        }
        public override void Activate()
        {
            if (CurrentCooldown == 0)
            {
                for (int i = 0; i < BoardUtils.BoardSize; ++i)
                {
                    var piece = BoardUtils.PieceOn(i);
                    if (piece == null) continue;
                    Debug.Log(piece.Type);
                    TileManager.Ins.MarkAsMoveable(piece.Pos);
                    var pending = new ChaoticConstructorPending(Color, piece.Pos, this);
                    BoardViewer.ListOf.Add(pending);

                    BoardViewer.Selecting = -2;
                    BoardViewer.SelectingFunction = 4;
                }
            }
        }

        public override void ActiveForAI()
        {

        }
    }
}