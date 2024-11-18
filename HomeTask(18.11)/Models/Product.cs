using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeTask_18._11_.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public double Price { get; set; }

        public List<Basket> Baskets { get; set; }

        public override string ToString()
        {
            return ShowInfo();
        }

        public string ShowInfo()
        {
            return $"Id: {Id}, Name: {Name}, Price: {Price}$";
        }
    }
}
