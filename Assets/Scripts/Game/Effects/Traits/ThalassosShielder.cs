using System.Collections.Generic;
using Game.Action;
using Game.Action.Internal;
using Game.Effects.Buffs;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using ZLinq;
using static Game.Common.BoardUtils;
using static Game.Managers.MatchManager;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ThalassosShielder : Effect, IEndTurnTrigger
    {
        private readonly List<PieceLogic> shielding = new();
        private List<PieceLogic> inRange = new();

        public ThalassosShielder(PieceLogic piece) : base(-1, 1, piece, "effect_thalassos_shielder")
        {
            EndTurnEffectType = EndTurnEffectType.EndOfAllyTurn;
            InRange();
        }

        public void OnCallEnd(Action.Action lastMainAction)
        {
            if (lastMainAction.GetMakerAsPiece() == Piece || lastMainAction.GetMakerAsPiece().Color == Piece.Color) InRange();
        }

        public EndTurnTriggerPriority Priority => EndTurnTriggerPriority.Buff;

        public EndTurnEffectType EndTurnEffectType { get; set; }

        private void InRange()
        {
            var newInRange = new List<PieceLogic>();

            for (var rankOff = -1; rankOff <= 1; rankOff++)
            {
                var rank = RankOf(Piece.Pos) + rankOff;
                if (!VerifyBounds(rank)) continue;

                for (var fileOff = -1; fileOff <= 1; fileOff++)
                {
                    if (rankOff == 0 && fileOff == 0) continue;
                    var file = FileOf(Piece.Pos) + fileOff;
                    if (!VerifyBounds(file)) continue;

                    var p = PieceOn(IndexOf(rank, file));

                    if (p != null && p.Color == Piece.Color) newInRange.Add(p);
                }
            }

            foreach (var pieceEntered in newInRange.Except(inRange))
            {
                if (pieceEntered.Effects.Any(e => e.EffectName == "effect_shield") || !MatchManager.Ins.Roll(50)) continue;

                shielding.Add(pieceEntered);
                ActionManager.EnqueueAction(new ApplyEffect(new Shield(pieceEntered), Piece));
            }

            foreach (var pieceExited in inRange.Except(newInRange))
            {
                if (!shielding.Contains(pieceExited)) continue;
                shielding.Remove(pieceExited);

                var effect = pieceExited.Effects.Find(e => e.GetType() == typeof(Shield));
                if (effect == null) continue;

                ActionManager.EnqueueAction(new RemoveEffect(effect));
            }

            inRange = newInRange;
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 120;
        }
    }
}