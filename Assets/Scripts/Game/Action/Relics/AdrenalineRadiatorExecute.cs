using MemoryPack;

namespace Game.Action.Relics
{
    [MemoryPackable]
    public partial class AdrenalineRadiatorExecute : Action, IRelicAction
    {
        [MemoryPackConstructor]
        private AdrenalineRadiatorExecute()
        {
        }

        public AdrenalineRadiatorExecute(int maker) : base(null, maker)
        {
        }

        protected override void ModifyGameState()
        {
            //Làm lại
            //GetTargetAsPiece().SkillCooldown = 0;
        }
    }
}