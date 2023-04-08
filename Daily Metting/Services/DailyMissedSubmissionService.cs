using Daily_Metting.DAO;
using Daily_Metting.Repositories;

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

        //public DailyMissedSubmissionService(IServiceScopeFactory serviceScopeFactory, ISubmissionRepository submissionRepository, IUserRepository userRepository)
        //{
        //    _timer = timer;
        //    _serviceScopeFactory = serviceScopeFactory;
        //    _submissionRepository = submissionRepository;
        //    _userRepository = userRepository;
        //    _timer = new Timer(DoWork, null, GetInitialDelay(), TimeSpan.FromDays(1));

        //}


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
            if (DateTime.Today.DayOfWeek != DayOfWeek.Saturday && DateTime.Today.DayOfWeek != DayOfWeek.Sunday)
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<DailyMeetingDbContext>();
                ISubmissionRepository _submissionRepository = new SubmissionRepository(dbContext);
                IUserRepository _userRepository = new UserRepository(dbContext);

                //var yesterday = DateTime.Today.AddDays(-1);
                var submissions = _submissionRepository.GetYesterdaySubmissions();
                var userSubmissions = submissions.GroupBy(s => s.User.Id).ToDictionary(g => g.Key, g => g.ToList());
                //_userRepository.updateUserMissedsubmission(userSubmissions);
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

        private TimeSpan GetInitialDelay()
        {
            var now1 = DateTime.Now;
            //var now1 = new DateTime(2023, 03, 13, 23, 59, 20);

            var nextRunTime = new DateTime(now1.Year, now1.Month, now1.Day, 0, 0, 0, DateTimeKind.Utc).AddDays(1);
           // var nextRunTime = new DateTime(now1.Year, now1.Month, now1.Day, 0, 0, 0, DateTimeKind.Utc).AddDays(1);
            //var nextRunTime = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, DateTimeKind.Utc).AddDays(1);
            var test = nextRunTime - now1;
            return test;
        }
    }

}
