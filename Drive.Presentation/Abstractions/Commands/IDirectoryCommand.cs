using Drive.Data.Entities.Models;
using File = Drive.Data.Entities.Models.File;

namespace Drive.Presentation.Abstractions.Commands;

public interface IDirectoryCommand : ICommand
{
    void Execute(ref Folder currentDirectory, ICollection<Folder> currentFolders, ICollection<File> currentFiles, string? commandArguments);
}