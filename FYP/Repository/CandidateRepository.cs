using Dapper;
using FYP.DatabaseModel;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FYP.Repository
{
    public class CandidateRepository : ICandidateRepository
    {
        private IDbConnection _db;

        public CandidateRepository(IConfiguration configuration)
        {
            _db = new SqlConnection(configuration.GetConnectionString("SqlConnection"));
        }
        public List<Candidate> GetCandidate(string VotingID)
        {
            var query = "SELECT * FROM Candidate WHERE voting_id = @VotingID";

            return _db.Query<Candidate>(query, new { VotingID }).ToList();
        }

        public int AddCandidate(string CandidateID, string VotingID, string CandidateName, string CandidateVision, string CandidateMission)
        {
            var query = "INSERT INTO Candidate (candidate_id, voting_id, candidate_name, candidate_vision, candidate_mission) VALUES (@CandidateID, @VotingID, @CandidateName, @CandidateVision, @CandidateMission)";

            return _db.Execute(query, new { CandidateID, VotingID, CandidateName, CandidateVision, CandidateMission });
        }

        public Candidate GetCandidateID(string CandidateID)
        {
            var query = "SELECT * FROM Candidate WHERE candidate_id = @CandidateID";

            return _db.QueryFirstOrDefault<Candidate>(query, new { CandidateID });
        }

        public int EditCandidate(string CandidateID, string CandidateName, string CandidateVision, string CandidateMission)
        {
            var query = "UPDATE Candidate SET candidate_name = @CandidateName, candidate_vision = @CandidateVision, candidate_mission = @CandidateMission WHERE candidate_id = @CandidateID";

            return _db.Execute(query, new { CandidateID, CandidateName, CandidateVision, CandidateMission });
        }

        public int RemoveCandidate(string CandidateID)
        {
            var query = "DELETE FROM Candidate WHERE candidate_id = @CandidateID";

            return _db.Execute(query, new { CandidateID });
        }

        public List<Candidate> GetCandidateForVoting(string VotingID)
        {
            var query = "SELECT Candidate.*, Voting.* FROM Candidate INNER JOIN Voting ON Candidate.voting_id = Voting.voting_id WHERE Candidate.voting_id = @VotingID AND Voting.voting_id = @VotingID";

            return _db.Query<Candidate>(query, new { VotingID }).ToList();
        }
    }
}
