namespace Tz.Utils.Interfaces
{
    public interface IPhonebookView
    {
        void RefreshGrid();
        string FirstName { get; set; }
        string Surname { get; set; }
        string Patronymic { get; set; }
    }
}
