using BookCatalogue.API.Dtos.Author;
using BookCatalogue.API.Dtos.Book;

namespace BookCatalogue.API.Mappings;

public class BookProfile: Profile
{
    public BookProfile()
    {
        CreateMap<CreateBookDto, Book>();
        CreateMap<UpdateBookDto, Book>();
        CreateMap<Book, ReadBookDto>();
        CreateMap<Book, ReadAuthorBooksDto>();
    }
}
