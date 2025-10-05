using Game.Common;
using Game.Managers;
using UnityEngine;
using UX.UI.Ingame;
using Game.Action;
using System.Linq;

namespace Game.Relics
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]

    public class FrostSigil : RelicLogic
    {
        private Tile.Tile hoveringTile;
        public FrostSigil(RelicConfig cfg) : base(cfg)
        {
            currentCooldown = 0;
        }

        public override void Activate()
        {
            if (currentCooldown == 0)
            {
                BoardViewer.Selecting = -2;
                BoardViewer.SelectingFunction = 4;

                Tile.Tile.OnPointEnterHandle = (thisTile) =>
                {
                    if (hoveringTile != null)
                    {
                        TileManager.Ins.MarkTileInRange(hoveringTile, 3, isMark: false);
                    }

                    hoveringTile = thisTile;
                    TileManager.Ins.MarkTileInRange(hoveringTile, 3, isMark: true, onlyMarkEnemy: false);
                    
                    int pos = BoardUtils.IndexOf(hoveringTile.rank, hoveringTile.file);
                    var pending = new FrostSigilPending(pos ,hoveringTile, this);
                    var comparer = new ActionComparer();

                    if (!BoardViewer.ListOf.Contains(pending, new ActionComparer()))
                    {
                        BoardViewer.ListOf.Add(pending);
                    }
                };
            }
            else
            {
                Debug.Log("Frost Sigil is on cooldown for " + currentCooldown + "more turn");
            }
        }
    }
}

