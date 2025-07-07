using Core;
using UnityEngine;

namespace Board.Action
{
    public class SirenDebuff: Action
    {
        public SirenDebuff(int f, int t)
        {
            From = (ushort)f;
            To = (ushort)t;
        }

        public override void ApplyAction(GameState state)
        {
            Debug.Log("Siren Debuffed " + To);
            
            ModifyGameState(state);
        }

        public override void ModifyGameState(GameState state)
        {
            state.MainBoard[To].Effects.Add(new Effect(EffectType.SlowOne, 1));
            state.LastMove = this;
        }

        public override bool DoesMoveChangePos()
        {
            return true;
        }
    }
}