using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeTask_18._11_.Models
{
    public class Basket
    {
        public int Id { get; set; }

        public int UserId {  get; set; }
        public User Users { get; set; }

        public int ProductId { get; set; }
        public Product Products { get; set; }
    }
}
