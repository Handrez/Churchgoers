using Churchgoers.Common.Entities;
using Churchgoers.Common.Enums;
using Churchgoers.Web.Data.Entities;
using Churchgoers.Web.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Churchgoers.Web.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckFieldsAsync();
            await CheckProfessionAsync();
            await CheckRolesAsync();
            await CheckUserAsync("1017268899", "Andres", "Giraldo Perez", "andres042099@gmail.com", "3105437769", "Calle Luna Calle Sol", UserType.Admin);
        }

        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.User.ToString());
        }

        private async Task<User> CheckUserAsync(
            string document,
            string firstName,
            string lastName,
            string email,
            string phone,
            string address,
            UserType userType)
        {
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Address = address,
                    Document = document,
                    Church = _context.Churches.FirstOrDefault(),
                    Profession = _context.Professions.FirstOrDefault(),
                    UserType = userType
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());

                string token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);
            }

            return user;
        }

        private async Task CheckFieldsAsync()
        {
            if (!_context.Fields.Any())
            {
                _context.Fields.Add(new Field
                {
                    Name = "Field 1",
                    Districts = new List<District>
                    {
                        new District
                        {
                            Name = "District 1",
                            Churches = new List<Church>
                            {
                                new Church { Name = "Church 1" },
                                new Church { Name = "Church 2" },
                            }
                        },
                        new District
                        {
                            Name = "District 2",
                            Churches = new List<Church>
                            {
                                new Church { Name = "Church 3" },
                                new Church { Name = "Church 4" },
                            }
                        }
                    }
                });

                _context.Fields.Add(new Field
                {
                    Name = "Field 2",
                    Districts = new List<District>
                    {
                        new District
                        {
                            Name = "District 3",
                            Churches = new List<Church>
                            {
                                new Church { Name = "Church 5" },
                                new Church { Name = "Church 6" },
                            }
                        },
                        new District
                        {
                            Name = "District 4",
                            Churches = new List<Church>
                            {
                                new Church { Name = "Church 7" },
                                new Church { Name = "Church 8" },
                            }
                        }
                    }
                });

                _context.Fields.Add(new Field
                {
                    Name = "Field 3",
                    Districts = new List<District>
                    {
                        new District
                        {
                            Name = "District 5",
                            Churches = new List<Church>
                            {
                                new Church { Name = "Church 9" },
                                new Church { Name = "Church 10" },
                            }
                        },
                        new District
                        {
                            Name = "District 6",
                            Churches = new List<Church>
                            {
                                new Church { Name = "Church 11" },
                                new Church { Name = "Church 12" },
                            }
                        }
                    }
                });
                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckProfessionAsync()
        {
            if (!_context.Professions.Any())
            {
                _context.Professions.Add(new Profession
                {
                    Name = "Profession 1",
                });

                _context.Professions.Add(new Profession
                {
                    Name = "Profession 2",
                });

                _context.Professions.Add(new Profession
                {
                    Name = "Profession 3",
                });
                await _context.SaveChangesAsync();
            }
        }
    }
}
