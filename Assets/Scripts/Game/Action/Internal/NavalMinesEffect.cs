namespace Game.Action.Internal
{
    public class NavalMinesEffect : Action
    {
        public NavalMinesEffect(int target) : base(target)
        {
            Target = (ushort)target;
        }

        protected override void ModifyGameState()
        {
            
        }
    }
}