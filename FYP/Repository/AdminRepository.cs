using Dapper;
using FYP.DatabaseModel;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;

namespace FYP.Repository
{
    public class AdminRepository : IAdminRepository
    {
        private IDbConnection _db;

        public AdminRepository(IConfiguration configuration)
        {
            _db = new SqlConnection(configuration.GetConnectionString("SqlConnection"));
        }

        public int RegisterAdmin(string AdminID, string Name, string Email, string Company, string Password, string PhoneNumber, DateTime DoB)
        {
            var query = "INSERT INTO Admin (admin_id, admin_name, admin_email, admin_company, admin_password, admin_phone, admin_dob, admin_role) VALUES (@AdminID, @Name, @Email, @Company, @Password, @PhoneNumber, @DoB, @Role)";

            return _db.Execute(query, new {AdminID, Name, Email, Company, Password, PhoneNumber, DoB, Role = "Admin" });
        }

        public Admin GetAdmin(string Email)
        {
            var query = "SELECT * FROM Admin WHERE admin_email = @Email";

            return _db.QueryFirstOrDefault<Admin>(query, new { Email });
        }

        public Admin GetAdminID(string AdminID)
        {
            var query = "SELECT * FROM Admin WHERE admin_id = @AdminID";

            return _db.QueryFirstOrDefault<Admin>(query, new { AdminID });
        }

        public Admin CheckEmail(string Email)
		{
            var query = "SELECT * FROM Admin WHERE admin_email = @Email";

            return _db.QueryFirstOrDefault<Admin>(query, new { Email });
        }

        public Admin CheckPhone(string PhoneNumber)
        {
            var query = "SELECT * FROM Admin WHERE admin_phone = @PhoneNumber";

            return _db.QueryFirstOrDefault<Admin>(query, new { PhoneNumber });
        }

        public int EditAdmin(string AdminID, string Name, string Email, string Company, string PhoneNumber, DateTime DoB)
        {
            var query = "UPDATE Admin SET admin_name = @Name, admin_email = @Email, admin_company = @Company, admin_phone = @PhoneNumber, admin_dob = @DoB WHERE admin_id = @AdminID";

            return _db.Execute(query, new {AdminID, Name, Email, Company, PhoneNumber, DoB });
        }

        public Admin CheckPassword(string AdminID)
        {
            var query = "SELECT admin_password FROM Admin WHERE admin_id = @AdminID";

            return _db.QueryFirstOrDefault<Admin>(query, new { AdminID });
        }

        public int ChangePass(string AdminID, string Password)
        {
            var query = "Update Admin SET admin_password = @Password WHERE admin_id = @AdminID";

            return _db.Execute(query, new {AdminID, Password});
        }
    }
}
