// CommonLib/IAnimation.cs
namespace CommonLib
{
    /// <summary>
    /// すべてのアニメーションが従う共通契約。
    /// WPFでもConsoleでも同一のインターフェイスを介して制御できる。
    /// </summary>
    public interface IAnimationAction
    {
        /// <summary>
        /// 初期化処理。必要なら外部リソースを確保する。
        /// </summary>
        Task InitializeAsync(CancellationToken token = default);

        /// <summary>
        /// 次のフレームを生成し、表示すべき文字列を返す。
        /// </summary>
        Task<string> NextFrameAsync(CancellationToken token);

        /// <summary>
        /// 終了処理。確保したリソースなどを解放する。
        /// </summary>
        Task FinalizeAsync(CancellationToken token = default);
    }
}
