using CommonLib;
using System.Text;
using System.Windows.Controls;

namespace WpfApp.Animations
{
    public class RainTextAnimation : IAnimationAction
    {
        private bool _isRunning;
        private CancellationTokenSource? _cts;




        public Task FinalizeAsync(CancellationToken token = default)
        {
            return Task.CompletedTask;
        }



        public async Task InitializeAsync(CancellationToken token = default)
        {
            _cts = CancellationTokenSource.CreateLinkedTokenSource(token);
            _isRunning = true;
            await Task.CompletedTask;
        }

        public async Task<string> NextFrameAsync(CancellationToken token)
        {
            var random = new Random();
            var width = 50;      // 表示幅（文字数）
            var height = 20;     // 表示高さ（行数）
            var buffer = new char[height, width];
            var sb = new StringBuilder();

            // 初期化（前フレームを考慮しない単発フレーム）
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    buffer[y, x] = ' ';

            // ランダムな文字を降らせる
            for (int i = 0; i < width / 3; i++)
            {
                int x = random.Next(width);
                int y = random.Next(height);
                buffer[y, x] = (char)random.Next(33, 126); // ASCII可視文字
            }

            // 出力組み立て
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                    sb.Append(buffer[y, x]);
                sb.AppendLine();
            }

            await Task.Delay(100, token);
            return sb.ToString();
        }

        public async Task RunAsync(TextBlock output, CancellationToken token)
        {
            _isRunning = true;
            var random = new Random();
            var width = 50;      // 表示幅（文字数）
            var height = 20;     // 表示高さ（行数）
            var buffer = new char[height, width];

            // 初期化
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    buffer[y, x] = ' ';

            var sb = new StringBuilder();

            while (!token.IsCancellationRequested && _isRunning)
            {
                // 新しい文字を一列分落とす
                for (int x = 0; x < width; x++)
                {
                    char c = (char)random.Next(33, 126); // 可視文字範囲
                    buffer[0, x] = c;
                }

                // 下に1行ずつずらす
                for (int y = height - 1; y > 0; y--)
                {
                    for (int x = 0; x < width; x++)
                        buffer[y, x] = buffer[y - 1, x];
                }

                // 上の行をクリア
                for (int x = 0; x < width; x++)
                    buffer[0, x] = ' ';

                // ランダムな位置に新しい文字を降らせる
                for (int i = 0; i < width / 3; i++)
                {
                    int x = random.Next(width);
                    buffer[0, x] = (char)random.Next(33, 126);
                }

                // 出力組み立て
                sb.Clear();
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                        sb.Append(buffer[y, x]);
                    sb.AppendLine();
                }

                await output.Dispatcher.InvokeAsync(() =>
                {
                    output.Text = sb.ToString();
                });

                await Task.Delay(100, token);
            }
        }

    }
}
