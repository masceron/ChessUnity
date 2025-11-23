using Game.Managers;
using UnityEngine;

namespace Game.Action.Captures
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class FrenziedCapture: Action, IDontEndTurn, ICaptures
    {
        public FrenziedCapture(int f, int t) : base(f, true)
        {
            Maker = f;
            Target = t;
        }
        protected override void Animate()
        {
            
        }

        protected override void ModifyGameState()
        {
            Debug.Log("Complete FrenziedCapture");
            PieceManager.Ins.Destroy(Target);
            PieceManager.Ins.Move(Maker, Target);
            MatchManager.Ins.GameState.Kill(Target);
            MatchManager.Ins.GameState.Move(Maker, Target);
            Maker = Target;
        }

        public void CompleteActionForAI()
        {
            //Implement for AI automatically
        }
    }
}