using System.Collections.Generic;
using Game.Common;
using Game.Piece;
using Game.Piece.PieceLogic.Commons;
using ZLinq;

namespace Game.Action.Internal
{
    public class Resurrect : Action, IInternal
    {
        private readonly PieceConfig _toResurrect;
        private readonly System.Action<PieceLogic>[] _onSpawned;

        public Resurrect(Entity maker, PieceConfig toResurrect, params System.Action<PieceLogic>[] onSpawned) : base(
            maker, toResurrect.Index)
        {
            _toResurrect = toResurrect;
            _onSpawned = onSpawned;
        }

        protected override void ModifyGameState()
        {
            var allActions = _onSpawned.Prepend(logic =>
            {
                var capList = BoardUtils.GetCapturedOf(logic.Color);
                ((List<PieceConfig>)capList).Remove(capList.First(p => p.Type == _toResurrect.Type));
            }).ToArray();

            ActionManager.EnqueueAction(new SpawnPiece(_toResurrect, allActions));
        }
    }
}