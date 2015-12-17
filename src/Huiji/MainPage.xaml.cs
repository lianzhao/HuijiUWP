using MVVMSidekick.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Huiji.ViewModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Huiji
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : MVVMPage
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.RegisterPropertyChangedCallback(ViewModelProperty, (_, __) =>
            {
                StrongTypeViewModel = this.ViewModel as MainPage_Model;
            });
            StrongTypeViewModel = this.ViewModel as MainPage_Model;
        }


    public MainPage_Model StrongTypeViewModel
        {
            get { return (MainPage_Model)GetValue(StrongTypeViewModelProperty); }
            set { SetValue(StrongTypeViewModelProperty, value); }
        }

        public static readonly DependencyProperty StrongTypeViewModelProperty =
                    DependencyProperty.Register("StrongTypeViewModel", typeof(MainPage_Model), typeof(MainPage), new PropertyMetadata(null));


        private async void MainPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var http = new HttpClient())
                {
                    var resp = await
                        http.GetAsync(
                            "http://coppermind.huiji.wiki/api.php?action=parse&page=%E6%B2%99%E5%85%B0%C2%B7%E8%BE%BE%E7%93%A6&prop=text&format=json").ConfigureAwait(false);
                    var json = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var html = JsonValue.Parse(json).GetObject().GetNamedObject("parse").GetNamedObject("text").GetNamedString("*");
                    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        webView.NavigateToString(html);
                    });
                }
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }
    }
}
