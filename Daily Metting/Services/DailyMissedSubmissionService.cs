using Daily_Metting.Repositories;
using Daily_Metting.Repositories.Submissions;
using Daily_Metting.Repositories.Users;
using Daily_Metting.Repositories;
using Daily_Metting.Data;

namespace Daily_Metting.Services
{
    public class DailyMissedSubmissionService : IHostedService, IDisposable
    {
        private readonly Timer _timer;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        


        public DailyMissedSubmissionService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _timer = new Timer(DoWork, null, GetInitialDelay(), TimeSpan.FromDays(1));
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer.Dispose();
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer.Dispose();
        }

        private void DoWork(object state)
        {
            DateTime start = new DateTime(2023, 4, 20);
            DateTime end = new DateTime(2023, 5, 2);
            var dates = new List<DateTime>();

            for (var dt = start; dt <= end; dt = dt.AddDays(1))
            {
                dates.Add(dt);
            }

            if (!dates.Contains(DateTime.Today.Date))
            {
                if (DateTime.Today.DayOfWeek != DayOfWeek.Saturday && DateTime.Today.DayOfWeek != DayOfWeek.Sunday)
                {
                    using var scope = _serviceScopeFactory.CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<DailyMeetingDbContext>();
                    ISubmissionRepository _submissionRepository = new SubmissionRepository(dbContext);
                    IUserRepository _userRepository = new UserRepository(dbContext);
                    var submissions = _submissionRepository.GetTodaySubmissions();
                    var userSubmissions = submissions.GroupBy(s => s.User.Id).ToDictionary(g => g.Key, g => g.ToList());
                    foreach (var user in _userRepository.GetMembers())
                    {
                        if (!userSubmissions.ContainsKey(user.Id))
                        {
                            user.MissedSubmissions = user.MissedSubmissions + 1;
                        }
                    }
                    // Save changes to the database
                    dbContext.SaveChanges();
                }
            }
        }

        private TimeSpan GetInitialDelay()
        {
            var now = DateTime.Now;
            
            DateTime targetTime = new DateTime(now.Year, now.Month, now.Day, 23, 59, 00);
            DateTime nextRunTime;

            if (now<=targetTime)
            {
                nextRunTime = targetTime;
            }
            else
            {
                nextRunTime = new DateTime(now.Year, now.Month, now.Day, 23, 59, 0, DateTimeKind.Utc).AddDays(1);
            }

            //var nextRunTime = new DateTime(now.Year, now.Month, now.Day, 14, 01, 0, DateTimeKind.Utc);
            // var nextRunTime = new DateTime(now1.Year, now1.Month, now1.Day, 0, 0, 0, DateTimeKind.Utc).AddDays(1);
            //var nextRunTime = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, DateTimeKind.Utc).AddDays(1);
            var test = nextRunTime - now;
            return test;
        }
    }

}
