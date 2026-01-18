using System.Collections.Generic;
using System.Linq;
using Game.Action.Internal.Pending;
using Game.Action.Internal.Pending.Relic;
using Game.Common;
using Game.Effects;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Relics.Commons;
using UnityEngine;
using UX.UI.Ingame;
using UX.UI.Ingame.HermosHorn;

namespace Game.Relics
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class HermosHorn : RelicLogic
    {
        public HermosHorn(RelicConfig cfg) : base(cfg)
        {
            CurrentCooldown = 0;
        }

        public override void Activate()
        {
            if (CurrentCooldown == 0)
            {
                var ui = Object.Instantiate(UIHolder.Ins.Get(IngameSubmenus.HermosHornUI)).GetComponent<HermosHornUI>();
                ui.Load(this);
            }
        }

        public override void ActiveForAI()
        {
            
        }
    }
}