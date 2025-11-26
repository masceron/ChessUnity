using System.Linq;
using Game.Action.Internal.Pending.Relic;
using Game.Managers;
using Game.Relics.Commons;
using UX.UI.Ingame;

namespace Game.Relics
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class RottingScythe : RelicLogic
    {
        public RottingScythe(RelicConfig cfg) : base(cfg)
        {
            currentCooldown = 0;
        }

        public override void Activate()
        {
            if (currentCooldown == 0)
            {
                foreach (var piece in MatchManager.Ins.GameState.PieceBoard)
                {
                    if (piece == null || piece.Effects.All(e => e.EffectName != "effect_infected")) continue;
                    TileManager.Ins.MarkAsMoveable(piece.Pos);
                    var pending = new RottingScythePending(this, piece.Pos, piece.Color);
                    BoardViewer.ListOf.Add(pending);
                }
                BoardViewer.Selecting = -2;
                BoardViewer.SelectingFunction = 4;
            }
        }
    }
}