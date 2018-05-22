using InStoreApp.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace InStoreApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ListProducts : Page
    {
        private List<Product> Products; //TODO -- Delete (just for test)

        public ListProducts()
        {
            this.InitializeComponent();
            Products = new List<Product>(); // ProductManager.getProducts();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter.GetType() == typeof(List<Product>))
            {
                Products = e.Parameter as List<Product>;
            }

        }

        private void button_infoProduct_Click(object sender, RoutedEventArgs e)
        {
            HyperlinkButton b = sender as HyperlinkButton;
            Product p = b.Tag as Product;
            Frame.Navigate(typeof(ProductDetails), p);
        }

        private void button_addWistList_Click(object sender, RoutedEventArgs e)
        {
            HyperlinkButton b = sender as HyperlinkButton;
            Product p = b.Tag as Product;
            SaySomnthing("Producto " + p.Brand + " " + p.Model + " adicionado com sucesso a sua wishList");
        }

        private void button_ProductLocation_Click(object sender, RoutedEventArgs e)
        {
            SaySomnthing("Encontre-se na 3ª prateleira ao fundo do corredor B");
        }

        private void SaySomnthing(String message)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            Page mainPage = rootFrame.Content as MainPage;
            Frame frame = mainPage.FindName("myFrame") as Frame;
            Page chatpage = frame.Content as ChatPage;
            var botAnswer = chatpage.FindName("botAnswer") as TextBlock;
            botAnswer.Text = message;
        }

    }
}
