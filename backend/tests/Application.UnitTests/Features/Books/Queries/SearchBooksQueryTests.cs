using BookLibrary.Application.Common.Interfaces;
using BookLibrary.Application.Features.Books.Queries;
using BookLibrary.Domain.Entities;
using MockQueryable.Moq;
using Moq;
using Moq.EntityFrameworkCore;
using static System.Reflection.Metadata.BlobBuilder;

namespace BookLibrary.Application.UnitTests.Features.Books.Queries
{
    public class SearchBooksQueryTests
    {
        private readonly Mock<IApplicationDbContext> _contextMock;
        private readonly List<Book> _books;

        public SearchBooksQueryTests()
        {
            // Setup test data
            _books = new List<Book>
                {
                    new Book { BookId = 1, Title = "Pride and Prejudice", FirstName = "Jane", LastName = "Austen",
                        TotalCopies = 100, CopiesInUse = 80, Type = "Hardcover", ISBN = "1234567891", Category = "Fiction" },
                    new Book { BookId = 2, Title = "To Kill a Mockingbird", FirstName = "Harper", LastName = "Lee",
                        TotalCopies = 75, CopiesInUse = 75, Type = "Paperback", ISBN = "1234567892", Category = "Fiction" },
                    new Book { BookId = 3, Title = "The Great Gatsby", FirstName = "F. Scott", LastName = "Fitzgerald",
                        TotalCopies = 50, CopiesInUse = 22, Type = "Hardcover", ISBN = "1234567894", Category = "Non-Fiction" },
                    new Book { BookId = 4, Title = "Moby Dick", FirstName = "Herman", LastName = "Melville",
                        TotalCopies = 30, CopiesInUse = 8, Type = "Hardcover", ISBN = "8901234567", Category = "Fiction" }
            };


            var mockDbSet = _books.AsQueryable().BuildMockDbSet();
            _contextMock = new Mock<IApplicationDbContext>();
            _contextMock.Setup(c => c.Books).Returns(mockDbSet.Object);
        }

        [Fact]
        public async Task Handle_NoFilters_ReturnsAllBooks()
        {
            // Arrange
            var query = new SearchBooksQuery();
            var handler = new SearchBooksQueryHandler(_contextMock.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(4, result.Count);
        }

        [Fact]
        public async Task Handle_FilterByAuthorFirstName_ReturnsMatchingBooks()
        {
            // Arrange
            var query = new SearchBooksQuery { Author = "Jane" };
            var handler = new SearchBooksQueryHandler(_contextMock.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Single(result);
            Assert.Equal("Pride and Prejudice", result.First().Title);
        }

        [Fact]
        public async Task Handle_FilterByAuthorLastName_ReturnsMatchingBooks()
        {
            // Arrange
            var query = new SearchBooksQuery { Author = "Lee" };
            var handler = new SearchBooksQueryHandler(_contextMock.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Single(result);
            Assert.Equal("To Kill a Mockingbird", result.First().Title);
        }

        [Fact]
        public async Task Handle_FilterByISBN_ReturnsMatchingBooks()
        {
            // Arrange
            var query = new SearchBooksQuery { ISBN = "1234567891" };
            var handler = new SearchBooksQueryHandler(_contextMock.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Single(result);
            Assert.Equal("Pride and Prejudice", result.First().Title);
        }

        [Fact]
        public async Task Handle_FilterByTitle_ReturnsMatchingBooks()
        {
            // Arrange
            var query = new SearchBooksQuery { Title = "Great" };
            var handler = new SearchBooksQueryHandler(_contextMock.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Single(result);
            Assert.Equal("The Great Gatsby", result.First().Title);
        }

        [Fact]
        public async Task Handle_FilterByCategory_ReturnsMatchingBooks()
        {
            // Arrange
            var query = new SearchBooksQuery { Category = "Fiction" };
            var handler = new SearchBooksQueryHandler(_contextMock.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(4, result.Count);
            Assert.Contains(result, book => book.Title == "The Great Gatsby");
        }

        [Fact]
        public async Task Handle_OnlyAvailable_ReturnsMatchingBooks()
        {
            // Arrange
            var query = new SearchBooksQuery { OnlyAvailable = true };
            var handler = new SearchBooksQueryHandler(_contextMock.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(3, result.Count);
            Assert.DoesNotContain(result, book => book.Title == "To Kill a Mockingbird");
        }

        [Fact]
        public async Task Handle_MultipleFilters_ReturnsMatchingBooks()
        {
            // Arrange
            var query = new SearchBooksQuery
            {
                Category = "Fiction",
                OnlyAvailable = true
            };
            var handler = new SearchBooksQueryHandler(_contextMock.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Contains(result, book => book.Title == "Pride and Prejudice");
            Assert.Contains(result, book => book.Title == "Moby Dick");
            Assert.DoesNotContain(result, book => book.Title == "To Kill a Mockingbird");
        }

        [Fact]
        public async Task Handle_NoMatchingBooks_ReturnsEmptyCollection()
        {
            // Arrange
            var query = new SearchBooksQuery { Author = "NonExistentAuthor" };
            var handler = new SearchBooksQueryHandler(_contextMock.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Empty(result);
        }
    }
}
