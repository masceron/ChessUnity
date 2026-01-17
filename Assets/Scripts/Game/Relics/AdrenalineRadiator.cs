using System.Collections.Generic;
using System.Linq;
using Game.Action.Internal.Pending;
using Game.Action.Internal.Pending.Relic;
using Game.Common;
using Game.Effects;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Relics.Commons;
using UX.UI.Ingame;

namespace Game.Relics
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class AdrenalineRadiator : RelicLogic
    {
        public AdrenalineRadiator(RelicConfig cfg) : base(cfg)
        {
            CurrentCooldown = 0;
        }

        public override void Activate()
        {
            if (CurrentCooldown == 0)
            {
                foreach (var piece in MatchManager.Ins.GameState.PieceBoard)
                {
                    if (piece == null || piece.Color != Color) continue;
                    // Can kiem tra phan san cua quan dich
                    TileManager.Ins.MarkAsMoveable(piece.Pos);
                    var pending = new AdrenalineRadiatorPending(this, piece.Pos);
                    BoardViewer.ListOf.Add(pending);
                }
                BoardViewer.Selecting = -2;
                BoardViewer.SelectingFunction = 4;
            }
        }

        public override void ActiveForAI()
        {
            
        }
    }
}