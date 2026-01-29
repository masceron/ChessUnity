using System.Linq;
using Game.Action;
using Game.Action.Internal;
using Game.Effects.Traits;
using Game.Piece.PieceLogic.Commons;
using ZLinq;

namespace Game.Effects.Augmentation
{
    public class KineticDiffuserPassive : Effect, IOnApply
    {
        private const int evasionProbability = 25;
        public KineticDiffuserPassive(PieceLogic piece) : base(-1, 1, piece, "effect_kinetic_diffuser_passive")
        {
            
        }


        public void OnApply()
        {
            Evasion evasion = Piece.Effects.OfType<Evasion>().FirstOrDefault();

            if (evasion != null)
            {
                evasion.Probability += evasionProbability;
            }
            else
            {
                ActionManager.EnqueueAction(
                    new ApplyEffect(new Evasion(-1, evasionProbability, Piece))
                );
            }

        }
    }
}