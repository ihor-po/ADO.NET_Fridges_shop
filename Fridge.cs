using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FridgeShop
{
    class Fridge
    {
        private int id;
        private string mark;
        private string model;
        private string artikle;
        private double price;
        private int quantity;

        public int Id { get => id; set => id = value; }
        public string Mark { get => mark; set => mark = value; }
        public string Model { get => model; set => model = value; }
        public string Artikle { get => artikle; set => artikle = value; }
        public double Price { get => price; set => price = value; }
        public int Quantity { get => quantity; set => quantity = value; }
    }
}
