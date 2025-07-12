using Game.Board.Effects;
using Game.Board.General;
using Game.Board.PieceLogic;
using UnityEngine;

namespace Game.Board.Action.Internal
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class VelkarisMark: Action
    {
        public VelkarisMark(int p, ushort f, ushort t): base(p, false, false, true)
        {
            From = f;
            To = t;
        }

        public override void ApplyAction(GameState state)
        {
            Debug.Log("Velkaris marked " + To);
            ActionManager.Execute(new ApplyEffect(new VelkarisMarked(state.MainBoard[To])));
            
            ModifyGameState(state);
        }

        public override void ModifyGameState(GameState state)
        {
            var caller = (Velkaris)state.MainBoard[From];
            caller.SkillCooldown = -1;
            caller.Marked = state.MainBoard[To];
        }
    }
}