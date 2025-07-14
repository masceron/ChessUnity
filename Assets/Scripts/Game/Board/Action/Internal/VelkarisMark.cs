using Game.Board.Effects;
using Game.Board.General;
using Game.Board.PieceLogic;
using Game.Board.PieceLogic.Commanders;
using UnityEngine;

namespace Game.Board.Action.Internal
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class VelkarisMark: Action, IInternal
    {
        public VelkarisMark(int p, ushort f, ushort t): base(p, false)
        {
            From = f;
            To = t;
        }

        public override void ApplyAction(GameState state)
        {
            Debug.Log("Velkaris marked " + To);
            ActionManager.TakeAction(new ApplyEffect(new VelkarisMarked(state.MainBoard[To])));
            
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