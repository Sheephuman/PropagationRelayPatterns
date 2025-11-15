using System.Text;
using System.Windows.Controls;

namespace CommonLib.Animations
{
    public abstract class AnimationTemplate<T> : IAnimationAction, IAsyncEnumerable<string>
        where T : IAnimationAction
    {
        protected List<string> _frames = new();
        protected int _frameIndex = 0;

        protected readonly TextBox _output;
        protected bool _isRunning;
        private bool _isPaused;
        protected int _offset { get; set; }

        protected virtual void AdvanceFrame()
        {
            _frameIndex++;

            if (_frameIndex >= _frames.Count)
                _frameIndex = 0;
        }

        protected async Task<string> RenderFrameAsync(string rawFrame, CancellationToken token)
        {
            var sb = new StringBuilder();
            string[] lines = rawFrame.Split("\n");

            int pad = Math.Max(0, _offset);

            foreach (var line in lines)
                sb.AppendLine(new string(' ', pad) + line);

            await _output.Dispatcher.InvokeAsync(() =>
            {
                _output.Text = sb.ToString();
                _output.ScrollToEnd();
            });

            // offset を左に移動
            _offset--;
            if (_offset < 0)
                _offset = returnOfset();

            // 描画間隔
            await Task.Delay(150, token);

            return sb.ToString();
        }


        protected AnimationTemplate(TextBox output)
        {
            _output = output;

            _offset = returnOfset();
        }

        public int returnOfset()
        {
            int result = (int)_output.ActualWidth / 12; // 初期位置を右端に
            return result;
        }

        public virtual Task InitializeAsync(CancellationToken token = default)
        {
            _isRunning = true;
            _isPaused = false;
            //_offset = 0;


            _offset = returnOfset(); // 初期位置を右端に
            return Task.CompletedTask;
        }

        public virtual Task FinalizeAsync(CancellationToken token = default)
        {
            _isRunning = false;
            return Task.CompletedTask;
        }

        // 派生クラスで AA を返す
        protected abstract List<string> GetFrames();

        // 次のフレーム生成
        protected virtual async Task<string> GenerateFrameAsync(CancellationToken token)
        {
            var frames = GetFrames();
            var sb = new StringBuilder();

            double width = 0;
            await _output.Dispatcher.InvokeAsync(() => width = _output.ActualWidth / 7);

            var currentFrame = frames[0].Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

            foreach (var line in currentFrame)
            {
                int pad = Math.Max(0, _offset);
                sb.AppendLine(new string(' ', pad) + line);
            }

            await _output.Dispatcher.InvokeAsync(() =>
            {
                _output.Text = sb.ToString();
                _output.ScrollToEnd();
            });

            // オフセット更新
            _offset--;
            if (_offset < 0) // 左端まで行ったら戻す
                _offset = returnOfset(); // 右端の位置にリセット

            await Task.Delay(150, token);
            return sb.ToString();
        }

        public async Task<string> NextFrameAsync(CancellationToken token)
        {
            if (!_isRunning) return string.Empty;

            while (_isPaused)
                await Task.Delay(50, token);



            string current = _frames[_frameIndex];

            // 共通描画
            string rendered = await RenderFrameAsync(current, token);

            // フレームを次に進める
            AdvanceFrame();

            return rendered;
        }


        public void Pause() => _isPaused = true;
        public void Resume() => _isPaused = false;

        public async IAsyncEnumerator<string> GetAsyncEnumerator(CancellationToken token = default)
        {
            while (_isRunning && !token.IsCancellationRequested)
                yield return await NextFrameAsync(token);
        }
    }
}
