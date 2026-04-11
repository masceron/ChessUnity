using Game.Action.Relics;
using Game.Common;
using Game.Managers;
using Game.Relics.Commons;
using UX.UI.Ingame;

namespace Game.Relics
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class MethaneCasing : RelicLogic
    {
        public MethaneCasing(RelicConfig cfg) : base(cfg)
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
                    //Làm lại
                    //var pending = new MethaneCasingPending(this, piece);
                   BoardViewer.ListOf.Add(new MethaneCasingExecute(piece));
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