using Dapplo.SignalR.Test.VueDemo.Model;

namespace Dapplo.SignalR.Test.VueDemo.Ui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow(IMyVueModel myVueModel)
        {
            DataContext = myVueModel;
            InitializeComponent();
        }
    }
}
