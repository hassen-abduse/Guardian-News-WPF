using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NewsApp.Properties;

namespace NewsApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string requestUrl = "http://content.guardianapis.com/search?api-key=01628096-8966-4965-b8f0-fd78299d408c&show-fields=all";
        
        public MainWindow()
        {
            
            InitializeComponent();

            // NoData.Visibility = Visibility.Hidden;
            // NewsList.Visibility = Visibility.Hidden;
            List<Item> News = LoadInBackground();
            try {
                
                NewsList.ItemsSource = News;
                LoadingIndicator.Visibility = Visibility.Hidden;
            }
            catch(NullReferenceException)
            
            {
                NoData.Visibility = Visibility.Visible;
            }
            catch (WebException)
            {
                NoData.Visibility = 0;
            }
            if (News == null || News.Count ==0)
            {
                NoData.Visibility = 0;
            }
            
        }
        
        // Responsible for the item click of the news listview
        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListViewItem item)
            {
                var currentItem = ((FrameworkElement)e.OriginalSource).DataContext as Item;


                System.Diagnostics.Process.Start(currentItem.WebUrl);
            }
        }
        // Loads the news data from the internet in a separate thread
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private List<Item> LoadInBackground()
        {

            
            List<Item> Items = null;
                var thread = new Thread(() =>
                 {
                     Thread.CurrentThread.IsBackground = true;
                     Items = QueryUtils.FetchNewsData(requestUrl);
                 });

                thread.Start();
                thread.Join();
                Thread.Sleep(100);
                return Items;
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //NewsList.Visibility = Visibility.Hidden;
            //LoadingIndicator.Visibility = 0;
            NoData.Visibility = Visibility.Hidden;
            List<Item> News = LoadInBackground();
            try
            {
                
                //LoadingIndicator.Visibility = Visibility.Hidden;
                
                NewsList.ItemsSource = News;
            }
            catch (NullReferenceException)

            {
                NoData.Visibility = Visibility.Visible;
            }
            if (News == null || News.Count == 0)
            {
                NoData.Visibility = 0;
            }
            
           

            //LoadingIndicator.Visibility = Visibility.Hidden;
            NewsList.Visibility = Visibility.Visible;


        }

       
    } }
