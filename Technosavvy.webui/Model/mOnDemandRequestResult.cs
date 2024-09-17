namespace TechnoApp.Ext.Web.UI.Model
{

    public enum mOnDemandRequestResult
    {
        Placed = 1,
        NoIssue = 2,
        DailyLimitIssue = 4,
        TotalLimitIssue = 8,
        AlreadyClaimed = 16,
        AlreadyAwaited = 32
    }
}