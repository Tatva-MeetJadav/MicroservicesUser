namespace MicroservicesUser.BusinessLogic.Interfaces
{
    public interface IEncryptDecryptServices
    {
        string EncryptPassword(string password);
        bool VerifyPassword(string password, string hashedPassword);
        string EncryptId(int id);
        int DecryptId(string encrypted);
    }
}
