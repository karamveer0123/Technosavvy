namespace NavExM.Int.Maintenance.APIs.Model
{
    public class mFAQDisplay
    {
        public int id { get; set; }
        [StringLength(2500)]
        public string QuestionText { get; set; }
        [StringLength(2500)]
        public string AnswerText { get; set; }
        [StringLength(2500)]
        public string GroupTitle { get; set; }
        public int OrderNo { get; set; }
    }

}
