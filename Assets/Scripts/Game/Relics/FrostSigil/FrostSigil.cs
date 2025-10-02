using Game.Common;
using Game.Managers;
using UnityEngine;
using Game.Tile;

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
                /*foreach (var activeBoard in BoardUtils.ActiveBoard())
                {
                    if ((bool)activeBoard)
                    {

                    }
                }*/
                Tile.Tile.OnPointEnterHandle = (thisTile) =>
                {
                    if (hoveringTile != null)
                    {
                        TileManager.Ins.MarkTileInRange(hoveringTile, 3, false);
                    }

                    hoveringTile = thisTile;
                    
                    TileManager.Ins.MarkTileInRange(hoveringTile, 3, true);
                };
                currentCooldown = TimeCooldown;
            }
            else
            {
                Debug.Log("Frost Sigil is on cooldown for " + currentCooldown + "more turn");
            }
        }
    }
}

