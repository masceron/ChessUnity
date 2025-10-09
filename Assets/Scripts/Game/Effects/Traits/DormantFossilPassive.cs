using System.Collections.Generic;
using System.Linq;
using Game.Action;
using Game.Action.Internal;
using Game.Action.Internal.Pending;
using Game.Piece;
using Game.Piece.PieceLogic;
using UnityEngine;
using UX.UI.Ingame;
using UX.UI.Ingame.ChrysosShop;
using UX.UI.Ingame.DormantFossil;
using static Game.Common.BoardUtils;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    
    public class DormantFossilPassive : Effect, IEndTurnEffect
    {
        private byte numTurns;
        private const byte TurnsToActive = 2;
        
        public EndTurnEffectType EndTurnEffectType { get; }
        
        public DormantFossilPassive(PieceLogic piece) : base(-1, -1, piece, EffectName.DormantFossilPassive)
        {
            EndTurnEffectType = EndTurnEffectType.EndOfEnemyTurn;
        }

        private void ActivePassive() 
        {
            var ui = Object.FindAnyObjectByType<DormantFossilUI>(FindObjectsInactive.Include);

            if (!ui)
            {
                var canvas = Object.FindAnyObjectByType<BoardViewer>(FindObjectsInactive.Exclude);
                ui = Object.Instantiate(UIHolder.Ins.Get(IngameSubmenus.DormantFossilUI), canvas.transform)
                    .GetComponent<DormantFossilUI>();
            }
            else
            {
                ui.gameObject.SetActive(true);
            }

            ui.Load(Piece.Pos);
        }

        public void OnCallEnd(Action.Action action)
        {
            numTurns++;
            Debug.Log(numTurns);
            if (numTurns % TurnsToActive == 0)
            {
                Debug.Log("actived");
                ActivePassive();
            }

        }

    }
}