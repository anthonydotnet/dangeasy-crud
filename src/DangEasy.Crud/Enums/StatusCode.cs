namespace DangEasy.Crud.Enums
{
    // Use HTTP Status codes so mapping will be more intuitive in later implementations
    public enum StatusCode
    {
        Ok = 200, // updates and gets
        Created = 201,
        NoContent = 204, // delete responses
        NotFound = 404, // get by id 
        Conflict = 409, // create with same id conflicts
        Error = 500, // generic internal error
    }
}
