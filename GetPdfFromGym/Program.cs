using AutoGymLogin;
using System;
using System.Collections.Generic;
using System.Security;

namespace GetPdfFromGym
{
    class Program
    {
        static void Main(string[] args)
        {
            var gym =  new GymLogin(RetrieveCredentials());

            gym.NavigateToGymWebsite();
            gym.InputUserCredentials();
            gym.InputDate();
            //gym._driver.Close();
        }

        public static SecureString GetPasswordFromConsole(string displayMessage)
        {
            SecureString pass = new SecureString();
            Console.Write(displayMessage);
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);

                // Backspace Should Not Work
                if (!char.IsControl(key.KeyChar))
                {
                    pass.AppendChar(key.KeyChar);
                    Console.Write("*");
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
                    {
                        pass.RemoveAt(pass.Length - 1);
                        Console.Write("\b \b");
                    }
                }
            }
            // Stops Receving Keys Once Enter is Pressed
            while (key.Key != ConsoleKey.Enter);
            return pass;
        }

        public static KeyValuePair<string, SecureString> RetrieveCredentials()
        {
            Console.WriteLine("Enter your username: ");
            var user = Console.ReadLine();
            var pass = GetPasswordFromConsole("Enter your password: ");

            return new KeyValuePair<string, SecureString>(user, pass);
        }
    }
}
