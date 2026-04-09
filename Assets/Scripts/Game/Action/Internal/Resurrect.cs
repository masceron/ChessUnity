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
            var allActions = _onSpawned.Prepend(logic => {
                ((List<PieceConfig>)BoardUtils.GetCapturedOf(logic.Color)).Remove(_toResurrect);
            }).ToArray();

            ActionManager.EnqueueAction(new SpawnPiece(_toResurrect, allActions));
        }
    }
}