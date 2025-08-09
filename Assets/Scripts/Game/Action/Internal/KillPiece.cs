using Game.Managers;

namespace Game.Action.Internal
{
    public class KillPiece: Action, IInternal
    {
        public KillPiece(int maker) : base(maker)
        {}

        protected override void Animate()
        {
            
        }

        protected override void ModifyGameState()
        {
            PieceManager.Ins.Destroy(Maker);
            MatchManager.Ins.GameState.Kill(Maker);
        }
    }
}