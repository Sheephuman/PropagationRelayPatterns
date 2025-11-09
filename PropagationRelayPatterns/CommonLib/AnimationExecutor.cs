// CommonLib/AnimationExecutor.cs
using System.Windows.Threading;

namespace CommonLib
{
    /// <summary>
    /// DIで受け取った IAnimation 実装を実行する共通制御クラス。
    /// 出力先(Action<string>)を注入することで、UI・Consoleの両対応を実現する。
    /// </summary>
    public sealed class AnimationExecutor
    {
        private readonly IAnimationAction _animation;
        private readonly Action<string> _outputAction;
        private readonly int _frameDelayMillis;

        public AnimationExecutor(IAnimationAction animation, Action<string> outputAction, int frameDelayMiallis = 100)
        {
            _animation = animation ?? throw new ArgumentNullException(nameof(animation));
            _outputAction = outputAction ?? throw new ArgumentNullException(nameof(outputAction));
            _frameDelayMillis = frameDelayMiallis;
        }






        /// <summary>
        /// 非同期でアニメーションを実行する。
        /// </summary>
        public async Task RunAsync(CancellationToken token = default)
        {
            await _animation.InitializeAsync(token);

            try
            {
                while (!token.IsCancellationRequested)
                {
                    var frame = await _animation.NextFrameAsync(token);
                    Dispatcher _outputDispatcher = Dispatcher.CurrentDispatcher;

                    _outputDispatcher.Invoke(() => _outputAction(frame));

                    await Task.Delay(_frameDelayMillis, token);
                }
            }
            // 最後に必ず開放（例外が発生しても確実に実行される）
            finally
            {
                await _animation.FinalizeAsync(token);
            }
        }

    }
}
