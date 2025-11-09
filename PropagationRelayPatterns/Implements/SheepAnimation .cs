using CommonLib;
using System.Text;
using System.Windows.Controls;

public sealed class SheepAnimation : IAnimationAction
{
    private CancellationTokenSource? _cts;
    private bool _isRunning;
    private int _frameIndex;
    private int _offset;
    private readonly TextBox _output;
    private bool _isMovingRight = true; // ← 右方向フラグを追加
    string lastOutput = string.Empty;

    private readonly List<string> _frames = new()
    {
@"
  __  __  
 (oo)\____
 (__)\    )\
     ||--|| *",
@"
  __  __  
 (oo)\____
 (__)\    )\
     ||--||Z",
@"
  __  __  
 (oo)\____
 (__)\    )\
     ||--||z",
@"
  __  __  
 (oo)\____
 (__)\    )\
     ||--||"
    };

    public SheepAnimation(TextBox output)
    {
        _output = output;
    }

    public Task InitializeAsync(CancellationToken token = default)
    {
        _cts = CancellationTokenSource.CreateLinkedTokenSource(token);
        _isRunning = true;
        _frameIndex = 0;

        double textBoxWidth = 0;
        _output.Dispatcher.Invoke(() =>
        {
            textBoxWidth = _output.ActualWidth / 7; // 文字換算
        });
        _offset = (int)(textBoxWidth) - 50; // 右端寄りから開始
        return Task.CompletedTask;
    }

    public async Task<string> NextFrameAsync(CancellationToken token)
    {
        if (!_isRunning)
            return lastOutput;

        string frame = _frames[_frameIndex];
        var lines = frame.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

        double textBoxWidth = 0;
        await _output.Dispatcher.InvokeAsync(() =>
        {
            textBoxWidth = _output.ActualWidth / 7; // 文字幅換算
        });

        var sb = new StringBuilder();

        // 安全にオフセットを制御（負数はゼロ扱い）
        int safeOffset = Math.Max(0, _offset);

        foreach (var line in lines)
        {
            string spaces = new string(' ', safeOffset);
            sb.AppendLine(spaces + line);
        }

        await _output.Dispatcher.InvokeAsync(() =>
        {
            _output.Text = sb.ToString();
            _output.ScrollToEnd();
            lastOutput = _output.Text;
        });

        // フレーム進行
        _frameIndex = (_frameIndex + 1) % _frames.Count;

        // 移動処理：左右に往復する
        if (!_isMovingRight)
        {
            _offset++;
            if (_offset > textBoxWidth - 50)
                _isMovingRight = true; // 右端到達で反転
        }
        else
        {
            _offset--;
            if (_offset <= 0)
                _isMovingRight = false; // 左端到達で反転
        }

        await Task.Delay(120, token);
        return sb.ToString();
    }

    public Task FinalizeAsync(CancellationToken token = default)
    {
        _isRunning = false;
        _cts?.Cancel();
        return Task.CompletedTask;
    }
}
