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
    static class Services
    {
        public static int? LoggedInUserId { get; set; }

        public static void Start()
        {
            bool mainMenuConditon = false;
            string mainMenuChoice;
            do
            {
                Console.WriteLine("1.Login");
                Console.WriteLine("2.Register");
                Console.WriteLine("0.Exit");
                Console.WriteLine(" ");

                Console.Write("Please select your choice : ");
                mainMenuChoice = Console.ReadLine();

                switch (mainMenuChoice)
                {
                    case "1":
                        LogIn();
                        break;
                    case "2":
                        Register();
                        break;
                    case "0":
                        Console.Clear();
                        Console.WriteLine("Program closed...");
                        mainMenuConditon = true;
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Wrong choice.");
                        break;
                }

            } while (!mainMenuConditon);
        }
        public static void Register()
        {
            Console.Clear();
            string name;
            string surname;
            string username;
            string password;

            Console.Write("Please enter the name : ");
            name = Console.ReadLine();

            Console.WriteLine(" ");

            Console.Write("Please enter the surname : ");
            surname = Console.ReadLine();

            Console.WriteLine(" ");

            Console.Write("Please enter the username : ");
            username = Console.ReadLine();

            Console.WriteLine(" ");

            Console.Write("Please enter the password : ");
            password = Console.ReadLine();

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(surname) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                Console.WriteLine("No information can be left blank.");
            }
            else
            {
                using (AppDbContext context = new AppDbContext())
                {

                    if (context.Users.Any(u => u.Username == username))
                    {
                        Console.WriteLine("This username has already taken.");
                    }
                    else
                    {
                        context.Users.Add(new User
                        {
                            Name = name,
                            Surname = surname,
                            Username = username,
                            Password = password
                        });
                        context.SaveChanges();
                        Console.WriteLine("Succesfully registered");
                    }
                }
            }

        }
        public static void LogIn()
        {
            string username;
            string password;
            Console.Clear();
            Console.Write("Please enter the username : ");
            username = Console.ReadLine();

            Console.WriteLine(" ");

            Console.Write("Please enter the password : ");
            password = Console.ReadLine();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                Console.WriteLine("Username or Password is not null");
            }
            else
            {
                using (AppDbContext context = new AppDbContext())
                {
                    try
                    {
                        User existingUser = context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
                        if (existingUser is not null)
                        {
                            Console.Clear();
                            Console.WriteLine($"Welcome {username} !");
                            LoggedInUserId = existingUser.Id;
                            LoggedInMenu.Start();
                        }
                        else
                        {
                            throw new UserNotFoundException("Username or password is wrong.");
                        }
                    }
                    catch (UserNotFoundException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }
    }
}
