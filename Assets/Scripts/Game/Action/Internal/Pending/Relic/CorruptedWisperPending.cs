using Game.Action.Quiets;
using Game.Action.Relics;
using Game.Common;
using Game.Effects;
using Game.Effects.Debuffs;
using Game.Managers;
using Game.Piece;
using Game.Piece.PieceLogic.Commons;
using Game.Relics;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UX.UI.Ingame;
using static Game.Common.BoardUtils;

namespace Game.Action.Internal.Pending.Relic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]

    public class CorruptedWisperPending : PendingAction, System.IDisposable
    {
        private CorruptedWisper corruptedWisper;

        public CorruptedWisperPending(int target, CorruptedWisper corruptedWisper) : base(target)
        {
            Target = (ushort)target;
            this.corruptedWisper = corruptedWisper;
        }


        public override void CompleteAction()
        {
            var execute = new CorruptedWisperExecute(-1, Target);

            BoardViewer.Ins.ExecuteAction(execute);
            corruptedWisper.LevelUp();
            TileManager.Ins.UnmarkAll();
            BoardViewer.Selecting = -1;
            BoardViewer.SelectingFunction = 0;
            MatchManager.Ins.InputProcessor.UpdateRelic();
        }

        public void Dispose()
        {
            BoardViewer.SelectingFunction = 0;
        }

    }
}

