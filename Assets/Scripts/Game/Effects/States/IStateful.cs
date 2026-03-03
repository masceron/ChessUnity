namespace Game.Effects.States
{
    /// <summary>
    ///     Marker interface — đánh dấu một <see cref="Game.Effects.Effect"/> là State Effect.
    ///     Mỗi quân cờ chỉ được mang tối đa 1 Effect implement interface này tại 1 thời điểm.
    /// </summary>
    public interface IStateful
    {
        /// <summary>Loại State mà Effect này đại diện.</summary>
        StateType StateType { get; }
    }
}
