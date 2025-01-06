using System;
using AutoMapper;
using Autumn.BL.Models.Request.V3;
using Autumn.UI.ViewModels;

namespace Autumn.UI.Profiles
{
    public class SearchRequestProfile : Profile
    {
        public SearchRequestProfile()
        {
            CreateMap<InputModel, BLSearchRequest>();
        }
    }
}
