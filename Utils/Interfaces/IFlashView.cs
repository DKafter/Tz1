using Tz.Models;

namespace Tz.Utils.Interfaces
{
    public interface IFlashView
    {
        void RefreshGrid();
        void AddFlashToDb(Flash dp);
        string SerialNumber { get; set; }
        string NameCompany { get; set; }
        string DateCreate { get; }
    }
}
