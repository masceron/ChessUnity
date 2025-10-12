using Game.Managers;

namespace Game.Effects.RegionalEffect
{
    public abstract class RegionalEffect
    {
        protected RegionalEffect(){
            MatchManager.Ins.GameState.OnIncreaseTurn += ApplyEffect;
        }
        protected abstract void ApplyEffect(int currentTurn);

    }
}
