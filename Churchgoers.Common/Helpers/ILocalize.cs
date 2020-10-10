using System.Globalization;

namespace Churchgoers.Common.Helpers
{
    public interface ILocalize
    {
        CultureInfo GetCurrentCultureInfo();

        void SetLocale(CultureInfo ci);
    }
}