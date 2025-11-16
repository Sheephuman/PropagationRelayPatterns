using CommonLib;
using CommonLib.Animations;
using PropagationByAbstractTemplate.Implements.CommonLib.Animations;
using System.Windows.Controls;

namespace PropagationByAbstractTemplate
{
    internal class AnimationViewModel : BindableBase
    {

        private IAnimationAction? _selectedAnimation;
        private CancellationTokenSource? _cts;


        public DelegateCommand ACommand { get; }
        public DelegateCommand BCommand { get; }

        public DelegateCommand CCommand { get; }

        public DelegateCommand ExecuteCommand { get; }

        public DelegateCommand PauseCommand { get; }
        public DelegateCommand ResumeCommand { get; }


        TextBox _outputBox;

        public AnimationViewModel(TextBox textBox)
        {
            ACommand = new DelegateCommand(() => CallSheepAnimetion());
            BCommand = new DelegateCommand(() => CallBirdAnimation());
            CCommand = new DelegateCommand(() => CallDolphineAnimation());
            ExecuteCommand = new DelegateCommand(() => ExecuteMethod());
            PauseCommand = new DelegateCommand(() => PauseMethod());
            ResumeCommand = new DelegateCommand(() => ResumeMethod());

            _outputBox = textBox;
        }

        private void ResumeMethod()
        {
            if (_selectedAnimation is AnimationTemplate<IAnimationAction> template)
            {
                template.Resume();
                StatusString = "アニメーションを再開しました。";
            }
        }

        private void PauseMethod()
        {
            if (_selectedAnimation is AnimationTemplate<IAnimationAction> template)
            {
                template.Pause();
                StatusString = "アニメーションを一時停止しました。";
            }
        }

        private async void ExecuteMethod()
        {
            if (_selectedAnimation == null)
            {
                StatusString = "先にアニメーションを選択してください。";
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
                    _outputBox.Dispatcher.Invoke(() =>
                      {
                          _outputBox.Text = frame;
                          _outputBox.ScrollToEnd();
                      });
                }
            }
            catch (OperationCanceledException)
            {
                StatusString = "アニメーションが停止しました。";
            }
            finally
            {
                await _selectedAnimation.FinalizeAsync(_cts.Token);
            }
        }

        private void CallDolphineAnimation()
        {        // 前回実行中ならキャンセル
            _cts?.Cancel();
            _cts = new CancellationTokenSource();


            _selectedAnimation = new DolphinAnimation(_outputBox);
            StatusString = "🐬 イルカアニメが選択されました";
        }

        private void CallBirdAnimation()
        {

            // 前回実行中ならキャンセル
            _cts?.Cancel();
            _cts = new CancellationTokenSource();


            _selectedAnimation = new BirdAnimation(_outputBox);
            StatusString = "🐤 鳥のアニメが選択されました";
        }

        string _statausStromg = "処理未選択";

        public string StatusString
        {
            get => _statausStromg;
            set => SetProperty(ref _statausStromg, value);
        }



        private void CallSheepAnimetion()
        {

            // 前回実行中ならキャンセル
            _cts?.Cancel();
            _cts = new CancellationTokenSource();

            _selectedAnimation = new SheepAnimation(_outputBox);
            StatusString = "🐤 羊のアニメが選択されました";



        }
    }
}
