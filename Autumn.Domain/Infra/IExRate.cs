using Autumn.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autumn.Domain.Infra
{
    public interface IExRate
    {
        IList<CBNEXRate> Load();
    }
}
