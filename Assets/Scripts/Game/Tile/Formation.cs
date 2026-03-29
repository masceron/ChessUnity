using System.Collections.Generic;
using System.Linq;
using Game.Action.Quiets;
using Game.Common;
using Game.Effects;
using Game.Effects.States;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

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

    public abstract class Formation : Entity, IAfterPieceActionTrigger, IOnPieceSpawnedTrigger
    {
        // ── Fields ───────────────────────────────────────────────────────────────

        public readonly FormationCategory Category;
        public bool Color;

        /// <summary>Callback được gọi khi Formation bị xóa khỏi board.</summary>
        public System.Action<Formation> OnRemoveFormation;

        // ── Effects ──────────────────────────────────────────────────────────────

        /// <summary>
        ///     Danh sách các Effect (chủ yếu là <see cref="StateEffect"/>) mà Formation đang bị dính từ bên ngoài.
        ///     Ví dụ: Formation bị Attached, Petrified, v.v.
        /// </summary>
        public readonly List<Effect> AffectedEffects = new List<Effect>();

        // ── Properties ───────────────────────────────────────────────────────────

        /// <summary>Quân cờ hiện đang đứng trên Formation.</summary>
        public PieceLogic PieceOnFormation { get; set; }

        /// <summary>Formation có giới hạn thời gian tồn tại hay không.</summary>
        public bool HaveDuration { get; protected set; }

        /// <summary>Số lượt còn lại trước khi Formation biến mất (nếu <see cref="HaveDuration"/> = true).</summary>
        public int Duration { get; protected set; }

        /// <summary>State hiện tại của Formation. Mặc định là <see cref="StateType.None"/>.</summary>
        public StateType CurrentState { get; private set; } = StateType.None;

        public AfterActionPriority Priority => AfterActionPriority.Formation;

        // ── Constructors ─────────────────────────────────────────────────────────

        protected Formation()
        {
        }

        protected Formation(bool color, int duration = -1)
        {
            Color = color;

            var info = AssetManager.Ins.FormationData[GetFormationType()];
            Category = info.formationCategory;

            if (duration == -1) return;
            HaveDuration = true;
            Duration = duration;
        }

        // ── Abstract ─────────────────────────────────────────────────────────────

        /// <summary>Trả về <see cref="FormationType"/> tương ứng với class con.</summary>
        public abstract FormationType GetFormationType();

        // ── State Management ─────────────────────────────────────────────────────

        /// <summary>
        ///     Gán State cho Formation thông qua <see cref="StateEffect"/>.
        ///     Nếu đang có State cũ, sẽ tự xóa trước — đảm bảo Formation chỉ có 1 State tại 1 thời điểm.
        /// </summary>
        public void SetState(StateEffect stateEffect)
        {
            ClearState();
            CurrentState = stateEffect.StateType;
            AffectedEffects.Add(stateEffect);
        }

        /// <summary>Gán State cho Formation bằng <see cref="StateType"/> trực tiếp (không qua Effect).</summary>
        public void SetState(StateType state) => CurrentState = state;

        /// <summary>Xóa State hiện tại và loại bỏ StateEffect khỏi <see cref="AffectedEffects"/>.</summary>
        public void ClearState()
        {
            var existing = AffectedEffects.Find(e => e is IStateful);
            if (existing != null)
            {
                AffectedEffects.Remove(existing);
                BoardUtils.RemoveObserver(existing);
            }

            CurrentState = StateType.None;
        }

        /// <summary>Kiểm tra Formation có đang ở State chỉ định không.</summary>
        public bool HasState(StateType type) => CurrentState == type;

        // ── Effect Management ────────────────────────────────────────────────────

        /// <summary>Thêm một Effect (thường là debuff/state) mà Formation đang bị dính.</summary>
        public void AddAffectedEffect(Effect effect)
        {
            if (effect != null && !AffectedEffects.Contains(effect))
                AffectedEffects.Add(effect);
        }

        /// <summary>Xóa một Effect khỏi danh sách effect mà Formation đang bị dính.</summary>
        public void RemoveAffectedEffect(Effect effect)
        {
            AffectedEffects.Remove(effect);
        }

        /// <summary>Kiểm tra Formation có đang bị dính effect theo tên hay không.</summary>
        public bool HasAffectedEffect(string effectName)
        {
            return AffectedEffects.Any(e => e.EffectName == effectName);
        }



        // ── Position & Duration ──────────────────────────────────────────────────

        /// <summary>
        ///     Gán vị trí cho Formation trên board.
        ///     <para>Lưu ý: Hàm này được gọi chủ động bởi FormationManager::SetFormation().</para>
        /// </summary>
        public void SetPosition(int index)
        {
            Pos = index;
        }

        /// <summary>Gán duration cho Formation, tự động bật <see cref="HaveDuration"/>.</summary>
        public void SetDuration(int duration)
        {
            Duration = duration;
            HaveDuration = true;
        }

        public bool GetColor() => Color;

        // ── Lifecycle ────────────────────────────────────────────────────────────

        /// <summary>
        ///     Được gọi 1 lần duy nhất sau khi tạo Formation.
        ///     Thường dùng để gây effect lên quân đứng sẵn trên đó.
        /// </summary>
        public virtual void OnCreated(PieceLogic piece)
        {
            OnPieceEnter(piece);
        }

        /// <summary>Được gọi khi Formation bị xóa khỏi board.</summary>
        public void OnRemove(PieceLogic piece)
        {
            OnRemoveFormation?.Invoke(this);
            if (piece != null) OnPieceExit(piece);
        }

        // ── Triggers ─────────────────────────────────────────────────────────────

        public virtual void OnCallAfterPieceAction(Action.Action action)
        {
            if (action is not IQuiets) return;

            var pieceOn = action.GetTarget() as PieceLogic;
            if (pieceOn == null) return;

            if (action.GetTargetPos() == Pos)
                OnPieceEnter(pieceOn);
            else if (action.GetFrom() == Pos)
                OnPieceExit(pieceOn);
        }

        public virtual void OnPieceSpawn(PieceLogic piece)
        {
            if (BoardUtils.IsAlive(piece) && piece.Pos == Pos)
                OnPieceEnter(piece);
        }

        // ── Piece Enter/Exit ─────────────────────────────────────────────────────

        /// <summary>
        ///     Được gọi tự động khi quân cờ đi vào vị trí của Formation.
        ///     Tương tự OnCollisionEnter() của MonoBehaviour.
        /// </summary>
        protected virtual void OnPieceEnter(PieceLogic piece)
        {
            PieceOnFormation = piece;
        }

        /// <summary>
        ///     Được gọi tự động khi quân cờ rời khỏi vị trí của Formation.
        ///     Tương tự OnCollisionExit() của MonoBehaviour.
        /// </summary>
        protected virtual void OnPieceExit(PieceLogic piece)
        {
            PieceOnFormation = null;
        }

        // ── AI ───────────────────────────────────────────────────────────────────

        public virtual int GetValueForAI()
        {
            return 0;
        }
    }
}