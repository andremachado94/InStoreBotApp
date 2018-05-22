using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InStoreApp.Model
{
    public class Product
    {

        public int ProductId { get; set; }

        // nome
        public string Name { get; set; }

        // preço
        public double Price { get; set; }

        // foto
        public string Photo { get; set; }

        // Processador
        public string CPU { get; set; }

        // Familia de Processador
        public string CPUFamily { get; set; }

        // Velocidade Processador
        public double CPUSpeed { get; set; }        //ghz

        // Quantidade de Núcleos Core
        public string CoreNr { get; set; }

        // RAM
        public double RAM { get; set; }             //gb

        // Tipo de Armazenamento
        public string StorageType { get; set; }

        // Armazenamento
        public double StorageAmount { get; set; }   //gb

        // Tipo de Placa Gráfica
        public string GraphicsCardType { get; set; }

        // Gráfica
        public string GraphicsCard { get; set; }

        // Memória Gráfica (Máx)
        public string MaxVideoMem { get; set; }

        // Autonomia (Estimada)
        public double Autonomy { get; set; }            //hours

        // "Placa de Som
        public string SoundCard { get; set; }

        // Câmara Incorporada
        public string HasCamera { get; set; }

        // Teclado numérico
        public string NumPad { get; set; }

        // Touch Bar
        public string TouchBar { get; set; }

        // Teclado Retroiluminado
        public string BacklitKeybr { get; set; }

        // Teclado Mecânico
        public string MechKeybr { get; set; }

        //Software
        public string Software { get; set; }

        // Sistema Operativo
        public string OS { get; set; }

        // Ecrã
        public string Screen { get; set; }

        // Diagonal do Ecrã('')
        public double ScreenDiagonal { get; set; }      //inches

        // Resolução do Ecrã
        public string ScreenResolution { get; set; }

        // Ecrã tatil
        public string TouchScreen { get; set; }

        // Referência Worten
        public string WortenRef { get; set; }

        // EAN
        public string EAN { get; set; }

        // Marca
        public string Brand { get; set; }

        // Modelo
        public string Model { get; set; }

        // Garantia
        public double Warranty { get; set; }        //years

        // Peso
        public double Weight { get; set; }          //kg

        // Cor
        public string Colour { get; set; }

        // Altura
        public double Height { get; set; }          //cm

        // Largura
        public double Width { get; set; }           //cm

        // Profundidade
        public double Depth { get; set; }           //cm

        // Garantia Bateria
        public string BatteryWarranty { get; set; }

        // Conteúdo Extra Incluído na Caixa
        public string ExtraContent { get; set; }

        // Tipo
        public string Type { get; set; }

        // Drive
        public string Drive { get; set; }

        // Conetividade
        public string Connectivity { get; set; }

        //Ligações
        public string Connections { get; set; }

        //Mais Informações
        public string MoreInfo { get; set; }

        //Part Number
        public string PartNr { get; set; }

    }

    public class ProductManager
    {
        public static List<Product> getProducts()
        {
            var listOfProducts = new List<Product>();

            listOfProducts.Add(new Product { ProductId = 1, Brand = "Asus ", Model = "Legion Y920", Name = "O Y920 vem com gráficos NVIDIA GeForce GTX Série 10, que permitem que você jogue suavemente e com taxas de frames capazes. Então avance para o jogo com toda a confiança pois este portátil oferece todo o desempenho que você precisa. Entre na arena com este laptop gaming fino, leve e portátil, que irá rodar facilmente muitos dos seus jogos.", Price = 2999, Photo = "https://www.digitalhouse.pt/51386-thickbox/portatil-lenovo-legion-y920-173-y920-17ikb-277.jpg" });
            listOfProducts.Add(new Product { ProductId = 1, Brand = "Lenovo ", Model = "Legion Y920", Name = "O Y920 vem com gráficos NVIDIA GeForce GTX Série 10, que permitem que você jogue suavemente e com taxas de frames capazes. Então avance para o jogo com toda a confiança pois este portátil oferece todo o desempenho que você precisa. Entre na arena com este laptop gaming fino, leve e portátil, que irá rodar facilmente muitos dos seus jogos.", Price = 2999, Photo = "https://www.digitalhouse.pt/51386-thickbox/portatil-lenovo-legion-y920-173-y920-17ikb-277.jpg" });
            listOfProducts.Add(new Product { ProductId = 1, Brand = "Lenovo ", Model = "Legion Y920", Name = "O Y920 vem com gráficos NVIDIA GeForce GTX Série 10, que permitem que você jogue suavemente e com taxas de frames capazes. Então avance para o jogo com toda a confiança pois este portátil oferece todo o desempenho que você precisa. Entre na arena com este laptop gaming fino, leve e portátil, que irá rodar facilmente muitos dos seus jogos.", Price = 2999, Photo = "https://www.digitalhouse.pt/51386-thickbox/portatil-lenovo-legion-y920-173-y920-17ikb-277.jpg" });
            listOfProducts.Add(new Product { ProductId = 1, Brand = "Lenovo ", Model = "Legion Y920", Name = "O Y920 vem com gráficos NVIDIA GeForce GTX Série 10, que permitem que você jogue suavemente e com taxas de frames capazes. Então avance para o jogo com toda a confiança pois este portátil oferece todo o desempenho que você precisa. Entre na arena com este laptop gaming fino, leve e portátil, que irá rodar facilmente muitos dos seus jogos.", Price = 2999, Photo = "https://www.digitalhouse.pt/51386-thickbox/portatil-lenovo-legion-y920-173-y920-17ikb-277.jpg" });
            listOfProducts.Add(new Product { ProductId = 1, Brand = "Lenovo ", Model = "Legion Y920", Name = "O Y920 vem com gráficos NVIDIA GeForce GTX Série 10, que permitem que você jogue suavemente e com taxas de frames capazes. Então avance para o jogo com toda a confiança pois este portátil oferece todo o desempenho que você precisa. Entre na arena com este laptop gaming fino, leve e portátil, que irá rodar facilmente muitos dos seus jogos.", Price = 2999, Photo = "https://www.digitalhouse.pt/51386-thickbox/portatil-lenovo-legion-y920-173-y920-17ikb-277.jpg" });
            listOfProducts.Add(new Product { ProductId = 1, Brand = "Lenovo ", Model = "Legion Y920", Name = "O Y920 vem com gráficos NVIDIA GeForce GTX Série 10, que permitem que você jogue suavemente e com taxas de frames capazes. Então avance para o jogo com toda a confiança pois este portátil oferece todo o desempenho que você precisa. Entre na arena com este laptop gaming fino, leve e portátil, que irá rodar facilmente muitos dos seus jogos.", Price = 2999, Photo = "https://www.digitalhouse.pt/51386-thickbox/portatil-lenovo-legion-y920-173-y920-17ikb-277.jpg" });
            listOfProducts.Add(new Product { ProductId = 1, Brand = "Lenovo ", Model = "Legion Y920", Name = "O Y920 vem com gráficos NVIDIA GeForce GTX Série 10, que permitem que você jogue suavemente e com taxas de frames capazes. Então avance para o jogo com toda a confiança pois este portátil oferece todo o desempenho que você precisa. Entre na arena com este laptop gaming fino, leve e portátil, que irá rodar facilmente muitos dos seus jogos.", Price = 2999, Photo = "https://www.digitalhouse.pt/51386-thickbox/portatil-lenovo-legion-y920-173-y920-17ikb-277.jpg" });
            listOfProducts.Add(new Product { ProductId = 1, Brand = "Lenovo ", Model = "Legion Y920", Name = "O Y920 vem com gráficos NVIDIA GeForce GTX Série 10, que permitem que você jogue suavemente e com taxas de frames capazes. Então avance para o jogo com toda a confiança pois este portátil oferece todo o desempenho que você precisa. Entre na arena com este laptop gaming fino, leve e portátil, que irá rodar facilmente muitos dos seus jogos.", Price = 2999, Photo = "https://www.digitalhouse.pt/51386-thickbox/portatil-lenovo-legion-y920-173-y920-17ikb-277.jpg" });
            listOfProducts.Add(new Product { ProductId = 1, Brand = "Lenovo ", Model = "Legion Y920", Name = "O Y920 vem com gráficos NVIDIA GeForce GTX Série 10, que permitem que você jogue suavemente e com taxas de frames capazes. Então avance para o jogo com toda a confiança pois este portátil oferece todo o desempenho que você precisa. Entre na arena com este laptop gaming fino, leve e portátil, que irá rodar facilmente muitos dos seus jogos.", Price = 2999, Photo = "https://www.digitalhouse.pt/51386-thickbox/portatil-lenovo-legion-y920-173-y920-17ikb-277.jpg" });

            return listOfProducts;
        }
    }
}
