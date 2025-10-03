using Game.Action;
using Game.Action.Internal;
using Game.Piece.PieceLogic;
using Game.Managers;
using Game.Action.Captures;
using UnityEngine;
namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Relentless: Effect
    {

        private int survivalCount;
        public Relentless(PieceLogic piece, int survivalCount) : base(-1, 1, piece, EffectName.Relentless)
        {
            this.survivalCount = survivalCount;
        }

        public override void OnCall(Action.Action action)
        {
            if (action == null || action.Target != Piece.Pos || action.Result != ActionResult.Succeed || action.Maker == action.Target) {
                return;
            }
            Debug.Log("Relentless: " + survivalCount);
            action.Result = ActionResult.Failed;
            ActionManager.EnqueueAction(new KillPiece(action.Maker));
            survivalCount--;
            if (survivalCount <= 0)
            {
                ActionManager.EnqueueAction(new KillPiece(Piece.Pos));
            }
        
        }


    }
}