using BookCatalogue.API.Dtos.Subject;

namespace SubjectCatalogue.API.Mappings;

public class SubjectProfile: Profile
{
    public SubjectProfile()
    {
        CreateMap<CreateSubjectDto, Subject>();
        CreateMap<UpdateSubjectDto, Subject>();
        CreateMap<Subject, ReadSubjectDto>();
        CreateMap<Subject, ReadSubjectPreviewDto>();
    }
}
