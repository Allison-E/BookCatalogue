using BookCatalogue.API.Dtos.Author;

namespace BookCatalogue.API.Mappings;

public class AuthorProfile: Profile
{
    public AuthorProfile()
    {
        CreateMap<CreateAuthorDto, Author>();
        CreateMap<UpdateAuthorDto, Author>();
        CreateMap<Author, ReadAuthorDto>();
    }
}
