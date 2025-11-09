using CommonLib;
using System.Windows.Controls;

namespace WpfApp.Animations
{
    public class SheepGospelAnimation : IAnimationAction
    {
        private bool _isRunning;
        private int _lineIndex = 0;
        private readonly string[] _verses =
        {
            "羊は主の声を聞き、その名を呼ばれて応える。",
            "主は羊を青草の野に伏させ、いこいの水のほとりに伴われる。",
            "我が魂を生き返らせ、義の道に導かれる。",
            "ひつじは光の方へ進み、闇を恐れない。",
            "EndOfData — ひつじはプログラムの中にも息づく。"
        };

        private readonly TextBox _output;

        public SheepGospelAnimation(TextBox output)
        {
            _output = output;
        }

        public Task InitializeAsync(CancellationToken token = default)
        {
            _isRunning = true;
            _lineIndex = 0;
            return Task.CompletedTask;
        }

        public async Task<string> NextFrameAsync(CancellationToken token)
        {
            if (!_isRunning)
                return string.Empty;

            // TextBox の行バッファを保持
            var lines = new List<string>();

            await _output.Dispatcher.InvokeAsync(() =>
            {
                var existing = _output.Text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
                lines.AddRange(existing);
            });

            // 新しい行を追加
            lines.Add(_verses[_lineIndex]);

            // 行数が多すぎたら上から削除（縦スクロール効果）
            int maxLines = 20;
            if (lines.Count > maxLines)
                lines.RemoveRange(0, lines.Count - maxLines);

            // 出力更新
            var text = string.Join(Environment.NewLine, lines);

            await _output.Dispatcher.InvokeAsync(() =>
            {
                _output.Text = text;
                _output.ScrollToEnd(); // 下に自動スクロール
            });

            // 次の行へ
            _lineIndex = (_lineIndex + 1) % _verses.Length;

            await Task.Delay(500, token); // スクロール速度調整
            return text;
        }

        public Task FinalizeAsync(CancellationToken token = default)
        {
            _isRunning = false;
            return Task.CompletedTask;
        }
    }
}
