using FYP.DatabaseModel;
using System;
using System.Collections.Generic;

namespace FYP.Repository
{
	public interface IVotingRepository
	{
		public int CreateVoting(string VotingID, string AdminID, string Title, string Description, int TotalVoter, int Duration, Status Status);
		public int CreateVoterCredential(string VoterCredentialID, string VotingID);
		public Voting GetVoting(string AdminID);
		public Voting GetVotingByID(string VotingID);
		public List<VoterCredential> GetCredential(string VotingID);
		public int StartVote(string VotingID, DateTime StartDate, DateTime EndTime, Status Status);
		public int EndVote(string VotingID, Status Status);
	}
}
