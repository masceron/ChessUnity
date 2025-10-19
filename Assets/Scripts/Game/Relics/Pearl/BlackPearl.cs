using Game.Managers;
using UX.UI.Ingame;
using Game.Action.Relics;
namespace Game.Relics.Pearl
{
    public class BlackPearl : RelicLogic, IRelicAction
    {
        public BlackPearl(RelicConfig cfg) : base(cfg)
        {
            TimeCooldown = cfg.TimeCooldown;
            currentCooldown = 0;
        }

        public override void Activate()
        {
            if (currentCooldown == 0)
            {
                foreach (var piece in MatchManager.Ins.GameState.PieceBoard)
                {
                    if (piece == null) continue;
                    TileManager.Ins.MarkAsMoveable(piece.Pos);
                    var pending = new BlackPearlPending(this, piece.Pos, false);
                    BoardViewer.ListOf.Add(pending);
                }
                BoardViewer.Selecting = -2;
                BoardViewer.SelectingFunction = 4;
            }
        }
    }
}