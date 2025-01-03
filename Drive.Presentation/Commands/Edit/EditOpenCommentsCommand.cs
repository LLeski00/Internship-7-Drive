﻿using Drive.Data.Entities.Models;
using Drive.Presentation.Abstractions.Commands;
using Drive.Presentation.Actions.Disk;
using Drive.Presentation.Helpers;
using File = Drive.Data.Entities.Models.File;

namespace Drive.Presentation.Commands.Edit
{
    public class EditOpenCommentsCommand : IEditCommand
    {
        public string Name { get; set; } = "comments";
        public string Description { get; set; } = "Enters the file comments. Usage: comments";
        public User User { get; set; }
        public File File { get; set; }

        public EditOpenCommentsCommand(User user, File file)
        {
            User=user;
            File=file;
        }

        public void Execute(string? commandArguments)
        {
            if (!IsCommandValid(commandArguments))
            {
                Writer.CommandError(Name, Description);
                return;
            }

            var fileCommentsAction = new FileCommentsAction(File, User);
            fileCommentsAction.Open();
        }

        public bool IsCommandValid(string? commandArguments)
        {
            if (!string.IsNullOrEmpty(commandArguments))
                return false;

            return true;
        }
    }
}


