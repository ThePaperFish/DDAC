using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using DDAC2.Data;

namespace DDAC2.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {

                context.Comment.AddRange(
                    new Comment
                    {
                        PostId = 1,
                        AuthorId = "Nani",
                        Content = "Hello World for Comment",
                        PublishedDate = DateTime.Parse("2018-2-15"),
                        EditedDate = DateTime.Parse("2018-2-15")
                    },
                    new Comment
                    {
                        PostId = 1,
                        AuthorId = "Landis",
                        Content = "Hello World for Comment",
                        PublishedDate = DateTime.Parse("2018-2-13"),
                        EditedDate = DateTime.Parse("2018-2-13")
                    },
                    new Comment
                    {
                        PostId = 2,
                        AuthorId = "Landis",
                        Content = "Test Comment 2",
                        PublishedDate = DateTime.Parse("2018-2-13"),
                        EditedDate = DateTime.Parse("2018-2-13")
                    },
                    new Comment
                    {
                        PostId = 2,
                        AuthorId = "Chong",
                        Content = "Test Comment 5",
                        PublishedDate = DateTime.Parse("2018-2-15"),
                        EditedDate = DateTime.Parse("2018-2-14")
                    }
                );

                context.SaveChanges();

                context.Post.AddRange(
                    new Post
                    {
                        Title = "Hello World",
                        Content = "Hello World",
                        PublishedDate = DateTime.Parse("2018-2-12"),
                        EditedDate = DateTime.Parse("2018-2-12"),
                        Tag = "ASP.Net/FirstPost"
                    },
                    new Post
                    {
                        Title = "Second Post",
                        Content = "To find out is there more post",
                        PublishedDate = DateTime.Parse("2018-2-13"),
                        EditedDate = DateTime.Parse("2018-2-15"),
                        Tag = "ASP.Net/SecondPost"
                    }
                );

                context.SaveChanges();

            }
        }
    }
}
