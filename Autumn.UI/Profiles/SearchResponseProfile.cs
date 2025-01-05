using System;
using System.Collections.Generic;
using AutoMapper;
using Autumn.BL.Models.Response.V3;
using Autumn.UI.ViewModels;

namespace Autumn.UI.Profiles
{
    public class SearchResponseProfile:Profile
    {
        public SearchResponseProfile()
        {
            CreateMap<BLSearchResponse, SearchResponse>();
            CreateMap<BLResultModel, ResultModel>();
        }
    }
}
