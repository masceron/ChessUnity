using Game.Action.Quiets;
using Game.Common;
using Game.Effects.Triggers;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;

namespace Game.Tile
{
    public enum FormationCategory
    {
        Positive,
        Negative,
        Miscellaneous
    }

    public enum FormationType
    {
        None,
        FogOfWar,
        BubbleVent,
        AnchorIce,
        DazzlingLight,
        UrchinField,
        Saprolegnia,
        Kelp,
        PredatorLair,
        NavalMines,
        SiltCloud,
        HydroidThicket,
        AnoxicPool,
        CarpetAnemone,
        HopkinsRose,
        RealityDistortion
    }

    public abstract class Formation : Observer, IAfterPieceActionTrigger, IOnPieceSpawnedTrigger
    {
        public readonly FormationCategory category;

        protected Formation()
        {
        }

        protected Formation(bool color)
        {
            Color = color;
            var info = AssetManager.Ins.FormationData[GetFormationType()];
            category = info.formationCategory;
        }

        public int Pos { get; private set; }
        protected PieceLogic PieceOnFormation { get; set; }
        public bool HaveDuration { get; protected set; }
        public int Duration { get; protected set; }

        public AfterActionPriority Priority => AfterActionPriority.Move;

        public virtual void OnCallAfterPieceAction(Action.Action action)
        {
            if (action is not IQuiets) return;
            var pieceOn = BoardUtils.PieceOn(action.Target);
            if (pieceOn == null) return;
            if (action.Target == Pos)
                OnPieceEnter(pieceOn);
            else if (action.Maker == Pos) OnPieceExit(pieceOn);
        }

        public void OnPieceSpawn(PieceLogic piece)
        {
            if (BoardUtils.IsAlive(piece) && piece.Pos == Pos) OnPieceEnter(piece);
        }

        /// <summary>
        ///     Hiện hàm này được gọi chủ động bởi FormationManager::SetFormation() nên mọi người không cần phải động tới
        /// </summary>
        /// <param name="index"></param>
        public void SetPositon(int index)
        {
            Pos = index;
        }

        public void SetDuration(int duration)
        {
            Duration = duration;
            HaveDuration = true;
        }

        public bool GetColor()
        {
            return Color;
        }

        /// <summary>
        ///     Trả về FormationType tương ứng với class
        /// </summary>
        public abstract FormationType GetFormationType();

        /// <summary>
        ///     Được gọi 1 lần duy nhất sau khi tạo Formation, thường dùng để gây effect lên quân đứng sẵn trên đó
        /// </summary>
        /// <param name="piece"></param>
        public virtual void OnCreated(PieceLogic piece)
        {
            OnPieceEnter(piece);
        }

        public void OnRemove(PieceLogic piece)
        {
            OnPieceExit(piece);
        }

        /// <summary>
        ///     Hàm này được gọi tự động giống OnCollisionEnter() của MonoBehaviour. Gọi ngay lập tức khi quân đi vào vị trí
        /// </summary>
        protected virtual void OnPieceEnter(PieceLogic piece)
        {
            PieceOnFormation = piece;
        }

        /// <summary>
        ///     Hàm này được gọi tự động giống OnCollisionEnter() của MonoBehaviour. Gọi ngay lập tức khi quân rời khỏi vị trí
        /// </summary>
        protected virtual void OnPieceExit(PieceLogic piece)
        {
            PieceOnFormation = null;
        }

        public virtual int GetValueForAI()
        {
            return 0;
        }
    }
}