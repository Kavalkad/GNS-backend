using Cronos;
using GNS.Interfaces;
using GNS.Services;
using GNS.Services.Interfaces;

namespace GNS.BackgroundServices
{
    public class MontlyResetBonusesService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly CronExpression _cronExpression;

        public MontlyResetBonusesService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;

            _cronExpression = CronExpression.Parse("0 0 1 * *");
        }

        protected override async Task ExecuteAsync(CancellationToken token)
        {
            await ScheduleNextExcequtionAsync(token);
        }
        private async Task ScheduleNextExcequtionAsync(CancellationToken token)
        {
           
            try
            {
                var now = DateTime.UtcNow;
                var nextExecution = _cronExpression.GetNextOccurrence(now);

                if (nextExecution.HasValue && !token.IsCancellationRequested)
                {
                    
                    var delay = nextExecution.Value - now;

                    await Task.Delay(delay, token);

                    if (token.IsCancellationRequested)
                    {
                        return;
                    }

                    await MonthlySetZeroBonuses();

                    _ = ScheduleNextExcequtionAsync(token);
                    
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Ошибка в планировщике " + e.Message);
                // Перепланируем через 1 минуту при ошибке
                await Task.Delay(TimeSpan.FromMinutes(1), token);
                _ = ScheduleNextExcequtionAsync(token);
            }

        }
        private async Task MonthlySetZeroBonuses()
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var employeeService = scope.ServiceProvider.GetRequiredService<IEmployeeService>();

                await employeeService.SetZeroBonuses();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public override async Task StopAsync(CancellationToken token)
        {
            Console.WriteLine("Остановка CronMonthlyBonusResetService");
            await base.StopAsync(token);
        }
    }
}