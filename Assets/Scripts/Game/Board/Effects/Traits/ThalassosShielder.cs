using System.Collections.Generic;
using System.Linq;
using Game.Board.Action;
using Game.Board.Action.Internal;
using Game.Board.Effects.Buffs;
using Game.Board.Piece.PieceLogic;
using static Game.Common.BoardUtils;
using static Game.Board.General.MatchManager;

namespace Game.Board.Effects.Traits
{
    public class ThalassosShielder: Effect, IEndTurnEffect
    {
        private List<PieceLogic> inRange = new();
        private readonly List<PieceLogic> shielding = new();
        public ThalassosShielder(PieceLogic piece) : base(-1, 1, piece, EffectName.ThalassosShielder)
        {
            EndTurnEffectType = EndTurnEffectType.EndOfAllyTurn;
            InRange();
        }
        
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

                    if (p != null && p.Color == Piece.Color)
                    {
                        newInRange.Add(p);
                    }
                }
            }

            foreach (var pieceEntered in newInRange.Except(inRange))
            {
                if (pieceEntered.Effects.Any(e => e.EffectName == EffectName.Shield) || !Roll(50)) continue;
                
                shielding.Add(pieceEntered);
                ActionManager.EnqueueAction(new ApplyEffect(new Shield(pieceEntered)));
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

        public void OnCallEnd(Action.Action action)
        {
            if (action.DoesMoveChangePos &&
                (action.Maker == Piece.Pos || ColorOfPiece(action.Maker) == Piece.Color))
            {
                InRange();
            }
        }

        public EndTurnEffectType EndTurnEffectType { get; set; }
    }
}