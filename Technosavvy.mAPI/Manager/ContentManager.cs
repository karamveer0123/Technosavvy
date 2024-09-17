namespace NavExM.Int.Maintenance.APIs.Manager
{


    internal class ContentManager : ManagerBase
    {
        internal List<mFAQDisplay> GetAllApprovedFAQs()
        {
            var ret = new List<mFAQDisplay>();
            try
            {
                ret = cdbctx.FAQs.Where(x => x.Status == Data.Entity.Contents.eAuthStatus.Approved).ToList().ToModel();
                return ret;

            }
            catch (Exception ex)
            {
                LogError(ex);
                return ret;
            }
        }
        internal bool SaveFAQ(mFAQDisplay vm)
        {
            try
            {
                var faq = cdbctx.FAQs.Add(new Data.Entity.Contents.FAQ
                {
                    AnswerText = vm.AnswerText,
                    QuestionText = vm.QuestionText,
                    GroupTitle = vm.GroupTitle,
                    OrderNo = vm.OrderNo,
                    Status = Data.Entity.Contents.eAuthStatus.Approved
                });

                return cdbctx.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            return false;
        }
    }

}
