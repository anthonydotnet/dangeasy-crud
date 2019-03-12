using DangEasy.Crud.Interfaces;

namespace DangEasy.Crud.Events
{
    public class EventResult
    {
        public ICrudResponse CrudResponse { get; set; }
        public bool ExitMethod { get; set; }
    }
}
