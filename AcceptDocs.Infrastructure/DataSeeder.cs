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

            if(_context.Database.CanConnect()) {
                if (!_context.DocumentTypes.Any()) {
                    _context.AddRange(new List<DocumentType>() {
                        new DocumentType() {
                            DocumentTypeId = 1,
                            Name = "Faktura",
                        },
                    });
                }

                if(!_context.PositionLevels.Any()) {
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
                            Login = "123456789",
                            Password = BCrypt.Net.BCrypt.HashPassword("123456789"),
                            Position = "Lorem ipsum dolor sit amet",
                            PositionLevelId = 7,
                        },
                        new User()
                        {
                            UserId = 2,
                            FirstName = "Mateusz",
                            LastName = "Kowalski",
                            Login = "987654321",
                            Password = BCrypt.Net.BCrypt.HashPassword("987654321"),
                            Position = "Lorem ipsum dolor sit amet",
                            PositionLevelId = 5,
                        },
                        new User()
                        {
                            UserId = 3,
                            FirstName = "Marian",
                            LastName = "Kowalski",
                            Login = "12345",
                            Password = BCrypt.Net.BCrypt.HashPassword("12345"),
                            Position = "Lorem ipsum dolor sit amet",
                            PositionLevelId = 5,
                        },
                        new User()
                        {
                            UserId = 4,
                            FirstName = "Piotrek",
                            LastName = "Kowalski",
                            Login = "54321",
                            Password = BCrypt.Net.BCrypt.HashPassword("54321"),
                            Position = "Lorem ipsum dolor sit amet",
                            PositionLevelId = 5,
                        },
                        new User()
                        {
                            UserId = 5,
                            FirstName = "Leokadia",
                            LastName = "Kowalska",
                            Login = "6789",
                            Password = BCrypt.Net.BCrypt.HashPassword("6789"),
                            Position = "Lorem ipsum dolor sit amet",
                            PositionLevelId = 5,
                        },
                    });
                }

                if (!_context.DocumentFlows.Any()) {
                    _context.AddRange(new List<DocumentFlow>()
                    {
                        new DocumentFlow() {
                            DocumentFlowId = 1,
                            Name = "Do wszystkich przypisanych",
                            Description = "Wysyła prośbę o sprawdzenie dokumentu wszystkim przypisanym do przepływu jednocześnie. Dokument zmienia status na zaakceptowany po zatwierdzeniu przez wszystkich użytkowników przypisanych do przepływu.",
                            SelectionMethod = SelectionMethod.All,
                            DocumentFlowUsers = new List<DocumentFlowUser>
                            {
                                new DocumentFlowUser() {
                                    DocumentFlowId = 1,
                                    UserId = 2,
                                    Value = 100
                                },
                                new DocumentFlowUser() {
                                    DocumentFlowId = 1,
                                    UserId = 3,
                                    Value = 200
                                },
                                new DocumentFlowUser() {
                                    DocumentFlowId = 1,
                                    UserId = 4,
                                    Value = 300
                                },
                                new DocumentFlowUser() {
                                    DocumentFlowId = 1,
                                    UserId = 5,
                                    Value = 400
                                },
                            }
                        },
                        new DocumentFlow() {
                            DocumentFlowId = 2,
                            Name = "Sekwencyjnie do wszystkich przypisanych",
                            Description = "Wysyła prośbę o sprawdzenie dokumentu użytkownikom przypisanym do przepływu po kolei według ich maksymalnej kwoty. Dokument zmienia status na zaakceptowany po zatwierdzeniu przez wszystkich użytkowników przypisanych do przepływu.",
                            SelectionMethod = SelectionMethod.AllSequential,
                            DocumentFlowUsers = new List<DocumentFlowUser>
                            {
                                new DocumentFlowUser() {
                                    DocumentFlowId = 2,
                                    UserId = 2,
                                    Value = 100
                                },
                                new DocumentFlowUser() {
                                    DocumentFlowId = 2,
                                    UserId = 3,
                                    Value = 200
                                },
                                new DocumentFlowUser() {
                                    DocumentFlowId = 2,
                                    UserId = 4,
                                    Value = 300
                                },
                                new DocumentFlowUser() {
                                    DocumentFlowId = 2,
                                    UserId = 5,
                                    Value = 400
                                }
                            }
                        },
                        new DocumentFlow() {
                            DocumentFlowId = 3,
                            Name = "Sekwencyjnie do kwalifikującego się",
                            Description = "Wysyła prośbę o sprawdzenie dokumentu użytkownikom przypisanym do przepływu po kolei według ich maksymalnej kwoty aż do użytkownika którego kwota przekracza kwotę dokumentu. Dokument zmienia status na zaakceptowany po zatwierdzeniu przez wszystkich użytkowników przypisanych do przepływu.",
                            SelectionMethod = SelectionMethod.AllSequential,
                            DocumentFlowUsers = new List<DocumentFlowUser>
                            {
                                new DocumentFlowUser() {
                                    DocumentFlowId = 3,
                                    UserId = 2,
                                    Value = 100
                                },
                                new DocumentFlowUser() {
                                    DocumentFlowId = 3,
                                    UserId = 3,
                                    Value = 200
                                },
                                new DocumentFlowUser() {
                                    DocumentFlowId = 3,
                                    UserId = 4,
                                    Value = 300
                                },
                                new DocumentFlowUser() {
                                    DocumentFlowId = 3,
                                    UserId = 5,
                                    Value = 400
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
