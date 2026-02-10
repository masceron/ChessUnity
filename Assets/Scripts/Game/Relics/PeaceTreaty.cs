using Game.Relics.Commons;
using UX.UI.Ingame;
using Game.Managers;
using Game.Action.Relics;

namespace Game.Relics
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class PeaceTreaty : RelicLogic
    {
        public PeaceTreaty(RelicConfig cfg) : base(cfg)
        {
            TimeCooldown = cfg.TimeCooldown;
            CurrentCooldown = 0;
        }

        public override void Activate()
        {
            if (CurrentCooldown == 0)
            {
                // foreach (var piece in MatchManager.Ins.GameState.PieceBoard)
                // {
                //     if (piece == null && piece.Color != Color) continue;
                //     TileManager.Ins.MarkAsMoveable(piece.Pos);
                //     var pending = new PeaceTreatyPending(this, piece.Pos);
                //     BoardViewer.ListOf.Add(pending);
                // }
                // BoardViewer.Selecting = -2;
                // BoardViewer.SelectingFunction = 4;
                var excute = new PeaceTreatyExcute(Color);
                BoardViewer.Ins.ExecuteAction(excute);
                MatchManager.Ins.InputProcessor.UpdateRelic();
                SetCooldown();
            }
        }

        public override void ActiveForAI()
        {
        }
    }
}