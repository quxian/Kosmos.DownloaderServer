using Kosmos.DownloaderServer.DbContext;
using Kosmos.DownloaderServer.Model;
using Kosmos.Singleton;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Kosmos.DownloaderServer
{
    public static class ResultCahce
    {
        public static ConcurrentDictionary<string, DownloadedResult> Results;
        private static Task _task;

        private static object _lock = new object();

        static ResultCahce()
        {
            Results = new ConcurrentDictionary<string, DownloadedResult>();

            _task = Task.Run(async () =>
            {
                while (true)
                {
                    try
                    {
                        await Task.Delay(TimeSpan.FromMinutes(2));
                        using (var dbContext = new AppDbContext())
                        {
                            CacheToDb(dbContext);
                        }
                    }
                    catch (Exception e)
                    {
                        SingleHttpClient.PostException(e);
                    }
                }
            });
        }

        public static void CacheToDb(AppDbContext dbContext)
        {
            try
            {
                lock (_lock)
                {
                    var results = Results.Distinct();

                    var dbHashCode = dbContext.DownloadedResults.Select(x => x.ResultHashCode);
                    var resultsHashCode = results.Select(x => x.Value.ResultHashCode);

                    var exceptHashCode = resultsHashCode.Except(dbHashCode).ToList();
                    if (exceptHashCode.Count > 0)
                    {
                        var except = results
                            .AsParallel()
                            .Where(x => exceptHashCode.Any(h => h == x.Value.ResultHashCode))
                            .Select(x => x.Value)
                            .ToList();
                        dbContext.DownloadedResults.AddRange(except);
                        dbContext.SaveChanges();
                    }
                    Results = new ConcurrentDictionary<string, DownloadedResult>();
                }
            }
            catch (Exception e)
            {
                SingleHttpClient.PostException(e);
            }
        }
    }
}