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

        private Vector3 rootScale;

        /// <summary>Transform gốc (PieceManager) để un-parent khi Detach.</summary>
        private Transform _originalParent;

        /// <summary>Host hiện tại đang bị ký sinh, dùng trong OnComplete callback.</summary>
        private Piece _hostPiece;

        void Awake()
        {
            _originalParent = transform.parent;
        }

        void Start()
        {
            rootScale = transform.localScale;
        }

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
        ///     Sau khi xong, parent quân này vào host để tự follow khi host di chuyển (chỉ visual).
        ///     Dùng cho animation Parasite ký sinh.
        /// </summary>
        public void MoveToParasitic(Piece targetPiece)
        {
            _hostPiece = targetPiece;
            var targetPos = targetPiece.transform.position;

            Tween.Position(transform, targetPos + Vector3.up * 0.5f, 0.3f, Ease.OutQuart)
                 .OnComplete(this, static self =>
                 {
                     Tween.Scale(self.transform, self.rootScale * 0.4f, 0.2f, Ease.InOutSine);
                     // Parent vào host: parasite sẽ tự follow mọi di chuyển của host (chỉ visual)
                     self.transform.SetParent(self._hostPiece.transform, worldPositionStays: true);
                 });
        }

        /// <summary>
        ///     Di chuyển quân Adhesive lên đỉnh của ô Formation tại (<paramref name="rankTo"/>, <paramref name="fileTo"/>)
        ///     rồi thu nhỏ về 0.4f. Không parent vào ai vì Formation không phải MonoBehaviour.
        ///     Dùng cho animation Adhesive bám vào Formation.
        /// </summary>
        public void MoveToAdhesiveFormation(int rankTo, int fileTo)
        {
            var targetPos = new Vector3(rankTo, YCoordinate, fileTo);

            Tween.Position(transform, targetPos + Vector3.up * 0.5f, 0.3f, Ease.OutQuart)
                 .OnComplete(this, static self =>
                     Tween.Scale(self.transform, self.rootScale * 0.4f, 0.2f, Ease.InOutSine));
        }

        /// <summary>
        ///     Tách khỏi host, di chuyển về <paramref name="rankTo"/>, <paramref name="fileTo"/>
        ///     rồi phóng to về scale gốc.
        ///     Dùng cho animation Parasite tách ra.
        /// </summary>
        public void MoveToDetach(int rankTo, int fileTo)
        {
            rank = rankTo;
            file = fileTo;

            // Tách khỏi host, giữ world position hiện tại để tween từ đúng chỗ
            transform.SetParent(_originalParent, worldPositionStays: true);
            _hostPiece = null;

            Tween.Position(transform, new Vector3(rank, YCoordinate, file), 0.3f, Ease.OutQuart)
                 .OnComplete(this, static self =>
                     Tween.Scale(self.transform, self.rootScale, 0.2f, Ease.InOutSine));
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