using Game.Action.Relics;
using Game.Managers;
using Game.Relics.Commons;
using UX.UI.Ingame;

namespace Game.Relics
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
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
                // foreach (var piece in BoardUtils.PieceBoard())
                // {
                //     if (piece == null && piece.Color != Color) continue;
                //     TileManager.Ins.MarkAsMoveable(piece.Pos);
                //     var pending = new PeaceTreatyPending(this, piece.Pos);
                //     BoardViewer.ListOf.Add(pending);
                // }
                // BoardViewer.Selecting = -2;
                // BoardViewer.SelectingFunction = 4;
                var excute = new PeaceTreatyExecute(Color);
                BoardViewer.Ins.ExecuteAction(excute);
                SetCooldown();
                MatchManager.Ins.InputProcessor.UpdateRelic();
            }
        }

        public override void ActiveForAI()
        {
        }
    }
}