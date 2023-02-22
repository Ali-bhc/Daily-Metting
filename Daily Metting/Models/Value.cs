namespace Daily_Metting.Models
{
    public class Value
    {
        public int ValueID { get; set; }
        public string Value_point { get; set; }
        public string description { get; set; }
        public string comment{ get; set; }
        public Point Point  { get; set; }
        public Submission Submission{ get; set; }

        public Value(int valueID, string value_point, string description, string comment, Point point, Submission submission)
        {
            ValueID = valueID;
            Value_point = value_point;
            this.description = description;
            this.comment = comment;
            Point = point;
            Submission = submission;
        }
    }
}
