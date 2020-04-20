using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using DDAC2.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace DDAC2.Models
{
    public static class SeedData
    {

        public static async void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {

                if (!context.Comment.Any())
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
                }

                if (!context.Post.Any())
                {

                    context.Post.AddRange(
                        new Post
                        {
                            Title = "Hello World",
                            CoverPhoto = "https://d1q6f0aelx0por.cloudfront.net/product-logos/5431a80b-9ab9-486c-906a-e3d4b5ccaa96-hello-world.png",
                            Content = "Hello World",
                            PublishedDate = DateTime.Parse("2018-2-12"),
                            EditedDate = DateTime.Parse("2018-2-12"),
                            Tag = "ASP.Net/FirstPost"
                        },
                        new Post
                        {
                            Title = "Second Post",
                            CoverPhoto = "https://d1q6f0aelx0por.cloudfront.net/product-logos/5431a80b-9ab9-486c-906a-e3d4b5ccaa96-hello-world.png",
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
}
