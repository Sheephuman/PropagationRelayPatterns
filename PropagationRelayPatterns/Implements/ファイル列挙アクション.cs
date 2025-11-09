using CommonLib;
using System.Diagnostics;
using System.Text;
using System.Windows.Controls;

public class ファイル列挙アクション : IAnimationAction
{
    private bool _isRunning;
    private Process? _process { get; set; }
    private readonly StringBuilder _buffer = new();
    private readonly TextBox _output;
    private readonly string _folderPath;

    public ファイル列挙アクション(TextBox output, string folderPath)
    {
        _output = output;
        _folderPath = folderPath;
    }


    string lastOutput = string.Empty;
    public Task InitializeAsync(CancellationToken token = default)
    {

        _isRunning = true;
        _buffer.Clear();

        var psCommand = $"Get-ChildItem -Path '{_folderPath}'| " +
                        "Select-Object LastWriteTime, Extension, Name";

        var startInfo = new ProcessStartInfo
        {
            FileName = "powershell.exe",
            Arguments = $"-NoProfile -Command \"{psCommand}\"",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        _process = new Process { StartInfo = startInfo, EnableRaisingEvents = true };
        _process.Start();

        _ = Task.Run(async () =>
        {
            if (_process == null) return;

            var lines = new List<string>();
            const int MaxLines = 1000;

            while (!_process.StandardOutput.EndOfStream &&
                   !_process.HasExited &&
                   _isRunning &&
                   !token.IsCancellationRequested)
            {
                var line = await _process.StandardOutput.ReadLineAsync();
                if (line == null) continue;

                lines.Add(line);

                // 200件取得で止める
                if (lines.Count >= MaxLines)
                {
                    _isRunning = false;
                    break;
                }

                // UIに反映（5件ごとに間引き）
                if (lines.Count % 5 == 0)
                {
                    var snapshot = string.Join(Environment.NewLine, lines);
                    await _output.Dispatcher.InvokeAsync(() =>
                    {
                        _output.Text = snapshot;
                        _output.ScrollToEnd();
                        lastOutput = snapshot; // ←ここ
                    }, System.Windows.Threading.DispatcherPriority.Background);
                }
            }

            // 最終出力
            var finalText = string.Join(Environment.NewLine, lines);
            await _output.Dispatcher.InvokeAsync(() =>
            {
                _output.Text = finalText + Environment.NewLine + "[完了: 200件取得]";
                _output.ScrollToEnd();
            });

            FinalizeAsync(token).Wait();

        }, token);

        return Task.CompletedTask;
    }





    public async Task<string> NextFrameAsync(CancellationToken token)
    {
        if (!_isRunning)
            return lastOutput;

        // フレーム単位で返すだけ
        await Task.Delay(100, token);
        return _buffer.ToString();
    }

    public Task FinalizeAsync(CancellationToken token = default)
    {
        _isRunning = false;
        try
        {
            if (_process != null && !_process.HasExited)
            {
                _process.Kill();
                _process.Dispose();
            }
        }
        catch { }

        return Task.CompletedTask;
    }
}
