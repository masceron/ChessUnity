using Game.Board.General;
using Game.Board.Interaction;
using Game.Board.PieceLogic;
using Game.Board.PieceLogic.Commanders;
using UnityEngine;

namespace Game.Board.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class VelkarisKill: Action, ISkills
    {
        public VelkarisKill(int p, ushort f, ushort t) : base(p, false)
        {
            From = f;
            To = t;
        }

        public override void ApplyAction(GameState state)
        {
            Object.Destroy(InteractionManager.PieceManager.GetPiece(To).gameObject);
            
            ModifyGameState(state);
        }

        public override void ModifyGameState(GameState state)
        {
            state.MainBoard[To] = null;
            ((Velkaris)state.MainBoard[From]).SkillCooldown = -1;
        }
    }
}