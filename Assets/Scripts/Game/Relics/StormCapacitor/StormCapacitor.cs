using System.Linq;
using Game.Action;
using Game.Common;
using Game.Managers;
using UnityEngine;
using UX.UI.Ingame;
using Game.Effects.Others;


namespace Game.Relics.StormCapacitor
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class StormCapacitor : RelicLogic
    {
        private Tile.Tile hoveringTile;
        private int size = 2;
        private Charge charge;
        public StormCapacitor(RelicConfig cfg) : base(cfg)
        {
            currentCooldown = 0;
            charge = new Charge(0, Color);
            BoardUtils.AddObserver(charge);
        }

        public override void Activate()
        {
            Debug.Log("Charge: " + charge.Strength);
            if (charge.Strength >= 3)
            {
                
                BoardViewer.Selecting = -2;
                BoardViewer.SelectingFunction = 4;

                Tile.Tile.OnPointEnterHandle = (thisTile) =>
                {
                    if (hoveringTile == thisTile) return;
                    if (hoveringTile != null)
                    {
                        TileManager.Ins.MarkTileInRange(hoveringTile, size, isMark: false);
                    }

                    hoveringTile = thisTile;
                    TileManager.Ins.MarkTileInRange(hoveringTile, size, isMark: true, onlyMarkEnemy: false);
                    
                    int pos = BoardUtils.IndexOf(hoveringTile.rank, hoveringTile.file);
                    var pending = new StormCapacitorPending(pos ,hoveringTile, this, size);
                    var comparer = new ActionComparer();

                    if (!BoardViewer.ListOf.Contains(pending, new ActionComparer()))
                    {
                        BoardViewer.ListOf.Add(pending);
                    }
                };
            }
            if(currentCooldown > 0)
            {
                charge.Strength = 0;
            }

        }
    }
}