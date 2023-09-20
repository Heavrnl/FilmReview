using Microsoft.AspNetCore.Identity;

namespace FilmReview.Models
{
    namespace FilmReview
    {
        public class Seed
        {
            private readonly DataContext _context;
            private readonly RoleManager<Role> _roleManager;
            private readonly UserManager<User> _userManager;

            public Seed(DataContext context, RoleManager<Role> roleManager, UserManager<User> userManager)
            {
                _context = context;
                _roleManager = roleManager;
                _userManager = userManager;
            }

            public  void SeedData()
            {

                

                if (!_context.categories.Any())
                {
                    // 添加示例类别数据
                    var categories = new List<Category>
                {
                    new Category { Name = "动作" },
                    new Category { Name = "喜剧" },
                    new Category { Name = "科幻" },
                    // 添加更多类别...
                };
                     _context.categories.AddRange(categories);
                     _context.SaveChanges();
                }

                if (!_context.countries.Any())
                {
                    // 添加示例国家数据
                    var countries = new List<Country>
                    {
                        new Country { Name = "美国" },
                        new Country { Name = "英国" },
                        new Country { Name = "加拿大" },
                        // 添加更多国家...
                    };
                     _context.countries.AddRange(countries);
                     _context.SaveChanges();
                }



                if (!_context.Users.Any())
                {
                    // 添加示例用户数据
                    var users = new List<User>
                    {
                    new User
                    {
                        UserName = "Ander",
                        CreateTime = DateTime.Now,
                        Country = _context.countries.FirstOrDefault(c => c.Name == "美国"),                        
                        // 添加其他属性...
                    },
                    new User
                    {
                        UserName = "Tom",
                        CreateTime = DateTime.Now,
                        Country = _context.countries.FirstOrDefault(c => c.Name == "英国"),

                        // 添加其他属性...
                    },
                    // 添加更多用户...
                     };




                     _context.Users.AddRange(users);
                     _context.SaveChanges();
                    //Role role1 = new Role { Name = "admin" };
                    //Role role2 = new Role { Name = "user" };
                    //var result1 =  _roleManager.Create(role1);
                    //var result2 =  _roleManager.Create(role2);

                    //foreach (var item in users)
                    //{
                    //     _userManager.AddToRole(item, "user");
                    //}

                    // _context.SaveChanges();

                }

                if (!_context.films.Any())
                {
                    // 添加示例电影数据
                    var films = new List<Film>
                {
                    new Film
                    {
                        Name = "星际穿越",
                        Description = "《星际穿越》是一部2014年上映的科幻电影，由克里斯托弗·诺兰执导和监制，马修·麦康纳、安妮·海瑟薇、杰西卡·查斯坦和迈克尔·肯恩主演。电影讲述一组宇航员通过穿越虫洞为人类寻找新家园的冒险故事。",
                        Director = "克里斯托弗·诺兰",
                        Category = _context.categories.FirstOrDefault(c => c.Name == "科幻"),
                       
                        PubDate = new DateTime(2014, 1, 1)
                        // 添加其他属性...
                    },
                    new Film
                    {
                        Name = "蝙蝠侠：黑暗骑士",
                        Description = "《蝙蝠侠：黑暗骑士》是一部于2008年上映的超级英雄电影，由克里斯托弗·诺兰编剧、监制及执导。本片是诺兰所执导的黑暗骑士三部曲中的第二部，以DC漫画旗下角色蝙蝠侠为主角，是2005年电影《蝙蝠侠：侠影之谜》的续集。",
                        Director = "克里斯托弗·诺兰",
                        Category = _context.categories.FirstOrDefault(c => c.Name == "动作"),
                        
                        PubDate = new DateTime(2008, 2, 1)
                        // 添加其他属性...
                    },
                    // 添加更多电影...
                };
                     _context.films.AddRange(films);
                     _context.SaveChanges();
                }

                if (!_context.reviews.Any())
                {
                    // 添加示例评论数据
                    var reviews = new List<Review>
                     {
                        new Review
                        {
                            Content = "Great movie!",
                            User = _context.Users.FirstOrDefault(u => u.UserName == "Tom"),
                            
                            Film = _context.films.FirstOrDefault(f => f.Name == "蝙蝠侠：黑暗骑士"),
                            // 添加其他属性...
                        },
                        new Review
                        {
                            Content = "Funny movie!",
                            User = _context.Users.FirstOrDefault(u => u.UserName == "Ander"),
                            
                            Film = _context.films.FirstOrDefault(f => f.Name == "星际穿越"),
                            // 添加其他属性...
                        },
                        // 添加更多评论...
                        };
                     _context.reviews.AddRange(reviews);
                     _context.SaveChanges();
                }

                if (!_context.ratings.Any())
                {
                    // 评分数据
                    var ratings = new List<Rating>
                     {
                        new Rating
                        {
                           User = _context.Users.FirstOrDefault(u => u.UserName == "Ander"),
                           
                           Film = _context.films.FirstOrDefault(f => f.Name == "星际穿越"),
                           Score = 3,
                        },
                        new Rating
                        {

                           User = _context.Users.FirstOrDefault(u => u.UserName == "Tom"),
                           
                           Film = _context.films.FirstOrDefault(f => f.Name == "蝙蝠侠：黑暗骑士"),
                           Score = 4,
                        },
                        // 添加更多评分...
                        };
                     _context.ratings.AddRange(ratings);
                     _context.SaveChanges();
                }

            }

           

        }
    }
}
