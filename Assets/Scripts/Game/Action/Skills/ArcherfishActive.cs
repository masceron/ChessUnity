using System.Linq;
using static Game.Common.BoardUtils;
using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using Game.AI;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ArcherfishActive: Action, ISkills, IAIAction
    {
        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            var maker = PieceOn(Maker);
            if (maker == null || pieceAI == null) return 0;

            // Skill này chỉ tác động lên quân địch.
            // Nếu quân của AI (pieceAI) khác màu với người tạo skill, nó sẽ bị phạt.
            return pieceAI.Color != maker.Color ? -15 : 0;
        }

        public ArcherfishActive(int maker, int target) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)target;
        }

        protected override void Animate()
        {

        }

        protected override void ModifyGameState()
        {
            // Gây hiệu ứng Blind và Marked lên 1 quân địch trong bán kính 4 ô
            ActionManager.EnqueueAction(new ApplyEffect(new Blinded(2, 100, PieceOn(Target)), PieceOn(Maker)));
            ActionManager.EnqueueAction(new ApplyEffect(new Marked(2, PieceOn(Target)), PieceOn(Maker)));

            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }

        public void CompleteActionForAI()
        {
            var (rank, file) = RankFileOf(Maker);
            var listPieces = GetPiecesInRadius(rank, file,4, p => p != null && p.Color != PieceOn(Maker).Color)
                .Where(p => p.Effects.Any(p => p.EffectName != "effect_extremophile"
                                          && p.EffectName != "effect_blind" &&
                                          p.EffectName != "effect_marked")).ToList();
            
            if (listPieces.Count == 0) return;
            listPieces.Sort((a, b) => a.GetValueForAI().CompareTo(b.GetValueForAI()));
            
            var idx = UnityEngine.Random.Range(0, listPieces.Count);
            ActionManager.EnqueueAction(new ApplyEffect(new Blinded(2, 100, listPieces[idx]), PieceOn(Maker)));
            ActionManager.EnqueueAction(new ApplyEffect(new Marked(2, listPieces[idx]), PieceOn(Maker)));
            
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }

    }
}
