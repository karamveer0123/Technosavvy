using System.ComponentModel.DataAnnotations;

namespace TechnoApp.Ext.Web.UI.Model

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
    public class mJD
    {
        public Guid id { get; set; }
        public string Title { get; set; }
        public string Department { get; set; }
        public string Body { get; set; }
        public string RefNo { get; set; }
    }

}
