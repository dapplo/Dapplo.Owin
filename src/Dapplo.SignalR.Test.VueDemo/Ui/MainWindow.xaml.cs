using System;
using System.Windows;
using Dapplo.SignalR.Test.VueDemo.Model;

namespace Dapplo.SignalR.Test.VueDemo.Ui;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    public MainWindow(MyVueModel myVueModel)
    {
        DataContext = myVueModel;
        InitializeComponent();
    }
}