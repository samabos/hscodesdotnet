

using Autumn_UIML.Model;

namespace Autumn.Service.Interface
{
    public interface IPredictService
    {
        ModelOutput GetHSCode(string product);
    }
}
