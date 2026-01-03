using Game.Managers;
using System;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class HourglassJellyActive: Action, ISkills
    {
        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            var maker = PieceOn(Maker);
            if (maker == null || pieceAI == null) return 0;
            if (pieceAI.Color != maker.Color) return -25;
            return 0;
        }
        private ushort destination;
        public HourglassJellyActive(int maker, int target) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)target;
            var piece = PieceOn(target);
            destination = (ushort)piece.PreviousMoves[Math.Max(0, piece.PreviousMoves.Count - 5)];
        }
        protected override void Animate()
        {
            PieceManager.Ins.Move(Target, destination);
            var destinationPiece = PieceOn(destination);
            if (destinationPiece != null){
                PieceManager.Ins.Destroy(destination);
            }
        }
        protected override void ModifyGameState()
        {
            var (rank, file) = RankFileOf(Maker);
            var target = PieceOn(Target);
            var destinationPiece = PieceOn(destination);
            if (destinationPiece != null)
            {
                MatchManager.Ins.GameState.Destroy(destination);
            }
            MatchManager.Ins.GameState.Move(Target, destination);
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }
        // public void CompleteActionForAI()
        // {
        //     var makerPiece = PieceOn(Maker);
        //     if (makerPiece == null) return;

        //     var (r, f) = RankFileOf(Maker);
        //     var enemies = GetPiecesInRadius(r, f, 4, p => p != null && p.Color != makerPiece.Color);

        //     int bestScore = enemies.Max(p => p.GetValueForAI());
        //     var top = enemies.Where(p => p.GetValueForAI() == bestScore).ToList();
        //     var chosen = top.Count == 1 ? top[0] : top[UnityEngine.Random.Range(0, top.Count)];

        //     Target = (ushort)chosen.Pos;
        //     var targetPiece = PieceOn(Target);
        //     if (targetPiece == null) return;
        //     destination = (ushort)targetPiece.PreviousMoves[Math.Max(0, targetPiece.PreviousMoves.Count - 5)];

        //     BoardViewer.Ins.ExecuteAction(this);
        // }
    }
}