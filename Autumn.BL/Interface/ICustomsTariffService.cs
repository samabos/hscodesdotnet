﻿using Autumn.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Autumn.Service.Interface
{
    public interface ICustomsTariffService : IBaseService<CustomsTariff>
    {
        Task<List<CustomsTariff>> GetByHeaderAsync(string header);
        Task<CustomsTariff> GetByHSCodeAsync(string hscode);
    }
}
