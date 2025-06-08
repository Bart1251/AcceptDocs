using AcceptDocs.Domain.Models;

namespace AcceptDocs.Infrastructure
{
    public class DataSeeder
    {
        private readonly AppDbContext _context;

        public DataSeeder(AppDbContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            if (_context.Database.CanConnect()) {
                if (!_context.DocumentTypes.Any()) {
                    _context.AddRange(new List<DocumentType>() {
                        new DocumentType() {
                            DocumentTypeId = 1,
                            Name = "Faktura",
                        },
                        new DocumentType() {
                            DocumentTypeId = 2,
                            Name = "Zapotrzebowanie zakupowe",
                        },
                        new DocumentType() {
                            DocumentTypeId = 3,
                            Name = "Zamówienie",
                        },
                        new DocumentType() {
                            DocumentTypeId = 4,
                            Name = "Umowa z kontrahentem",
                        },
                    });
                }

                if (!_context.PositionLevels.Any()) {
                    _context.AddRange(new List<PositionLevel>() {
                        new PositionLevel() {
                            PositionLevelId = 1,
                            Name = "Prezes",
                        },
                        new PositionLevel() {
                            PositionLevelId = 2,
                            Name = "Dyrektor",
                        },
                        new PositionLevel() {
                            PositionLevelId = 3,
                            Name = "Menedżer",
                        },
                        new PositionLevel() {
                            PositionLevelId = 4,
                            Name = "Starszy specjalista",
                        },
                        new PositionLevel() {
                            PositionLevelId = 5,
                            Name = "Specjalista",
                        },
                        new PositionLevel() {
                            PositionLevelId = 6,
                            Name = "Młodszy specjalista",
                        },
                        new PositionLevel() {
                            PositionLevelId = 7,
                            Name = "Stażysta",
                        },
                    });
                }

                if (!_context.Users.Any()) {
                    _context.AddRange(new List<User>() {
                        new User() {
                            UserId = 1,
                            FirstName = "Jan",
                            LastName = "Kowalski",
                            Login = "JanKowalski",
                            Password = BCrypt.Net.BCrypt.HashPassword("12345"),
                            Position = "ds. Zakupów",
                            PositionLevelId = 5,
                        },
                        new User()
                        {
                            UserId = 2,
                            FirstName = "Marta",
                            LastName = "Nowak",
                            Login = "MartaNowak",
                            Password = BCrypt.Net.BCrypt.HashPassword("12345"),
                            Position = "Działu Zakupów",
                            PositionLevelId = 3,
                        },
                        new User()
                        {
                            UserId = 3,
                            FirstName = "Anna",
                            LastName = "Kot",
                            Login = "AnnaKot",
                            Password = BCrypt.Net.BCrypt.HashPassword("12345"),
                            Position = "Operacyjny",
                            PositionLevelId = 2,
                        },
                        new User()
                        {
                            UserId = 4,
                            FirstName = "Piotr",
                            LastName = "Krawczyk",
                            Login = "PiotrKrawczyk",
                            Password = BCrypt.Net.BCrypt.HashPassword("12345"),
                            Position = "Księgowy",
                            PositionLevelId = 4,
                        },
                        new User()
                        {
                            UserId = 5,
                            FirstName = "Leokadia",
                            LastName = "Gawron",
                            Login = "LeokadiaGawron",
                            Password = BCrypt.Net.BCrypt.HashPassword("12345"),
                            Position = "Zarządu",
                            PositionLevelId = 1,
                        },
                        new User()
                        {
                            UserId = 6,
                            FirstName = "Krzysztof",
                            LastName = "Orzech",
                            Login = "KrzysztofOrzech",
                            Password = BCrypt.Net.BCrypt.HashPassword("12345"),
                            Position = "Działu IT",
                            PositionLevelId = 3,
                        },
                        new User() {
                            UserId = 7,
                            FirstName = "Julia",
                            LastName = "Florek",
                            Login = "JuliaFlorek",
                            Password = BCrypt.Net.BCrypt.HashPassword("12345"),
                            Position = "ds. Sprzedaży",
                            PositionLevelId = 5,
                        },
                    });
                }

                if (!_context.DocumentFlows.Any()) {
                    _context.AddRange(new List<DocumentFlow>()
                    {
                        new DocumentFlow() {
                            DocumentFlowId = 1,
                            Name = "Faktura sprzedaży",
                            Description = "Przepływ akceptacji faktur sprzedaży, sekwencyjny. Dokument przechodzi kolejno przez weryfikację przez osoby przypisane, wg ich maksymalnej kwoty. Dokument zmienia status na zaakceptowany po zatwierdzeniu przez wszystkich użytkowników przypisanych do przepływu.",
                            SelectionMethod = SelectionMethod.AllSequential,
                            DocumentFlowUsers = new List<DocumentFlowUser>
                            {
                                new DocumentFlowUser() {
                                    DocumentFlowId = 1,
                                    UserId = 1,
                                    Value = 1000
                                },
                                new DocumentFlowUser() {
                                    DocumentFlowId = 1,
                                    UserId = 2,
                                    Value = 5000
                                },
                                new DocumentFlowUser() {
                                    DocumentFlowId = 1,
                                    UserId = 4,
                                    Value = 10000
                                },
                                new DocumentFlowUser() {
                                    DocumentFlowId = 1,
                                    UserId = 5,
                                    Value = 100000
                                },
                            }
                        },
                        new DocumentFlow() {
                            DocumentFlowId = 2,
                            Name = "Zapotrzebowanie zakupowe w dziale IT",
                            Description = "Wysyła prośbę o zatwierdzenie dokumentu wszystkim przypisanym użytkownikom jednocześnie. Dokument zmienia status na zaakceptowany po zatwierdzeniu przez wszystkich użytkowników przypisanych do przepływu.",
                            SelectionMethod = SelectionMethod.All,
                            DocumentFlowUsers = new List<DocumentFlowUser>
                            {
                                new DocumentFlowUser() {
                                    DocumentFlowId = 2,
                                    UserId = 6,
                                    Value = 100
                                },
                                new DocumentFlowUser() {
                                    DocumentFlowId = 2,
                                    UserId = 2,
                                    Value = 300
                                },
                                new DocumentFlowUser() {
                                    DocumentFlowId = 2,
                                    UserId = 4,
                                    Value = 900
                                }
                            }
                        },
                        new DocumentFlow() {
                            DocumentFlowId = 3,
                            Name = "Umowa o współpracy handlowej",
                            Description = "Flow akceptacji umowy o współpracy handlowej jest sekwencyjny. Wysyła prośbę o sprawdzenie dokumentu użytkownikom przypisanym do przepływu po kolei według ich maksymalnej kwoty aż do użytkownika którego kwota przekracza kwotę dokumentu.",
                            SelectionMethod = SelectionMethod.AllSequential,
                            DocumentFlowUsers = new List<DocumentFlowUser>
                            {
                                new DocumentFlowUser() {
                                    DocumentFlowId = 3,
                                    UserId = 2,
                                    Value = 1000
                                },
                                new DocumentFlowUser() {
                                    DocumentFlowId = 3,
                                    UserId = 7,
                                    Value = 2000
                                },
                                new DocumentFlowUser() {
                                    DocumentFlowId = 3,
                                    UserId = 3,
                                    Value = 3000
                                }
                            }
                        },
                    });
                }
                _context.SaveChanges();
            }
        }
    }
}
