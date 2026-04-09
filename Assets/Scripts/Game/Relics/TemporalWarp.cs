using Game.Common;
using Game.Managers;
using Game.Relics.Commons;
using UnityEngine;
using UX.UI.Ingame;

namespace Game.Relics
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class TemporalWarp : RelicLogic
    {
        public TemporalWarp(RelicConfig config) : base(config)
        {
            type = config.Type;
            Color = config.Color;
            TimeCooldown = 2;
            CurrentCooldown = 0;
        }

        public override void Activate()
        {
            if (CurrentCooldown != 0) return;
            foreach (var piece in BoardUtils.FindAllAlliesInEnemyHalf(Color))
            {
                Debug.Log(piece.Type);
                TileManager.Ins.MarkAsMoveable(piece.Pos);
                //Làm lại
                // var pending = new TemporalWarpPending(piece.Pos);
                // BoardViewer.ListOf.Add(pending);

                BoardViewer.Selecting = -2;
                BoardViewer.SelectingFunction = 4;
            }
        }

        public override void ActiveForAI()
        {
        }
    }
}