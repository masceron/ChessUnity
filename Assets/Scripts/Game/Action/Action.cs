using System;
using System.Collections.Generic;
using Game.Common;
using Game.Piece.PieceLogic.Commons;
using Game.Tile;
using MemoryPack;
// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global

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
        Unit = 1,
        Location = 2,
    }

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public abstract partial class Action
    {
        public ActionFlag Flag = ActionFlag.None;
        public ResultFlag Result = ResultFlag.Success;
        
        [MemoryPackInclude] protected int Maker;
        [MemoryPackInclude] protected int From;
        [MemoryPackInclude] protected int Target = -1;
        [MemoryPackInclude] protected TargetingType TargetingType;

        protected Action(Entity maker, Entity target)
        {
            Maker = maker?.ID ?? -1;
            From = maker?.Pos ?? -1;
            TargetingType = TargetingType.Unit;

            Target = target?.ID ?? -1;
        }

        protected Action(Entity maker, int target)
        {
            Maker = maker?.ID ?? -1;
            From = maker?.Pos ?? -1;

            TargetingType = TargetingType.Location;
            Target = target;
        }

        protected Action(Entity maker)
        {
            Maker = maker?.ID ?? -1;
            From = maker?.Pos ?? -1;
            TargetingType = TargetingType.Unit;
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

        public PieceLogic GetMakerAsPiece()
        {
            return BoardUtils.GetEntityByID(Maker) as PieceLogic;
        }

        public Formation GetMakerAsFormation()
        {
            return BoardUtils.GetEntityByID(Maker) as Formation;
        }

        public PieceLogic GetTargetAsPiece()
        {
            return TargetingType == TargetingType.Unit ? BoardUtils.GetEntityByID(Target) as PieceLogic : null;
        }

        public Formation GetTargetAsFormation()
        {
            return TargetingType == TargetingType.Unit ? BoardUtils.GetEntityByID(Target) as Formation : null;
        }

        public int GetTargetPos()
        {
            return TargetingType == TargetingType.Unit ? BoardUtils.GetEntityByID(Target).Pos : Target;
        }

        public int GetFrom()
        {
            return From;
        }

        public void ChangeTarget(PieceLogic target)
        {
            Target = target.ID;
        }

        public void ChangeTarget(int index)
        {
            Target = index;
        }

        public TargetingType GetTargetingType()
        {
            return TargetingType;
        }
    }

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ActionComparer : IEqualityComparer<Action>
    {
        public bool Equals(Action x, Action y)
        {
            if (x!.GetType() != y!.GetType()) return false;
            if (x.GetTargetingType() != y.GetTargetingType()) return false;
            return x.GetMakerAsPiece() == y.GetMakerAsPiece() && (
                x.GetTargetingType() == TargetingType.Unit
                    ? x.GetTargetAsPiece() == y.GetTargetAsPiece()
                    : x.GetTargetPos() == y.GetTargetPos()
            );
        }

        public int GetHashCode(Action obj)
        {
            return HashCode.Combine(obj.GetType(), obj.GetMakerAsPiece()?.ID, obj.GetTargetPos());
        }
    }
}