namespace NavExM.Int.Maintenance.APIs.Manager
{
    internal class PageEventManager : ManagerBase
    {
        internal void AddEvents(List<mPageEventRecord> lst)
        {
            lst.ForEach(x =>
            {
                x.Country = x.Country ?? "Hidden";
                edbctx.PageEvent.Add(new PageEventRecord { 
                 At=x.At, City=x.City, Country=x.Country, Event=x.Event, IP=x.IP, LTUID=x.LTUID, Page=x.Page,PageInstanceId=x.PageInstanceId,
                    Scroll = x.Scroll,ScreenHeight=x.ScreenHeight
                });
            });
            edbctx.SaveChanges();
        }
        internal List<IGrouping<string?,PageEventRecord>> GetVisitorsPerCountry(int durationInminutes = 1440)
        {
        if (durationInminutes <= 0) durationInminutes = 1440;
            var dt=DateTime.UtcNow.AddMinutes(-durationInminutes);
            var lst = edbctx.PageEvent.Where(x => x.At > dt)
               // .Select(x => new { x.Country, x.LTUID })
                .GroupBy(x => x.Country).ToList();
            return lst;
        }

    }

}
