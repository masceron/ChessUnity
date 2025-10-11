using Game.Piece.PieceLogic;
using UnityEngine;
using UX.UI.Ingame;
using UX.UI.Ingame.ChrysosShop;
using UX.UI.Ingame.DormantFossil;
using static Game.Common.BoardUtils;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    
    public class DormantFossilPassive : Effect, IEndTurnEffect, IStartTurnEffect
    {
        private const byte TurnsToActive = 2;
        private byte numTurns = TurnsToActive;
        
        public DormantFossilPassive(PieceLogic piece) : base(-1, -1, piece, EffectName.DormantFossilPassive)
        {
            //Debug.Log("activated");
            EndTurnEffectType = EndTurnEffectType.EndOfEnemyTurn;
            StartTurnEffectType = StartTurnEffectType.StartOfAllyTurn;
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

        public override void OnCallStart(Action.Action action)
        {
            if (numTurns == 0)
            {
                ActivePassive();
                numTurns = TurnsToActive;
            }
        }

        public void OnCallEnd(Action.Action action)
        {
            numTurns--;
            Debug.Log("increased turn" + numTurns);
        }
        
        public EndTurnEffectType EndTurnEffectType { get; set; }
        public StartTurnEffectType StartTurnEffectType { get; set; }
    }
}