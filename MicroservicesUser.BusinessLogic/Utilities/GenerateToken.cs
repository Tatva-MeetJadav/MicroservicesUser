namespace Microservices.BusinessLogic.Utilities
{
    public static class GenerateToken
    {
        public static string GenerateGuid()
        {
            Guid guid = Guid.NewGuid();
            return guid.ToString();
        }
    }
}