using Cysharp.Threading.Tasks;

namespace UX.UI.Common
{
    public enum InGameMenuType
    {
        PauseMenu,
        ChrysosShop,
        ChrysosShopItem,
        ThalassosShop,
        ThalassosItem,
        FreePlayTest
    }
    
    public interface IAwaitableUI<in TPayload, TResult>: ICloseableUI
    {
        UniTask<TResult> WaitForSelection(TPayload payload);

        void Confirm(TResult result);
        void Cancel();
    }
}