using Drive.Data.Entities.Models;
using Drive.Domain.Factories;
using Drive.Domain.Repositories;
using File = Drive.Data.Entities.Models.File;

namespace Drive.Presentation.Utils;

public static class CommentUtils
{
    public static void PrintAllFileComments(File file)
    {
        var comments = RepositoryFactory.Create<FileCommentRepository>().GetByFile(file);

        if (comments.Count == 0)
        {
            Console.WriteLine("There are no comments for now.");
            return;
        }

        PrintComments(comments);
    }

    public static void PrintComments(ICollection<FileComment> comments)
    {
        foreach (var comment in comments)
        {
            PrintComment(comment);
        }
    }

    public static void PrintComment(FileComment comment)
    {
        var author = RepositoryFactory.Create<UserRepository>().GetById(comment.AuthorId);

        if (author == null)
            Console.WriteLine($"{comment.Id}-Unknown\n{comment.Content}");
        else
            Console.WriteLine($"{comment.Id}-{author.Email}\n{comment.Content}");
    }
}