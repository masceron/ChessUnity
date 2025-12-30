

using Game.Action;
using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using UnityEngine;
using static Game.Common.BoardUtils;

namespace Game.Effects.SpecialAbility
{
    public class CabezonPassive : Effect
    {
        public CabezonPassive(PieceLogic piece) : base(-1, 1, piece, "effect_cabezon_passive")
        {

        }
        public override void OnCallPieceAction(Action.Action action)
        {
            Debug.Log("[Cabezon] passive activated");
            if (action.Maker == Piece.Pos || action.Result == ResultFlag.Blocked)
            {
                ActionManager.EnqueueAction(new ApplyEffect(new Poison(1, PieceOn(action.Target))));
            }
        }
    }
}