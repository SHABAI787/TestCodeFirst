/*
https://docs.microsoft.com/en-us/ef/ef6/modeling/code-first/workflows/new-database


1 - добавить нужные классы

public class Blog
    {
        public int BlogId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

        public virtual List<Post> Posts { get; set; }
    }

 public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int BlogId { get; set; }
        public virtual Blog Blog { get; set; }
    }



2 - добавить контекст

 public class BloggingContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
        
    }



3 - вызываем контекст, делаем сохранение и добавление 
using (var db = new BloggingContext())
            {
                // Create and save a new Blog
                Console.Write("Enter a name for a new Blog: ");
                var name = Console.ReadLine();

                var blog = new Blog { Name = name };
                var user = new User { Username = "Имя юзера" };
                db.Blogs.Add(blog);
                db.SaveChanges();

                // Display all Blogs from the database
                var query = from b in db.Blogs
                            orderby b.Name
                            select b;

                Console.WriteLine("All blogs in the database:");
                foreach (var item in query)
                {
                    Console.WriteLine(item.Name);
                }

                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }

После этого база данных создана

4 - Выполняем первую миграцию, связываем код с базой
PM> Enable-Migrations
Теперь у нас есть первая миграция 
Если допустим нужно добавить новое поле в класс(новую колонку в таблицу SQL)
То мы просто в коде создаём новое поле, а в консоль диспетчера пакетов пишем:
     -     Add-Migration AddNewItem // AddNewItem - название миграции
	- Update-Database // Обновляем SQL базу

5 - Если нужно явно указать идентификатор использовать атрибут KEY

 public class User
    {
        [Key]
        public string Username { get; set; }
        public string DisplayName { get; set; }
    }
Есть и другие атрибуты - читать по ссылке выше



Дополнительно можно почитать тут:
Связи между таблицами
https://docs.microsoft.com/en-us/ef/ef6/fundamentals/relationships
Поиск
https://docs.microsoft.com/en-us/ef/ef6/querying/
*/



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace TestCodeFirst
{
    public class Blog
    {
        public int BlogId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

        public virtual List<Post> Posts { get; set; }
    }

    public class BloggingContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(u => u.DisplayName)
                .HasColumnName("display_name");
        }
    }

    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int BlogId { get; set; }
        public virtual Blog Blog { get; set; }
    }

    public class User
    {
        [Key]
        public string Username { get; set; }
        public string DisplayName { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {

            using (var db = new BloggingContext())
            {
                // Create and save a new Blog
                Console.Write("Enter a name for a new Blog: ");
                var name = Console.ReadLine();

                var blog = new Blog { Name = name };
                var user = new User { Username = "Имя юзера" };
                db.Blogs.Add(blog);
                db.Users.Add(user);
                db.SaveChanges();

                // Display all Blogs from the database
                var query = from b in db.Blogs
                            orderby b.Name
                            select b;

                Console.WriteLine("All blogs in the database:");
                foreach (var item in query)
                {
                    Console.WriteLine(item.Name);
                }

                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }
    }
}
