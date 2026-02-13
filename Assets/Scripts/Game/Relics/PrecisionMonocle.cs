using Game.Relics.Commons;
using UX.UI.Ingame;

namespace Game.Relics
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class PrecisionMonocle : RelicLogic
    {
        public PrecisionMonocle(RelicConfig cfg) : base(cfg)
        {
            TimeCooldown = cfg.TimeCooldown;
            CurrentCooldown = 0;
        }

        public override void Activate()
        {
            if (CurrentCooldown == 0)
            {
                // TileManager.Ins.MarkAsMoveable();
                // var pending = new PrecisionMonoclePending(this, piece.Pos);
                // BoardViewer.ListOf.Add(pending);
                //TODO: Find Maker
                BoardViewer.Selecting = -2;
                BoardViewer.SelectingFunction = 4;
            }
        }
    }
}