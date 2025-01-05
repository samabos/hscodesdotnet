using System;
using System.Collections.Generic;

namespace Autumn.Domain.Infra
{
    public interface ITokenizer
    {
        IList<string> GetTokens(string text);
    }
}
