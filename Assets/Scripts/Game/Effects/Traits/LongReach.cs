using UnityEngine;
using Game.Action;
using Game.Action.Internal;
using Game.Piece.PieceLogic;
using Game.Managers;

namespace Game.Effects.Buffs{
    public class LongReach : Effect
    {

        public LongReach(PieceLogic piece) : base(-1, 1, piece, EffectName.LongReach)
        {
            piece.AttackRange += 2;

        }

        public override void OnRemove()
        {
            Piece.AttackRange -= 2;
        }
    }
}

