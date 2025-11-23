using Game.Piece.PieceLogic.Commons;
using UnityEngine;
using UX.UI.Ingame;
using UX.UI.Ingame.DormantFossil;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    
    public class DormantFossilPassive : Effect, IEndTurnEffect
    {
        private const byte TurnsToActive = 15;
        private byte numTurns = TurnsToActive;
        
        public DormantFossilPassive(PieceLogic piece) : base(-1, -1, piece, "effect_dormant_fossil_passive")
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

        public void OnCallEnd(Action.Action lastMainAction)
        {
            numTurns--;
            if (numTurns == 0)
            {
                ActivePassive();
            }
        }
        
        public EndTurnEffectType EndTurnEffectType { get; }
    }
}