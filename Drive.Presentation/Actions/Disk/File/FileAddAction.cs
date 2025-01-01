﻿using Drive.Presentation.Abstractions;
using Drive.Domain.Repositories;
using Drive.Data.Entities.Models;
using Drive.Domain.Enums;
using Drive.Presentation.Extensions;
using Drive.Presentation.Helpers;
using File = Drive.Data.Entities.Models.File;

namespace Drive.Presentation.Actions.Disk
{
    public class FileAddAction : IAction
    {
        private readonly FileRepository _fileRepository;
        public string FileToAdd { get; set; }
        public Folder ParentFolder { get; set; }
        public ICollection<File> CurrentFiles { get; set; }
        public User User { get; set; }

        public string Name { get; set; } = "Add file";
        public int MenuIndex { get; set; }

        public FileAddAction(FileRepository fileRepository, string fileToAdd, Folder parentFolder, ICollection<File> currentFiles, User user)
        {
            _fileRepository = fileRepository;
            FileToAdd = fileToAdd;
            ParentFolder = parentFolder;
            CurrentFiles = currentFiles;
            User = user;
        }

        public void Open()
        {
            var fileSplitByDot = FileToAdd.Split('.');
            var fileName = fileSplitByDot[0];
            var fileExtension = fileSplitByDot[1];

            if (DiskExtensions.GetFileByName(CurrentFiles, fileName, fileExtension) != null)
            {
                Writer.Error("File with that name and extension already exists in this folder!");
                return;
            }

            if (!UserExtensions.ConfirmUserAction("Are you sure you want to add this file?"))
                return;

            var newFile = new File(fileName, fileExtension, User.Id, ParentFolder.Id);
            var fileResponse = _fileRepository.Add(newFile);

            if (fileResponse != ResponseResultType.Success)
            {
                Writer.Error("ERROR: Something went wrong with adding the file.");
                return;
            }

            CurrentFiles.Add(newFile);
            Console.WriteLine("File successfully added.");
        }
    }
}