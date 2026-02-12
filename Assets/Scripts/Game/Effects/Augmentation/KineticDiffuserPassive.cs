using Game.Action;
using Game.Action.Internal;
using Game.Effects.Traits;
using Game.Piece.PieceLogic.Commons;
using ZLinq;

namespace Game.Effects.Augmentation
{
    public class KineticDiffuserPassive : Effect, IOnApply
    {
        private const int EvasionProbability = 25;
        public KineticDiffuserPassive(PieceLogic piece) : base(-1, 1, piece, "effect_kinetic_diffuser_passive")
        {
            
        }


        public void OnApply()
        {
            var evasion = Piece.Effects.OfType<Evasion>().FirstOrDefault();

            if (evasion != null)
            {
                evasion.Strength += EvasionProbability;
            }
            else
            {
                ActionManager.EnqueueAction(
                    new ApplyEffect(new Evasion(-1, EvasionProbability, Piece))
                );
            }

        }
    }
}