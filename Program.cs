using System;

namespace DZ_Napilnik
{
    class Program
    {
        static void Main()
        {
            Item iPhone11 = new Item("IPhone 11");
            Item iPhone12 = new Item("IPhone 12");

            Warehouse warehouse = new Warehouse();

            Shop shop = new Shop(warehouse);

            warehouse.Delive(iPhone12, 10);
            warehouse.Delive(iPhone11, 1);

            shop.Info();

            Cart cart = shop.GetCart();
            cart.Add(iPhone12, 4);
            cart.Add(iPhone11, 1);

            Console.WriteLine(cart.Order().Paylink);

            cart.Info();
            shop.Info();

            cart.Order();

            cart.Info();
            shop.Info();

            cart.Add(iPhone12, 4);
            cart.Add(iPhone11, 3);

            cart.Info();
            shop.Info();

            cart.Add(iPhone12, 9); //Ошибка, после заказа со склада убираются заказанные товары
        }
    }

    public class Shop
    {
        private Warehouse _warehouse;
        private Cart _cart;

        public Shop(Warehouse warehouse)
        {
            _warehouse = warehouse;
            _cart = new Cart(warehouse);
        }

        public Cart GetCart()
        {
            return _cart;
        }

        public void Info()
        {
            _warehouse.Info();
        }
    }

    public class Cart
    {
        private List<Box> _boxes = new List<Box>();
        private Warehouse _warehouse;

        public Cart(Warehouse warehouse)
        {
            _warehouse = warehouse;
        }

        public void Add(Item item, int count)
        {
            if (_warehouse.IsCanConvey(item, count))
                _boxes.Add(new Box(item, count));
            else
                throw new IndexOutOfRangeException();
        }

        public Order Order()
        {
            _warehouse.ConveyItems(_boxes);

            List<Box> boxes = new List<Box>();

            for (int i = 0; i < _boxes.Count; i++)
                boxes.Add(_boxes[i]);

            return new Order(boxes);
        }

        public void Info()
        {
            Console.WriteLine("В корзине: ");

            for (int i = 0; i < _boxes.Count; i++)
                Console.WriteLine(_boxes[i].Item.Name + ": " + _boxes[i].Count);
        }
    }

    public class Warehouse
    {
        private List<Box> _boxes = new List<Box>();

        public void Delive(Item item, int count)
        {
            _boxes.Add(new Box(item, count));
        }

        public bool IsCanConvey(Item item, int count)
        {
            for (int i = 0; i < _boxes.Count; i++)
            {
                if (_boxes[i].Item == item && _boxes[i].Count >= count)
                    return true;
            }

            return false;
        }

        public void ConveyItems(List<Box> boxes)
        {
            for (int i = 0; i < _boxes.Count; i++)
            {
                for (int j = 0; j < boxes.Count; j++)
                {
                    if (_boxes[i].Item == boxes[j].Item)
                    {
                        _boxes[i].DeleteItems(boxes[i].Item, boxes[i].Count);
                        break;
                    }
                }
            }
        }

        public void Info()
        {
            Console.WriteLine("На складе: ");

            for (int i = 0; i < _boxes.Count; i++)
                Console.WriteLine(_boxes[i].Item.Name + ": " + _boxes[i].Count);
        }
    }

    public class Box
    {
        private Item _item;
        private int _count;

        public Box(Item item, int count)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            if (count < 0)
                throw new ArgumentOutOfRangeException("negative count");

            _item = item;
            _count = count;
        }

        public Item Item { get => _item; private set => _item = value; }
        public int Count { get => _count; private set => _count = value; }

        public void DeleteItems(Item item, int count)
        {
            if (item == _item && _count >= count)
                _count -= count;
        }
    }

    public class Order
    {
        private List<Box> _boxes;
        private int _paylink = 245;

        public Order(List<Box> boxes)
        {
            _boxes = boxes;
        }

        public int Paylink { get => _paylink; set => _paylink = value; }
    }

    public class Item
    {
        private readonly string _name;

        public Item(string name)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            _name = name;
        }

        public string Name => _name;
    }
}