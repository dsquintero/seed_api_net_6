using Api.DTOs;
using Api.Models;
using AutoMapper;

namespace Api.AutoMapperProfiles
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            #region Auth
            CreateMap<User, LoginResponse>()
                .ForMember(d => d.UserName, o => o.MapFrom(s => string.Concat(s.FirsName, " ", s.LastName).Trim()));
            #endregion            
        }
    }
}
