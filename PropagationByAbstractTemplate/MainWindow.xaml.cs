using CommonLib;
using CommonLib.Animations;
using PropagationByAbstractTemplate.Implements;
using PropagationByAbstractTemplate.Implements.CommonLib.Animations;
using System.Windows;

namespace PropagationByAbstractTemplate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();



        }
        private IAnimationAction? _selectedAnimation;
        private CancellationTokenSource? _cts;

        public static int TextWidth { get; set; }

        private void ButtonA_Click(object sender, RoutedEventArgs e)
        {

            // 前回実行中ならキャンセル
            _cts?.Cancel();
            _cts = new CancellationTokenSource();



            _selectedAnimation = new SheepAnimation(TextOutput);
            StatusText.Text = "🐑 羊アニメが選択されました";
        }

        // ButtonB: 文字降下
        private void ButtonB_Click(object sender, RoutedEventArgs e)
        {


            // 前回実行中ならキャンセル
            _cts?.Cancel();
            _cts = new CancellationTokenSource();


            _selectedAnimation = new BirdAnimation(TextOutput);
            StatusText.Text = "🐤 鳥のアニメが選択されました";
        }

        // ButtonC: テロップ
        private void ButtonC_Click(object sender, RoutedEventArgs e)
        {

            // 前回実行中ならキャンセル
            _cts?.Cancel();
            _cts = new CancellationTokenSource();


            _selectedAnimation = new DolphinAnimation(TextOutput);
            StatusText.Text = "🐬 イルカアニメが選択されました";
        }

        private async void ExecuteButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedAnimation == null)
            {
                StatusText.Text = "先にアニメーションを選択してください。";
                return;
            }

            // 前回実行中ならキャンセル
            _cts?.Cancel();
            _cts = new CancellationTokenSource();

            try
            {
                await _selectedAnimation.InitializeAsync(_cts.Token);

                // IAsyncEnumerable を活用してフレームを逐次表示
                await foreach (var frame in (_selectedAnimation as IAsyncEnumerable<string>)!.WithCancellation(_cts.Token))
                {
                    // UI スレッドで TextBox 更新
                    Dispatcher.Invoke(() =>
                    {
                        TextOutput.Text = frame;
                        TextOutput.ScrollToEnd();
                    });
                }
            }
            catch (OperationCanceledException)
            {
                StatusText.Text = "アニメーションが停止しました。";
            }
            finally
            {
                await _selectedAnimation.FinalizeAsync(_cts.Token);
            }
        }

        private void ResumeButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedAnimation is AnimationTemplate<IAnimationAction> template)
            {
                template.Resume();
                StatusText.Text = "アニメーションを再開しました。";
            }
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedAnimation is AnimationTemplate<IAnimationAction> template)
            {
                template.Pause();
                StatusText.Text = "アニメーションを一時停止しました。";
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TextWidth = (int)TextOutput.ActualWidth;
        }
    }
}