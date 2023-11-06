using System.Windows;

namespace TreeNamespace
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


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Tree t = new Tree(sizeSlider.Value, reduxSlider.Value, biasSlider.Value, canvas);
        }


        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Tree t = new Tree(sizeSlider.Value, reduxSlider.Value, biasSlider.Value, canvas);
        }
    }

}
