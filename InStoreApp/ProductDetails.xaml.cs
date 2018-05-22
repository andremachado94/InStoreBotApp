﻿using InStoreApp.Model;
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
    public sealed partial class ProductDetails : Page
    {
        private List<Product> myProduct;

        public ProductDetails()
        {
            this.InitializeComponent();
            myProduct = new List<Product>();
            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if(e.Parameter.GetType() == typeof(Product))
            {
                myProduct.Add(e.Parameter as Product);
            }
           
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

        private void goBack_listProduct_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }

}
