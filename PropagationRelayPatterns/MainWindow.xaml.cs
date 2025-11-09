using CommonLib;
using Microsoft.Win32;

using System.Windows;
using System.Windows.Threading;
using WpfApp.Animations;

namespace PropagationRelayPatterns
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IAnimationAction _selectedAnimation = null!;
        private CancellationTokenSource? _cts;

        string _folderPath = string.Empty;



        public MainWindow()
        {
            InitializeComponent();

            _isTickRunning = true;
        }

        private bool _isTickRunning = false;

        // ButtonAクリック → 羊アニメを選択
        private void ButtonA_Click(object sender, RoutedEventArgs e)
        {
            _selectedAnimation = new SheepAnimation(TextOutput);

            _isTickRunning = false;
            _cts?.Cancel();


            TextOutput.Text = "SheepAnimation（羊アニメーション）が選択されました。";
        }

        // ButtonBクリック → 文字落下アニメを選択
        private void ButtonB_Click(object sender, RoutedEventArgs e)
        {
            _selectedAnimation = new RainTextAnimation();



            _isTickRunning = false;
            _cts?.Cancel();
            TextOutput.Text = "RainTextAnimation（文字落下アニメーション）が選択されました。";
        }


        // ButtonCクリック → 聖書×羊テロップアニメーションを選択
        private void ButtonC_Click(object sender, RoutedEventArgs e)
        {

            _selectedAnimation = new SheepGospelAnimation(TextOutput);

            _isTickRunning = false;
            _cts?.Cancel();

            TextOutput.Text = "SheepGospelAnimation（聖書×羊テロップ）が選択されました。";
        }



        // 実行ボタン
        private async void ExecuteButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedAnimation == null)
            {
                TextOutput.Text = "先にアニメーションを選択してください。";
                return;
            }



            // 前回実行中ならキャンセル
            _cts?.Cancel();
            _cts = new CancellationTokenSource();


            var executor = new AnimationExecutor(
                _selectedAnimation,
                outputAction: frame => Dispatcher.Invoke(() => TextOutput.Text = frame),
                100
            );
            ///注：_selectedAnimationの参照を内部でコピーしている

            try
            {
                // ここでアニメーションの初期化
                await _selectedAnimation.InitializeAsync(_cts.Token);

                await executor.RunAsync(_cts.Token);
            }
            catch (OperationCanceledException)
            {
                TextOutput.Text = "アニメーションが停止しました。";
            }
        }


        // 停止ボタン
        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            _isTickRunning = false;
            _cts?.Cancel();
        }

        private void ButtonD_Click(object sender, RoutedEventArgs e)
        {
            FilePatheSelect();
            _selectedAnimation = new ファイル列挙アクション(TextOutput, _folderPath);

            TextOutput.Text = "ファイル列挙アクションが択されました。";
        }

        private void FilePatheSelect()
        {
            // ダイアログのインスタンスを生成
            var dialog = new OpenFolderDialog();

            // ファイルの種類を設定
            dialog.DefaultDirectory = "C\\";


            // ダイアログを表示する
            if (dialog.ShowDialog() == true)
            {
                // 選択されたファイル名 (ファイルパス) をメッセージボックスに表示                
                _folderPath = dialog.FolderName;
            }
        }

        private void TextOutput_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }

        private void ScrollViewer_ScrollChanged(object sender, System.Windows.Controls.ScrollChangedEventArgs e)
        {
        }



    }

}