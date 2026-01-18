using Game.Action.Internal.Pending.Relic;
using Game.Common;
using Game.Managers;
using Game.Relics.Commons;
using UX.UI.Ingame;

namespace Game.Relics
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class RayStinger : RelicLogic
    {
        public RayStinger(RelicConfig cfg) : base(cfg)
        {
            CurrentCooldown = 0;
        }

        public override void Activate()
        {
            if (CurrentCooldown == 0)
            {
                foreach (var piece in BoardUtils.PieceBoard())
                {
                    if (piece == null || piece.Color == Color) continue;
                    TileManager.Ins.MarkAsMoveable(piece.Pos);
                    var pending = new RayStingerActive(this, piece.Pos);
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