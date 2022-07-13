using Dapper;
using FYP.DatabaseModel;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FYP.Repository
{
	public class VotingRepository : IVotingRepository
	{
        private IDbConnection _db;

        public VotingRepository(IConfiguration configuration)
        {
            _db = new SqlConnection(configuration.GetConnectionString("SqlConnection"));
        }

        public int CreateVoting(string VotingID, string AdminID, string Title, string Description, int TotalVoter, int Duration, Status Status)
		{
            var query = "INSERT INTO Voting (voting_id, admin_id, voting_title, voting_description, voting_total_voter, voting_duration, voting_status) VALUES (@VotingID, @AdminID, @Title, @Description, @TotalVoter, @Duration, @Status)";

            return _db.Execute(query, new { VotingID, AdminID, Title, Description, TotalVoter, Duration, Status});
        }

        public int CreateVoterCredential(string VoterCredentialID, string VotingID)
        {
            var query = "INSERT INTO VoterCredential (voter_credential_id, voting_id) VALUES (@VoterCredentialID, @VotingID)";

            return _db.Execute(query, new { VoterCredentialID, VotingID });
        }

        public Voting GetVoting(string AdminID)
        {
            var query = "SELECT * FROM Voting WHERE admin_id = @AdminID";

            return _db.QueryFirstOrDefault<Voting>(query, new { AdminID });
        }
        public Voting GetVotingByID(string VotingID)
        {
            var query = "SELECT * FROM Voting WHERE voting_id = @VotingID";

            return _db.QueryFirstOrDefault<Voting>(query, new { VotingID });
        }
        public List<VoterCredential> GetCredential(string VotingID)
        {
            var query = "SELECT * FROM VoterCredential WHERE voting_id = @VotingID";

            return _db.Query<VoterCredential>(query, new { VotingID }).ToList<VoterCredential>();
        }

        public int StartVote(string VotingID, DateTime StartDate, DateTime EndDate, Status Status)
        {
            var query = "UPDATE Voting SET start_date = @StartDate, end_date = @EndDate, voting_status = @Status WHERE voting_id = @VotingID";

            return _db.Execute(query, new {VotingID, StartDate, EndDate, Status });
        }

        public int EndVote(string VotingID, Status Status)
        {
            var query = "UPDATE Voting SET voting_status = @Status WHERE voting_id = @VotingID";

            return _db.Execute(query, new { VotingID, Status });
        }
    }
}
