using FYP.DatabaseModel;
using System.Collections.Generic;

namespace FYP.Repository
{
    public interface ICandidateRepository
    {
        public List<Candidate> GetCandidate(string VotingID);
        public int AddCandidate(string CandidateID, string VotingID, string CandidateName, string CandidateVision, string CandidateMission);
        public Candidate GetCandidateID(string CandidateID);
        public int EditCandidate(string CandidateID, string CandidateName, string CandidateVision, string CandidateMission);
        public int RemoveCandidate(string CandidateID);
        public List<Candidate> GetCandidateForVoting(string VotingID);
    }
}
