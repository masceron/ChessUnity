using MemoryPack;
using Game.Managers;
using UnityEngine;
namespace Game.Action.Captures
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class FrenziedCaptureDontMove: Action, IDontEndTurn
    {
        public FrenziedCaptureDontMove(int maker, int target) : base(maker)
        {
            Target = target;
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