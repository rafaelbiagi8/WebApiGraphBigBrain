using AutoMapper;
using WebApiGraphBigBrain.models;

namespace WebApiGraphBigBrain.Services
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<User, Microsoft.Graph.User>().ReverseMap();
        }

    }
}
