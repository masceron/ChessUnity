using Game.Action;
using Game.Action.Internal;
using Game.Effects.Buffs;
using Game.Piece.PieceLogic.Commons;
using Unity.Mathematics;
using UnityEngine;
using static Game.Common.BoardUtils;

namespace Game.Effects.Traits
{
    public class ThreadPipefishEffect : Effect, IEndTurnEffect
    {
        private PieceLogic Target;
        public ThreadPipefishEffect(PieceLogic piece, PieceLogic target) : base(-1, -1, piece, "effect_thread_pipefish_effect")
        {
            Target = target;
            EndTurnEffectType = EndTurnEffectType.EndOfAllyTurn;
        }

        public override void OnApply()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new TrueBite(Target)));
            ActionManager.EnqueueAction(new ApplyEffect(new Piercing(-1, Target)));
            ActionManager.EnqueueAction(new ApplyEffect(new Momentum(-1, Target)));
        }

        public void OnCallEnd(Action.Action lastMainAction)
        {
            int posMaker = Piece.Pos;
            int posTarget = Target.Pos;
            var distance = math.max(math.abs(RankOf(posMaker) - RankOf(posTarget)),
                math.abs(FileOf(posMaker) - FileOf(posTarget)));

            Debug.Log(distance);
            if (distance > 3)
            {
                ActionManager.EnqueueAction(new RemoveEffect(this));
            }
        }

        public override void OnRemove()
        {
            foreach (var effect in Target.Effects)
            {
                var name = effect.EffectName;
                if (name.Equals("effect_true_bite") || name.Equals("effect_piercing") || name.Equals("effect_momentum")) 
                    ActionManager.EnqueueAction(new RemoveEffect(effect));
            }
        }
        
        public EndTurnEffectType EndTurnEffectType { get; set; }
    }
}