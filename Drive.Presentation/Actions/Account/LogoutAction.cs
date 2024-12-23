﻿using Drive.Data.Entities.Models;
using Drive.Presentation.Abstractions;
using Drive.Presentation.Helpers;
using Drive.Domain.Repositories;
using Drive.Presentation.Extensions;

namespace Drive.Presentation.Actions.Account
{
    public class LogoutAction : IAction
    {
        public string Name { get; set; } = "Log out";
        public int MenuIndex { get; set; }

        public LogoutAction()
        {
        }

        public void Open()
        {
            Console.WriteLine("You have been logged out.");
            Console.ReadLine();
        }
    }
}