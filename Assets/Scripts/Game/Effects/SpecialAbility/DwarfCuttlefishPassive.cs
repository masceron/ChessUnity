
using Game.Effects.Traits;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Tile;
using Game.Triggers;
using UnityEngine;
using ZLinq;

namespace Game.Effects.SpecialAbility
{
    public class DwarfCuttlefishPassive : Effect, IFormationTrigger
    {
        private const int EvasionChanceBuff = 10;
        
        public DwarfCuttlefishPassive(PieceLogic piece) : base(-1, 1, piece, "effect_dwarf_cuttlefish_passive")
        {
            
        }

        public void OnEnter(PieceLogic piece, Formation formation)
        {
            if (formation != null)
            {
                var evasion = Piece.Effects.OfType<Evasion>().FirstOrDefault();
                if (evasion != null)
                {
                    evasion.Strength += EvasionChanceBuff;
                    Debug.Log("Evasion = " + evasion.Strength);
                }
            }
        }

        public void OnExit(PieceLogic piece, Formation formation)
        {
            if (formation != null)
            {
                var evasion = Piece.Effects.OfType<Evasion>().FirstOrDefault();
                if (evasion != null)
                {
                    if (evasion.Strength > EvasionChanceBuff)
                    {
                        evasion.Strength -= EvasionChanceBuff;
                        Debug.Log("Evasion = " + evasion.Strength);
                    }
                    else evasion.Strength = 0;
                }
            }
        }
    }
}