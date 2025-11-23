using Game.Managers;
using UnityEngine;
namespace Game.Action.Captures
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class FrenziedCaptureDontMove: Action, IDontEndTurn
    {
        public FrenziedCaptureDontMove(int f, int t) : base(f, true)
        {
            Maker = f;
            Target = t;
        }
        protected override void Animate()
        {
            
        }

        protected override void ModifyGameState()
        {
            Debug.Log("Complete FrenziedCaptureDontMove");
            PieceManager.Ins.Destroy(Target);
            MatchManager.Ins.GameState.Kill(Target);
        }
    }
}