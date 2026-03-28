using Game.Action.Quiets;
using Game.Common;
using Game.Effects.States;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using ZLinq;

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
        public System.Action<Formation> OnRemoveFormation;
        protected Formation()
        {
        }

        protected Formation(bool color, int duration = -1)
        {
            Color = color;
            var info = AssetManager.Ins.FormationData[GetFormationType()];
            category = info.formationCategory;
            if (duration != -1)
            {
                HaveDuration = true;
                Duration = duration;
            }
        }

        public int Pos { get; private set; }
        public PieceLogic PieceOnFormation { get; set; }
        public bool HaveDuration { get; protected set; }
        public int Duration { get; protected set; }

        /// <summary>State hiện tại của Formation. Mặc định là <see cref="StateType.None"/>.</summary>
        public StateType CurrentState { get; private set; } = StateType.None;

        /// <summary>Gán State cho Formation. Ghi đè state cũ (Formation chỉ có 1 state).</summary>
        public void SetState(StateType state) => CurrentState = state;

        /// <summary>Xóa State, trả về <see cref="StateType.None"/>.</summary>
        public void ClearState() => CurrentState = StateType.None;

        /// <summary>Kiểm tra Formation có đang ở State chỉ định không.</summary>
        public bool HasState(StateType type) => CurrentState == type;

        public AfterActionPriority Priority => AfterActionPriority.Formation;

        public virtual void OnCallAfterPieceAction(Action.Action action)
        {
            if (action is not IQuiets) return;
            var pieceOn = BoardUtils.PieceOn(action.Target);
            if (pieceOn == null) return;
            if (action.Target == Pos)
                OnPieceEnter(pieceOn);
            else if (action.Maker == Pos) OnPieceExit(pieceOn);
        }

        public virtual void OnPieceSpawn(PieceLogic piece)
        {
            if (BoardUtils.IsAlive(piece) && piece.Pos == Pos) OnPieceEnter(piece);
        }

        /// <summary>
        ///     Hàm đã lỗi thời, để cài đặt Duration, bây giờ chỉ cần qua constructor
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
            OnRemoveFormation?.Invoke(this);
            if (piece != null) OnPieceExit(piece);
        }

        /// <summary>
        ///     Hàm này được gọi tự động giống OnCollisionEnter() của MonoBehaviour. Gọi ngay lập tức khi quân đi vào vị trí
        /// </summary>
        protected virtual void OnPieceEnter(PieceLogic piece)
        {
            PieceOnFormation = piece;
            foreach (var effect in piece.Effects.OfType<IFormationTrigger>())
            {
                effect.OnEnter(piece, this);
            }
        }

        /// <summary>
        ///     Hàm này được gọi tự động giống OnCollisionEnter() của MonoBehaviour. Gọi ngay lập tức khi quân rời khỏi vị trí
        /// </summary>
        protected virtual void OnPieceExit(PieceLogic piece)
        {
            PieceOnFormation = null;
            foreach (var effect in piece.Effects.OfType<IFormationTrigger>())
            {
                effect.OnExit(piece, this);
            }
            
        }

        public virtual int GetValueForAI()
        {
            return 0;
        }
    }
}