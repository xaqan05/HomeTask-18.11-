using HomeTask_18._11_.Context;
using HomeTask_18._11_.Helpers.Exceptions;
using HomeTask_18._11_.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeTask_18._11_.Services
{
    static class LoggedInMenu
    {
        public static void Start()
        {
            bool loggedMenuCondition = false;
            string loggedMenuChoice;
            do
            {
                Console.WriteLine("1.Show Products ");
                Console.WriteLine("2.Show Basket");
                Console.WriteLine("3.Log Out Account");

                Console.WriteLine(" ");

                Console.Write("Please make your choice : ");
                loggedMenuChoice = Console.ReadLine();

                switch (loggedMenuChoice)
                {
                    case "1":
                        ShowProducts();
                        break;
                    case "2":
                        ShowBasket();
                        break;
                    case "3":
                        LoggingOut();
                        loggedMenuCondition = true;
                        break;
                    default:
                        Console.WriteLine("Wrong choice");
                        break;
                }
            } while (!loggedMenuCondition);
        }
        public static void ShowProducts()
        {
            Console.Clear();
            int productId;
            List<Product> products = new List<Product>();
            using (AppDbContext context = new AppDbContext())
            {
                products = context.Products.ToList();
            }

            if (products.Count > 0)
            {
                foreach (var item in products)
                {
                    Console.WriteLine(item);
                }
            }

            bool checkId = false;
            Console.WriteLine(" ");
            do
            {
                Console.WriteLine("Select 0 to close this menu");
                Console.Write("Or enter the product ID you want to add to the cart : ");
                checkId = int.TryParse(Console.ReadLine(), out productId);
            } while (!checkId);

            Product addedProduct = new Product();

            if (productId > 0)
            {
                Console.Clear();

                try
                {
                    using (AppDbContext context = new AppDbContext())
                    {
                        addedProduct = context.Products.Find(productId);
                    }

                    if (addedProduct != null)
                    {
                        using (AppDbContext context = new AppDbContext())
                        {
                            context.Baskets.Add(new Basket
                            {
                                ProductId = productId,
                                UserId = Services.LoggedInUserId.Value
                            });
                            context.SaveChanges();
                        }
                        Console.WriteLine("Succesfulyy added in basket");
                    }
                    else
                    {
                        throw new ProductNotFoundException("This product doesn't exist.");
                    }
                }
                catch (ProductNotFoundException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Menu closed...");
            }
        }
        public static void ShowBasket()
        {
            Console.Clear();
            int productId;

            using (AppDbContext context = new AppDbContext())
            {
                var basketItems = context.Baskets.Where(b => b.UserId == Services.LoggedInUserId).ToList();

                if (basketItems.Count > 0)
                {
                    foreach (var item in basketItems)
                    {
                        Product basketProduct = context.Products.FirstOrDefault(p => p.Id == item.ProductId);
                        
                        Console.WriteLine(basketProduct);
                    }

                    bool checkId = false;
                    do
                    {
                        Console.WriteLine(" ");
                        Console.WriteLine("Select 0 to close this menu");
                        Console.Write("Or enter the product ID you want to delete : ");
                        checkId = int.TryParse(Console.ReadLine(), out productId);

                    } while (!checkId);
                    if (productId > 0)
                    {
                        try
                        {
                            Basket deletedProductId = basketItems.Find(x => x.ProductId == productId);

                            if (deletedProductId != null)
                            {
                                context.Baskets.Remove(deletedProductId);
                                context.SaveChanges();
                                Console.WriteLine(" ");
                                Console.WriteLine($"Successfully deleted product with this ID : {productId}");
                                Console.Clear();
                            }

                            else
                            {
                                Console.WriteLine(" ");
                                throw new ProductNotFoundException("This product doesn't exist in basket.");
                            }
                        }
                        catch (ProductNotFoundException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Menu closed...");
                    }
                }
                else
                {
                    Console.WriteLine(" ");
                    Console.WriteLine("Basket is empty.Please add product to basket.");
                }
            }
        }

        public static void LoggingOut()
        {
            Console.Clear();
            Console.WriteLine("Logging out...");
            Services.LoggedInUserId = null;
            Services.Start();
        }

    }
}
