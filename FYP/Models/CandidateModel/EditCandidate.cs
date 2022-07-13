using System.ComponentModel.DataAnnotations;

namespace FYP.Models.CandidateModel
{
    public class EditCandidate
    {
		public string candidate_id { get; set; }
		[Display(Name = "Name")]
		public string candidate_name { get; set; }
		[Display(Name = "Vision")]
		public string candidate_vision { get; set; }
		[Display(Name = "Mission")]
		public string candidate_mission { get; set; }
	}
}
