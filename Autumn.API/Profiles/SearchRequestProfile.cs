using System;
using AutoMapper;
using Autumn.API.Contract.V2.Requests;
using Autumn.BL.Models.Request.V2;

namespace Autumn.API.Profiles
{
    public class SearchRequestProfile : Profile
    {
        public SearchRequestProfile()
        {
            CreateMap<SearchRequest,BLSearchRequest>();
        }
    }
}
