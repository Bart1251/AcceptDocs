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
                            Login = "123",
                            Password = "123",
                            Position = "Lorem ipsum dolor sit amet",
                            PositionLevelId = 7,
                        }
                    });
                }

                if (!_context.DocumentFlows.Any()) {
                    _context.AddRange(new List<DocumentFlow>()
                    {
                        new DocumentFlow() {
                            DocumentFlowId = 1,
                            Name = "Test",
                            Description = "Test",
                            SelectionMethod = SelectionMethod.All,
                        }
                    });
                }
                _context.SaveChanges();
            }
        }
    }
}
