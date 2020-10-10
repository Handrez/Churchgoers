using System.IO;

namespace Churchgoers.Common.Helpers
{
    public interface IFilesHelper
    {
        byte[] ReadFully(Stream input);
    }
}
