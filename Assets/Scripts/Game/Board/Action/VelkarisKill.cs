using Game.Board.General;
using Game.Board.Interaction;
using Game.Board.PieceLogic;
using UnityEngine;

namespace Game.Board.Action
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class VelkarisKill: Action
    {
        public VelkarisKill(int p, ushort f, ushort t) : base(p, false, false, false)
        {
            From = f;
            To = t;
        }

        public override void ApplyAction(GameState state)
        {
            Object.Destroy(InteractionManager.PieceManager.GetPiece(To).gameObject);
            ActionManager.Execute(new EndTurn());
            
            ModifyGameState(state);
        }

        public override void ModifyGameState(GameState state)
        {
            state.MainBoard[To] = null;
            ((Velkaris)state.MainBoard[From]).SkillCooldown = -1;
        }
    }
}