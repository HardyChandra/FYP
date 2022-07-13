using System;

namespace FYP.DatabaseModel
{
    public class Voting
    {
        public string voting_id { get; set; }
        public string admin_id { get; set; }
        public string voting_title { get; set; }
        public string voting_description { get; set; }
        public int voting_total_voter { get; set; }
        public int voting_duration { get; set; }
        public Status voting_status { get; set; }

        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }

    }
}
