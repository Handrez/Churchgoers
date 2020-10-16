using Churchgoers.Common.Enums;
using Churchgoers.Web.Data.Entities;
using Churchgoers.Web.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Churchgoers.Web.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IBlobHelper _blobHelper;

        public SeedDb(DataContext context, IUserHelper userHelper, IBlobHelper blobHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _blobHelper = blobHelper;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckFieldsAsync();
            await CheckProfessionAsync();
            await CheckRolesAsync();
            await CheckUsersAsync();
            await CheckMeetingAsync();
        }

        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.Teacher.ToString());
            await _userHelper.CheckRoleAsync(UserType.Member.ToString());
        }

        private async Task CheckUsersAsync()
        {
            if (!_context.Users.Any())
            {
                await AddUsersAsync("1017-268899", "Andres", "Giraldo", "andres042099@gmail.com", "310 5437769", "Calle 82 #74 - 158", "Administrador", "Iglesia 1", UserType.Admin, "Andres");

                //Teachers
                await AddUsersAsync("0711-544488", "Charli", "D'Amelio", "teacher11@yopmail.com", "322 098 6378", "8600 Von Lights", "Profesor", "Iglesia 1", UserType.Teacher, "Teacher1");
                await AddUsersAsync("0766-240125", "Kobe", "Bryant ", "teacher22@yopmail.com", "312 794 3154", "48320 Edwina Estate", "Profesor", "Iglesia 2", UserType.Teacher, "Teacher2");
                await AddUsersAsync("6618-523035", "Naomi", "Robinson", "teacher33@yopmail.com", "305 741 7208", "512 Stroman Meadow", "Profesor", "Iglesia 3", UserType.Teacher, "Teacher3");
                await AddUsersAsync("9920-591991", "Emmily", "Thompson", "teacher44@yopmail.com", "300 768 7508", "956 Ryan Fork", "Profesor", "Iglesia 4", UserType.Teacher, "Teacher4");
                await AddUsersAsync("9317-304735", "Diego", "Dupont", "teacher55@yopmail.com", "302 887 6896", "640 Viviane Island", "Profesor", "Iglesia 5", UserType.Teacher, "Teacher5");
                await AddUsersAsync("6168-500984", "Simon", "Kristensen", "teacher66@yopmail.com", "304 029 0325", "7484 Urban Manors", "Profesor", "Iglesia 6", UserType.Teacher, "Teacher6");
                await AddUsersAsync("6259-706221", "Manolo", "Wiitala", "teacher77@yopmail.com", "300 094 0225", "91685 Stracke Springs", "Profesor", "Iglesia 7", UserType.Teacher, "Teacher7");
                await AddUsersAsync("9269-786880", "Elena", "Kumpula", "teacher88@yopmail.com", "320 539 5108", "4480 Roberts Orchard", "Profesor", "Iglesia 8", UserType.Teacher, "Teacher8");

                //Church 1
                await AddUsersAsync("9028-781739", "Saul", "Espinosa", "member1@yopmail.com", "685 290 2234", "55878 Bobby Ridges", "Medico", "Iglesia 1", UserType.Member, "Member1");
                await AddUsersAsync("7101-121021", "Alexis", "Suarez", "member2@yopmail.com", "070 810 8902", "949 Elfrieda Junctions", "Deportista", "Iglesia 1", UserType.Member, "Member2");
                await AddUsersAsync("1165-238513", "Paula", "Maestre", "member3@yopmail.com", "720 520 3377", "38476 Maymie Bridge", "Psicologo", "Iglesia 1", UserType.Member, "Member3");
                await AddUsersAsync("0905-380655", "Luna", "Lazaro", "member4@yopmail.com", "558 948 0898", "0119 Hyatt Streets", "Abogado", "Iglesia 1", UserType.Member, "Member4");
                await AddUsersAsync("7401-404385", "Rubén", "Mayor", "member5@yopmail.com", "166 211 3429", "285 DuBuque Field", "Veterinario", "Iglesia 1", UserType.Member, "Member5");

                //Church 2
                await AddUsersAsync("8078-533660", "Avelina", "Caceres", "member6@yopmail.com", "928 543 4041", "9531 Schumm Corner", "Medico", "Iglesia 2", UserType.Member, "Member6");
                await AddUsersAsync("8349-622106", "Cintia", "Raya", "member7@yopmail.com", "008 261 7458", "7765 Anastasia View", "Deportista", "Iglesia 2", UserType.Member, "Member7");
                await AddUsersAsync("0703-794945", "Juana", "Morillo", "member8@yopmail.com", "301 850 0614", "148 Kuhic Road", "Psicologo", "Iglesia 2", UserType.Member, "Member8");
                await AddUsersAsync("9457-862581", "Ceferino", "Bonilla", "member9@yopmail.com", "653 137 8693", "6463 Leopoldo Ramp", "Abogado", "Iglesia 2", UserType.Member, "Member9");
                await AddUsersAsync("2302-925252", "Raul", "Tejada", "member10@yopmail.com", "304 547 4057", "6687 Corene Well", "Veterinario", "Iglesia 2", UserType.Member, "Member10");

                //Church 3
                await AddUsersAsync("2582-729723", "César", "Villegas", "member11@yopmail.com", "212 465 3062", "87799 Lauriane Dam", "Medico", "Iglesia 3", UserType.Member, "Member11");
                await AddUsersAsync("0732-817564", "Valeriano", "Rios", "member12@yopmail.com", "538 944 9927", "6316 Kozey Valleys", "Deportista", "Iglesia 3", UserType.Member, "Member12");
                await AddUsersAsync("7251-994310", "Carmen", "Olivares", "member13@yopmail.com", "621 484 2033", "66938 Elenor Station", "Psicologo", "Iglesia 3", UserType.Member, "Member13");
                await AddUsersAsync("6218-156004", "Claudia", "Gimeno", "member14@yopmail.com", "121 140 5454", "8165 Franecki Trafficway", "Abogado", "Iglesia 3", UserType.Member, "Member14");
                await AddUsersAsync("7174-255992", "Alexandra", "Bueno", "member15@yopmail.com", "805 556 4463", "8432 Emelie Ranch", "Veterinario", "Iglesia 3", UserType.Member, "Member15");

                //Church 4
                await AddUsersAsync("5057-997068", "Jesús", "Abreu", "member16@yopmail.com", "219 332 4597", "1665 Bechtelar Key", "Medico", "Iglesia 4", UserType.Member, "Member16");
                await AddUsersAsync("0767-857079", "Naira", "Romera", "member17@yopmail.com", "380 255 9273", "858 Medhurst Walks", "Deportista", "Iglesia 4", UserType.Member, "Member17");
                await AddUsersAsync("7421-700399", "Daniel", "Serrano", "member18@yopmail.com", "822 699 9301", "5128 Verner Forge", "Psicologo", "Iglesia 4", UserType.Member, "Member18");
                await AddUsersAsync("7778-920390", "Marc", "Padilla", "member19@yopmail.com", "218 696 5574", "683 Adams Walks", "Abogado", "Iglesia 4", UserType.Member, "Member19");
                await AddUsersAsync("5764-475321", "Narciso", "Bolaños", "member20@yopmail.com", "875 575 3341", "39103 Trantow Fall", "Veterinario", "Iglesia 4", UserType.Member, "Member20");

                //Church 5
                await AddUsersAsync("9078-318579", "Roberto", "Frutos", "member21@yopmail.com", "727 295 2973", "8984 Santa Key", "Fotografo", "Iglesia 5", UserType.Member, "Member21");
                await AddUsersAsync("0265-112046", "Benito", "Martinez", "member22@yopmail.com", "826 129 3977", "70331 Hyatt Well", "Cantante", "Iglesia 5", UserType.Member, "Member22");
                await AddUsersAsync("7172-434611", "Gabriel", "Garcia", "member23@yopmail.com", "242 030 6251", "86544 Verona Turnpike", "Criminologo", "Iglesia 5", UserType.Member, "Member23");
                await AddUsersAsync("5368-616663", "Penelope", "Perez", "member24@yopmail.com", "797 558 2933", "01692 Huels Passage", "Bailarin", "Iglesia 5", UserType.Member, "Member24");
                await AddUsersAsync("2238-763572", "Fermina", "Gimeno", "member25@yopmail.com", "957 710 5691", "094 Cyrus Manor", "Arquitecto", "Iglesia 5", UserType.Member, "Member25");

                //Church 6
                await AddUsersAsync("9827-220203", "Yasmin", "Moran", "member26@yopmail.com", "743 412 8272", "499 Turner Alley", "Fotografo", "Iglesia 6", UserType.Member, "Member26");
                await AddUsersAsync("5578-294201", "Nelson", "Olivera", "member27@yopmail.com", "051 537 5535", "006 Brigitte Track", "Cantante", "Iglesia 6", UserType.Member, "Member27");
                await AddUsersAsync("7328-309322", "Tomas", "Oliver", "member28@yopmail.com", "142 571 7857", "095 Garfield Lakes", "Criminologo", "Iglesia 6", UserType.Member, "Member28");
                await AddUsersAsync("3933-490448", "Leonor", "Codina", "member29@yopmail.com", "819 372 6246", "947 Kayla Lock", "Bailarin", "Iglesia 6", UserType.Member, "Member29");
                await AddUsersAsync("2179-577818", "Teresa", "Ramirez", "member30@yopmail.com", "511 091 8601", "873 Rath Light", "Arquitecto", "Iglesia 6", UserType.Member, "Member30");

                //Church 7
                await AddUsersAsync("4979-801850", "Amaranta", "Galvan", "member31@yopmail.com", "463 330 8379", "216 Wilhelmine Village", "Fotografo", "Iglesia 7", UserType.Member, "Member31");
                await AddUsersAsync("3204-729715", "Alejandro", "Fernandez", "member32@yopmail.com", "985 514 0251", "2658 Aufderhar Via", "Cantante", "Iglesia 7", UserType.Member, "Member32");
                await AddUsersAsync("2692-686787", "Miriam", "Parra", "member33@yopmail.com", "734 767 5855", "612 Lowe Brook", "Criminologo", "Iglesia 7", UserType.Member, "Member33");
                await AddUsersAsync("3772-528440", "Ignacia", "Pacheco", "member34@yopmail.com", "488 025 1804", "928 Tevin Route", "Bailarin", "Iglesia 7", UserType.Member, "Member34");
                await AddUsersAsync("2261-414981", "Julia", "Velazquez", "member35@yopmail.com", "379 589 6257", "3974 Janice Walk", "Arquitecto", "Iglesia 7", UserType.Member, "Member35");

                //Church 8
                await AddUsersAsync("8147-045568", "Natália", "Zamora", "member36@yopmail.com", "658 722 3197", "79795 Jayde Summit", "Fotografo", "Iglesia 8", UserType.Member, "Member36");
                await AddUsersAsync("2731-165894", "Oscar", "Paredes", "member37@yopmail.com", "768 954 0596", "044 Walker Trace", "Cantante", "Iglesia 8", UserType.Member, "Member37");
                await AddUsersAsync("9726-208517", "Alícia", "Quevedo", "member38@yopmail.com", "469 787 2617", "90577 Boehm Via", "Criminologo", "Iglesia 8", UserType.Member, "Member38");
                await AddUsersAsync("4672-446468", "Cecilia", "Freire", "member39@yopmail.com", "851 280 4611", "443 Green Loop", "Bailarin", "Iglesia 8", UserType.Member, "Member39");
                await AddUsersAsync("7567-230305", "Valentina", "Ibañez", "member40@yopmail.com", "445 791 833", "798 Jadon Key", "Arquitecto", "Iglesia 8", UserType.Member, "Member40");
            }
        }

        private async Task<User> AddUsersAsync(string document, string firstName, string lastName, string email, string phone, string address, string profession, string church, UserType userType, string image)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\images", $"{image}.jpg");
            Guid imageId = await _blobHelper.UploadBlobAsync(path, "users");

            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                user = new User
                {
                    Document = document,
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Address = address,
                    Profession = _context.Professions.FirstOrDefault(p => p.Name == profession),
                    Church = _context.Churches.FirstOrDefault(c => c.Name == church),
                    UserType = userType,
                    ImageId = imageId
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());

                string token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);
            }
            await _context.SaveChangesAsync();
            return user;
        }

        private async Task CheckFieldsAsync()
        {
            if (!_context.Fields.Any())
            {
                _context.Fields.Add(new Field
                {
                    Name = "Campo 1",
                    Districts = new List<District>
                    {
                        new District
                        {
                            Name = "Distrito 1",
                            Churches = new List<Church>
                            {
                                new Church { Name = "Iglesia 1" },
                                new Church { Name = "Iglesia 2" },
                            }
                        },
                        new District
                        {
                            Name = "Distrito 2",
                            Churches = new List<Church>
                            {
                                new Church { Name = "Iglesia 3" },
                                new Church { Name = "Iglesia 4" },
                            }
                        }
                    }
                });

                _context.Fields.Add(new Field
                {
                    Name = "Campo 2",
                    Districts = new List<District>
                    {
                        new District
                        {
                            Name = "Distrito 3",
                            Churches = new List<Church>
                            {
                                new Church { Name = "Iglesia 5" },
                                new Church { Name = "Iglesia 6" },
                            }
                        },
                        new District
                        {
                            Name = "Distrito 4",
                            Churches = new List<Church>
                            {
                                new Church { Name = "Iglesia 7" },
                                new Church { Name = "Iglesia 8" },
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
                _context.Professions.Add(new Profession{Name = "Administrador",});

                _context.Professions.Add(new Profession{Name = "Profesor",});

                _context.Professions.Add(new Profession{Name = "Medico",});

                _context.Professions.Add(new Profession{Name = "Fotografo",});

                _context.Professions.Add(new Profession{Name = "Deportista",});

                _context.Professions.Add(new Profession{Name = "Cantante",});

                _context.Professions.Add(new Profession{Name = "Psicologo",});

                _context.Professions.Add(new Profession{Name = "Criminologo",});

                _context.Professions.Add(new Profession{Name = "Abogado",});

                _context.Professions.Add(new Profession{Name = "Bailarin",});

                _context.Professions.Add(new Profession{Name = "Veterinario",});

                _context.Professions.Add(new Profession{Name = "Arquitecto",});

                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckMeetingAsync()
        {
            if (!_context.Meetings.Any())
            {
                //Iglesia 1
                _context.Meetings.Add(new Meeting
                {
                    Church = _context.Churches.FirstOrDefault(c => c.Name == "Iglesia 1"),
                    Date = DateTime.Parse("01/09/2020"),
                    Assistances = new List<Assistance>
                    {
                        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member1@yopmail.com"), IsPresent = true,},
                        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member2@yopmail.com"), IsPresent = false,},
                        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member3@yopmail.com"), IsPresent = true,},
                        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member4@yopmail.com"), IsPresent = false,},
                        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member5@yopmail.com"), IsPresent = true,}
                    }
                });
                //_context.Meetings.Add(new Meeting
                //{
                //    Church = _context.Churches.FirstOrDefault(c => c.Name == "Iglesia 1"),
                //    Date = DateTime.Parse("09/09/2020"),
                //    Assistances = new List<Assistance>
                //    {
                //        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member1@yopmail.com"), IsPresent = false,},
                //        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member2@yopmail.com"), IsPresent = true,},
                //        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member3@yopmail.com"), IsPresent = false,},
                //        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member4@yopmail.com"), IsPresent = true,},
                //        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member5@yopmail.com"), IsPresent = false,}
                //    }
                //});

                //Iglesia 2
                _context.Meetings.Add(new Meeting
                {
                    Church = _context.Churches.FirstOrDefault(c => c.Name == "Iglesia 2"),
                    Date = DateTime.Parse("02/09/2020"),
                    Assistances = new List<Assistance>
                    {
                        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member6@yopmail.com"), IsPresent = false,},
                        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member7@yopmail.com"), IsPresent = true,},
                        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member8@yopmail.com"), IsPresent = false,},
                        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member9@yopmail.com"), IsPresent = true,},
                        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member10@yopmail.com"), IsPresent = false,}
                    }
                });
                //_context.Meetings.Add(new Meeting
                //{
                //    Church = _context.Churches.FirstOrDefault(c => c.Name == "Iglesia 2"),
                //    Date = DateTime.Parse("10/09/2020"),
                //    Assistances = new List<Assistance>
                //    {
                //        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member6@yopmail.com"), IsPresent = true,},
                //        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member7@yopmail.com"), IsPresent = false,},
                //        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member8@yopmail.com"), IsPresent = true,},
                //        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member9@yopmail.com"), IsPresent = false,},
                //        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member10@yopmail.com"), IsPresent = true,}
                //    }
                //});

                //Iglesia 3
                _context.Meetings.Add(new Meeting
                {
                    Church = _context.Churches.FirstOrDefault(c => c.Name == "Iglesia 3"),
                    Date = DateTime.Parse("03/09/2020"),
                    Assistances = new List<Assistance>
                    {
                        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member11@yopmail.com"), IsPresent = true,},
                        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member12@yopmail.com"), IsPresent = false,},
                        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member13@yopmail.com"), IsPresent = true,},
                        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member14@yopmail.com"), IsPresent = false,},
                        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member15@yopmail.com"), IsPresent = true,}
                    }
                });
                //_context.Meetings.Add(new Meeting
                //{
                //    Church = _context.Churches.FirstOrDefault(c => c.Name == "Iglesia 3"),
                //    Date = DateTime.Parse("11/09/2020"),
                //    Assistances = new List<Assistance>
                //    {
                //        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member11@yopmail.com"), IsPresent = false,},
                //        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member12@yopmail.com"), IsPresent = true,},
                //        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member13@yopmail.com"), IsPresent = false,},
                //        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member14@yopmail.com"), IsPresent = true,},
                //        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member15@yopmail.com"), IsPresent = false,}
                //    }
                //});

                //Iglesia 4
                _context.Meetings.Add(new Meeting
                {
                    Church = _context.Churches.FirstOrDefault(c => c.Name == "Iglesia 4"),
                    Date = DateTime.Parse("04/09/2020"),
                    Assistances = new List<Assistance>
                    {
                        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member16@yopmail.com"), IsPresent = false,},
                        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member17@yopmail.com"), IsPresent = true,},
                        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member18@yopmail.com"), IsPresent = false,},
                        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member19@yopmail.com"), IsPresent = true,},
                        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member20@yopmail.com"), IsPresent = false,}
                    }
                });
                //_context.Meetings.Add(new Meeting
                //{
                //    Church = _context.Churches.FirstOrDefault(c => c.Name == "Iglesia 4"),
                //    Date = DateTime.Parse("12/09/2020"),
                //    Assistances = new List<Assistance>
                //    {
                //        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member16@yopmail.com"), IsPresent = true,},
                //        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member17@yopmail.com"), IsPresent = false,},
                //        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member18@yopmail.com"), IsPresent = true,},
                //        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member19@yopmail.com"), IsPresent = false,},
                //        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member20@yopmail.com"), IsPresent = true,}
                //    }
                //});

                //Iglesia 5
                _context.Meetings.Add(new Meeting
                {
                    Church = _context.Churches.FirstOrDefault(c => c.Name == "Iglesia 5"),
                    Date = DateTime.Parse("05/09/2020"),
                    Assistances = new List<Assistance>
                    {
                        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member21@yopmail.com"), IsPresent = true,},
                        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member22@yopmail.com"), IsPresent = false,},
                        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member23@yopmail.com"), IsPresent = true,},
                        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member24@yopmail.com"), IsPresent = false,},
                        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member25@yopmail.com"), IsPresent = true,}
                    }
                });
                //_context.Meetings.Add(new Meeting
                //{
                //    Church = _context.Churches.FirstOrDefault(c => c.Name == "Iglesia 5"),
                //    Date = DateTime.Parse("13/09/2020"),
                //    Assistances = new List<Assistance>
                //    {
                //        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member21@yopmail.com"), IsPresent = false,},
                //        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member22@yopmail.com"), IsPresent = true,},
                //        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member23@yopmail.com"), IsPresent = false,},
                //        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member24@yopmail.com"), IsPresent = true,},
                //        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member25@yopmail.com"), IsPresent = false,}
                //    }
                //});

                //Iglesia 6
                _context.Meetings.Add(new Meeting
                {
                    Church = _context.Churches.FirstOrDefault(c => c.Name == "Iglesia 6"),
                    Date = DateTime.Parse("06/09/2020"),
                    Assistances = new List<Assistance>
                    {
                        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member26@yopmail.com"), IsPresent = false,},
                        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member27@yopmail.com"), IsPresent = true,},
                        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member28@yopmail.com"), IsPresent = false,},
                        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member29@yopmail.com"), IsPresent = true,},
                        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member30@yopmail.com"), IsPresent = false,}
                    }
                });
                //_context.Meetings.Add(new Meeting
                //{
                //    Church = _context.Churches.FirstOrDefault(c => c.Name == "Iglesia 6"),
                //    Date = DateTime.Parse("14/09/2020"),
                //    Assistances = new List<Assistance>
                //    {
                //        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member26@yopmail.com"), IsPresent = true,},
                //        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member27@yopmail.com"), IsPresent = false,},
                //        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member28@yopmail.com"), IsPresent = true,},
                //        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member29@yopmail.com"), IsPresent = false,},
                //        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member30@yopmail.com"), IsPresent = true,}
                //    }
                //});

                //Iglesia 7
                _context.Meetings.Add(new Meeting
                {
                    Church = _context.Churches.FirstOrDefault(c => c.Name == "Iglesia 7"),
                    Date = DateTime.Parse("07/09/2020"),
                    Assistances = new List<Assistance>
                    {
                        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member31@yopmail.com"), IsPresent = true,},
                        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member32@yopmail.com"), IsPresent = false,},
                        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member33@yopmail.com"), IsPresent = true,},
                        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member34@yopmail.com"), IsPresent = false,},
                        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member35@yopmail.com"), IsPresent = true,}
                    }
                });
                //_context.Meetings.Add(new Meeting
                //{
                //    Church = _context.Churches.FirstOrDefault(c => c.Name == "Iglesia 7"),
                //    Date = DateTime.Parse("15/09/2020"),
                //    Assistances = new List<Assistance>
                //    {
                //        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member31@yopmail.com"), IsPresent = false,},
                //        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member32@yopmail.com"), IsPresent = true,},
                //        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member33@yopmail.com"), IsPresent = false,},
                //        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member34@yopmail.com"), IsPresent = true,},
                //        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member35@yopmail.com"), IsPresent = false,}
                //    }
                //});

                //Iglesia 8
                _context.Meetings.Add(new Meeting
                {
                    Church = _context.Churches.FirstOrDefault(c => c.Name == "Iglesia 8"),
                    Date = DateTime.Parse("08/09/2020"),
                    Assistances = new List<Assistance>
                    {
                        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member36@yopmail.com"), IsPresent = false,},
                        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member37@yopmail.com"), IsPresent = true,},
                        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member38@yopmail.com"), IsPresent = false,},
                        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member39@yopmail.com"), IsPresent = true,},
                        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member40@yopmail.com"), IsPresent = false,}
                    }
                });
                //_context.Meetings.Add(new Meeting
                //{
                //    Church = _context.Churches.FirstOrDefault(c => c.Name == "Iglesia 8"),
                //    Date = DateTime.Parse("16/09/2020"),
                //    Assistances = new List<Assistance>
                //    {
                //        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member36@yopmail.com"), IsPresent = true,},
                //        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member37@yopmail.com"), IsPresent = false,},
                //        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member38@yopmail.com"), IsPresent = true,},
                //        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member39@yopmail.com"), IsPresent = false,},
                //        new Assistance{User = _context.Users.FirstOrDefault(u => u.Email == "member40@yopmail.com"), IsPresent = true,}
                //    }
                //});
            }
            await _context.SaveChangesAsync();
        }
    }
}