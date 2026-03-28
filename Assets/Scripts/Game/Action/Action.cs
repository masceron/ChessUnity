using System;
using System.Collections.Generic;
using Game.Common;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;

namespace Game.Action
{
    [Flags]
    public enum ActionFlag : byte
    {
        None = 0,
        Unblockable = 1,
        Undodgeable = 1 << 1
    }

    public enum ResultFlag
    {
        Success = 0, // Action thành công
        Blocked = 1, // Shield thành công chặn ăn quân
        HardenedBlock = 2, // Hardened shield chặn
        Parry = 3, // Carapace chặn
        Miss = 4, // Blinded thành công làm quân ăn trượt
        Evade = 5, // Evasion chặn
        Incorruptible = 6, //(Trait) Sanity (Aug), Metal Mind thành công chặn hiệu ứng
        Untouchable = 7, // (Aug) Metal Spine, (Aug) Metal Regulator, (Trait) Extremophile, Thành công chặn hiệu ứng
        Unshaken = 8, // (Trait) Free Movement, (Aug) Cold Blooded, Thành công chặn hiệu ứng
        Muted = 9, //(Aug) Crown of Silience thành công chặn Commander địch sử dụng Skill.
        CantApplyEffect = 10, // Không thể áp dụng effect nói chung
        EffectResistance = 11, // Sử dụng trong trường hợp Kháng Effect
        SurvivedHit = 12, // bị ăn nhưng không chết
        SelfDestroy = 13, // Tự chết khi thực hiện ăn quân khác
        Infest = 14, // Ký sinh thay vì ăn quân
    }

    public enum TargetingType
    {
        None = 0,
        UnitTargeting = 1,
        LocationTargeting = 2,
    }

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public abstract partial class Action
    {
        public ActionFlag Flag = ActionFlag.None;
        public ResultFlag Result = ResultFlag.Success;
        private readonly int _maker;
        private readonly int _from;
        private readonly int _target = -1;
        protected TargetingType TargetingType;

        protected Action(int maker = -1, int target = -1, TargetingType targetingType = TargetingType.UnitTargeting)
        {
            _maker = BoardUtils.GetIDAt(maker);
            _from = maker;
            TargetingType = targetingType;

            if (targetingType == TargetingType.None) return;
            _target = targetingType == TargetingType.UnitTargeting ? BoardUtils.GetIDAt(target) : target;
        }

        [MemoryPackConstructor]
        protected Action()
        {
        }

        public void Execute()
        {
            Animate();
            ModifyGameState();
        }

        protected virtual void Animate()
        {
        }

        public bool IsValid()
        {
            return Result == ResultFlag.Success;
        }

        protected abstract void ModifyGameState();

        public PieceLogic GetMaker()
        {
            return BoardUtils.GetPieceByID(_maker);
        }

        public PieceLogic GetTarget()
        {
            return TargetingType == TargetingType.UnitTargeting ? BoardUtils.GetPieceByID(_target) : null;
        }

        public int GetMakerPos()
        {
            return BoardUtils.GetPieceByID(_maker).Pos;
        }

        public int GetTargetPos()
        {
            return TargetingType == TargetingType.UnitTargeting ? BoardUtils.GetPieceByID(_target).Pos : _target;
        }

        public int GetFrom()
        {
            return _from;
        }
    }

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ActionComparer : IEqualityComparer<Action>
    {
        public bool Equals(Action x, Action y)
        {
            if (x!.GetType() != y!.GetType()) return false;
            return x.GetMaker() == y.GetMaker() && x.GetTarget() == y.GetTarget();
        }

        public int GetHashCode(Action obj)
        {
            return HashCode.Combine(obj.GetTarget(), obj.GetMaker());
        }
    }
}