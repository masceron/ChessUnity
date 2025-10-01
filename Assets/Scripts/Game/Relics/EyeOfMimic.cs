using Game.Action;
using Game.Common;
using Game.Managers;
using Game.Relics;
using UnityEngine;

namespace Game.Relics
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class EyeOfMimic : RelicLogic 
    {
        public EyeOfMimic(RelicConfig config) : base(config)
        {
            Type = RelicType.EyeOfMimic;
            TimeCooldown = 3; // Cooldown in turns
            currentCooldown = 0;
        }
        public override void Activate()
        {
            if (currentCooldown == 0)
            {
                //select Piece and random 2 effect buff if the piece same color, 2 effect debuff if different color
                Debug.Log("Eye of Mimic activated!");
                foreach (var piece in MatchManager.Ins.GameState.PieceBoard)
                {
                    if (piece == null) continue;
                    if (BoardUtils.PieceOn((piece.Pos)) != null)
                    {
                        TileManager.Ins.MarkAsMoveable(piece.Pos);
                    }
                }
                // Implement the logic to mimic the last used relic's effect here.
                currentCooldown = TimeCooldown;
            }
            else
            {
                Debug.Log("Eye of Mimic is on cooldown for " + currentCooldown + " more turns.");
            }
        }
    }
}
