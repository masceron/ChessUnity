using System.Linq;
using Game.Effects;
using Game.Managers;
using UX.UI.Ingame;

namespace Game.Relics.SeafoamPhial
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SeafoamPhial : RelicLogic
    {
        public SeafoamPhial(RelicConfig cfg) : base(cfg)
        {
            currentCooldown = 0;
        }

        public override void Activate()
        {
            if (currentCooldown == 0)
            {
                foreach (var piece in MatchManager.Ins.GameState.PieceBoard)
                {
                    if (piece == null || piece.Color != Color) continue;
                    TileManager.Ins.MarkAsMoveable(piece.Pos);
                    var pending = new SeafoamPhialPending(this, piece.Pos, piece.Color);
                    BoardViewer.ListOf.Add(pending);
                }
                BoardViewer.Selecting = -2;
                BoardViewer.SelectingFunction = 4;
            }
        }
    }
}