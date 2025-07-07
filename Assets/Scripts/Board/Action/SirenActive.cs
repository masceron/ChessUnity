using Core;

namespace Board.Action
{
    public class SirenActive: Action
    {
        public SirenActive(int c, int f, int t)
        {
            Caller = (ushort)c;
            From = (ushort)f;
            To = (ushort)t;
        }
        public override void ApplyAction(GameState state)
        {
            
            
            ModifyGameState(state);
        }

        public override void ModifyGameState(GameState state)
        {
            state.MainBoard[To] = state.MainBoard[From];
            state.MainBoard[To].Pos = To;
            state.MainBoard[From] = null;
            state.MainBoard[Caller].SkillCooldown = 12;
            state.LastMove = this;
        }

        public override bool DoesMoveChangePos()
        {
            return true;
        }
    }
}