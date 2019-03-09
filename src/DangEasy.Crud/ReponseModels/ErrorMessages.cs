using System;
namespace DangEasy.Crud.Results
{
    public static class ErrorMessage
    {
        // public const string Error;

        public static string Build(string methodName, params object[] parameters)
        {
            var res = $"Error during {methodName}. Parameters: {string.Join(", ", parameters)}";

            return res;
        }
    }
}
