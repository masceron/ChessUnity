using Game.Piece.PieceLogic.Commons;
using PrimeTween;
using UnityEngine;
using static Game.Common.BoardUtils;

namespace Game.Piece
{
    public enum PieceRank : byte
    {
        None,
        Construct,
        Summoned,
        Swarm,
        Common,
        Elite,
        Champion,
        Commander
    }

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Piece : MonoBehaviour
    {
        private bool color;
        private int file;
        private int rank;

        public void Spawn(int pos, bool c)
        {
            rank = RankOf(pos);
            file = FileOf(pos);
            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            transform.position = new Vector3(rank, YCoordinate, file);

            var angles = new Quaternion
            {
                eulerAngles = !c
                    ? new Vector3(-90, 0, 0)
                    : new Vector3(-90, 0, 180)
            };

            transform.rotation = angles;
        }

        public void Move(int rankTo, int fileTo)
        {
            rank = rankTo;
            file = fileTo;

            Tween.Position(transform, new Vector3(rank, transform.position.y, file), 0.2f);
        }

        /// <summary>
        ///     Di chuyển quân này lên đỉnh của <paramref name="targetPiece"/> rồi thu nhỏ về 0.3f.
        ///     Dùng cho animation Parasite ký sinh.
        /// </summary>
        public void MoveToParasitic(Piece targetPiece)
        {
            var targetPos = targetPiece.transform.position;
            Tween.Position(transform, targetPos, 0.3f, Ease.OutQuart)
                 .OnComplete(this, static self =>
                     Tween.Scale(self.transform, Vector3.one * 0.3f, 0.2f, Ease.InOutSine));
        }

        /// <summary>
        ///     Di chuyển quân này về <paramref name="rankTo"/>, <paramref name="fileTo"/> rồi phóng to về scale 1.
        ///     Dùng cho animation Parasite tách ra.
        /// </summary>
        public void MoveToDetach(int rankTo, int fileTo)
        {
            rank = rankTo;
            file = fileTo;

            Tween.Position(transform, new Vector3(rank, transform.position.y, file), 0.3f, Ease.OutQuart)
                 .OnComplete(this, static self =>
                     Tween.Scale(self.transform, Vector3.one, 0.2f, Ease.InOutSine));
        }
    }

    public static class PieceMaker
    {
        public static PieceLogic.Commons.PieceLogic Get(PieceConfig config)
        {
            return PieceFactory.CreateLogicInstance(config.Type, config);
        }
    }
}