using System;
using Game.Common;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class HourglassJellyActive : Action, ISkills
    {
        [MemoryPackInclude] private int _destination;

        [MemoryPackConstructor]
        private HourglassJellyActive()
        {
        }

        public HourglassJellyActive(PieceLogic maker, PieceLogic target) : base(maker, target)
        {
            var piece = GetTargetAsPiece();
            //Làm lại
            //_destination = piece.PreviousMoves[Math.Max(0, piece.PreviousMoves.Count - 5)];
        }

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            var maker = GetMakerAsPiece();
            if (maker == null || pieceAI == null) return 0;
            if (pieceAI.Color != maker.Color) return -25;
            return 0;
        }

        protected override void Animate()
        {
            PieceManager.Ins.Move(GetTargetPos(), _destination);
            var destinationPiece = PieceOn(_destination);
            if (destinationPiece != null) PieceManager.Ins.Destroy(_destination);
        }

        protected override void ModifyGameState()
        {
            var destinationPiece = PieceOn(_destination);
            if (destinationPiece != null) Destroy(PieceOn(_destination));
            Move(GetTargetAsPiece(), _destination);
            SetCooldown(GetMakerAsPiece(), ((IPieceWithSkill)GetMakerAsPiece()).TimeToCooldown);
        }
        // public void CompleteActionForAI()
        // {
        //     var makerPiece = GetMakerAsPiece();
        //     if (makerPiece == null) return;

        //     var (r, f) = RankFileOf(Maker);
        //     var enemies = GetPiecesInRadius(r, f, 4, p => p != null && p.Color != makerPiece.Color);

        //     int bestScore = enemies.Max(p => p.GetValueForAI());
        //     var top = enemies.Where(p => p.GetValueForAI() == bestScore).ToList();
        //     var chosen = top.Count == 1 ? top[0] : top[UnityEngine.Random.Range(0, top.Count)];

        //     Target = chosen.Pos;
        //     var targetPiece = GetTargetAsPiece();
        //     if (targetPiece == null) return;
        //     destination = targetPiece.PreviousMoves[Math.Max(0, targetPiece.PreviousMoves.Count - 5)];

        //     BoardViewer.Ins.ExecuteAction(this);
        // }
    }
}