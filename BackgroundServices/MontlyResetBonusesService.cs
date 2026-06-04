using Cronos;
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

            _cronExpression = CronExpression.Parse("1 * * * *");
        }

        protected override async Task ExecuteAsync(CancellationToken token)
        {
            await ScheduleNextExcequtionAsync(token);
        }
        private async Task ScheduleNextExcequtionAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                var now = DateTime.UtcNow;
                var nextExecution = _cronExpression.GetNextOccurrence(now);

                if (nextExecution is null)
                {
                    break;
                }

                var delay = nextExecution.Value - now;
                
                try
                {
                    await Task.Delay(delay, token);

                }
                catch (OperationCanceledException) when (token.IsCancellationRequested)
                {
                    // Нужно логгирование
                    return;
                }

                try
                {
                    await SetZeroBonuses(token);
                }
                catch (Exception e)
                {
                    // Нужно логгирование

                }
            }
        }


        private async Task SetZeroBonuses(CancellationToken token = default)
        {

            using var scope = _scopeFactory.CreateScope();
            var employeeService = scope.ServiceProvider.GetRequiredService<IEmployeeService>();

            await employeeService.SetZeroBonusesAsync(token);

        }
        public override async Task StopAsync(CancellationToken token)
        {
            await base.StopAsync(token);
        }
    }
}