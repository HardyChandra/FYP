using FYP.DatabaseModel;
using System;

namespace FYP.Repository
{
    public interface IAdminRepository
    {
        public int RegisterAdmin(string AdminID, string Name, string Email, string Company, string Password, string PhoneNumber, DateTime DoB);
        Admin GetAdmin(string Email);
        Admin GetAdminID(string AdminID);
        Admin CheckEmail(string Email);
        Admin CheckPhone(string PhoneNumber);
        public int EditAdmin(string AdminID, string Name, string Email, string Company, string PhoneNumber, DateTime DoB);
        Admin CheckPassword(string AdminID);
        public int ChangePass(string AdminID, string Password);
    }
}
