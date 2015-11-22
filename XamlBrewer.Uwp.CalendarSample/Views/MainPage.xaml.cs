using Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using XamlBrewer.Uwp.CalendarSample.ViewModels;

namespace XamlBrewer.Uwp.CalendarSample
{
    public sealed partial class MainPage : Page
    {
        private MainPageViewModel vm;
        private MenuItem openItem;
        private MenuItem addItem;
        private MenuItem cleanItem;

        public MainPage()
        {
            this.InitializeComponent();

            vm = new MainPageViewModel();
            this.DataContext = vm;

            openItem = new MenuItem() { Glyph = "\uE163", Text = "Open", Command = vm.OpenCommand };
            addItem = new MenuItem() { Glyph = "\uE8D1", Text = "Add", Command = vm.AddCommand };
            cleanItem = new MenuItem() { Glyph = "\uEA99", Text = "Cleanup", Command = vm.CleanCommand };
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            vm.Menu.Add(openItem);
            vm.Menu.Add(addItem);
            vm.Menu.Add(cleanItem);
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            vm.Menu.Remove(openItem);
            vm.Menu.Remove(addItem);
            vm.Menu.Remove(cleanItem);
            base.OnNavigatedFrom(e);
        }
    }
}
