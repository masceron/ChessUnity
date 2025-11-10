using Game.Managers;
using UnityEngine;
using UX.UI.Ingame;
using Game.Common;

namespace Game.Relics.MangroveCharm
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class MangroveCharm : RelicLogic
    {
        public MangroveCharm(RelicConfig config) : base(config)
        {
            Type = config.Type;
            Color = config.Color;
            TimeCooldown = config.TimeCooldown; // Cooldown in turns
            currentCooldown = 0;
        }
        public override void Activate()
        {
            if (currentCooldown == 0)
            {
                
                foreach (var piece in MatchManager.Ins.GameState.PieceBoard)
                {
                    if (piece == null) continue;
                    if (!BoardUtils.IsNextEachOther(piece)) continue;
                    TileManager.Ins.MarkAsMoveable(piece.Pos);
                    var pending = new MangroveCharmPending(this, piece.Pos, piece.Color);
                    BoardViewer.ListOf.Add(pending);
                }

                BoardViewer.Selecting = -2;
                BoardViewer.SelectingFunction = 4;
            }
            else
            {
                Debug.Log("Eye of Mimic is on cooldown for " + currentCooldown + " more turns.");
            }
        }
    }
}
