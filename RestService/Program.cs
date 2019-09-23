using System;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestService.Models;
using RestService.RestClient;

namespace RestService
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("~~~~~~~ Welcome to Rest Services ~~~~~~~");

            var res = string.Empty;

            while (res != "exit")
            {
                Console.WriteLine("Select a option to execute:");
                Console.WriteLine("1 - Get object");
                Console.WriteLine("2 - Get a list of object");
                Console.WriteLine("3 - Post request object");
                Console.WriteLine("4 - Post request list");
                Console.WriteLine("5 - Put object");
                Console.WriteLine("6 - Delete object");
                res = Console.ReadLine();

                if (res == "exit")
                {
                    Console.WriteLine("~~~~~~~ Exit ~~~~~~~");
                    break;
                }

                Exec(res);
            }
        }

        private static void Exec(string op)
        {
            new Action(async () =>
            {
                await Execute(op);
            }).Invoke();
        }

        private static async Task Execute(string op)
        {
            // creating the instance
            var restClient = new RequestService<posts>();

            // example of how to call some functions
            switch (op)
            {
                case "1":
                    var getPosts = await restClient.GetAllAsync("posts");
                    Console.WriteLine($"[Get All]Selected values from host: {getPosts.Count()} posts");
                    break;

                case "2":
                    var getPost = await restClient.GetAsync("posts/1");
                    Console.WriteLine($"[Get One]Selected value from host: {getPost.title}");
                    break;

                case "3":

                    var newPost = new posts
                    {
                        body = "Just a test",
                        title = "Lorem Ipsum",
                        userId = 1
                    };

                    //var example = await restClient.PostAsync("posts", JsonConvert.SerializeObject(newPost));

                    var postPost = await restClient.PostAsyncWithReturnObject("posts", newPost);
                    Console.WriteLine($"[Post One]Object was succefully save on host: {postPost.id}");
                    break;

                case "6":
                    var deletePost = await restClient.DeleteAsync("posts/1");
                    Console.WriteLine($"[Post One]Object was succefully removed from host: {deletePost}");
                    break;
            }
        }
    }
}
