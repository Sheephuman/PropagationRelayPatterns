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


            amimationViewModel = new AnimationViewModel(TextOutput);
            DataContext = amimationViewModel;
        }

        private AnimationViewModel amimationViewModel;

    }
}