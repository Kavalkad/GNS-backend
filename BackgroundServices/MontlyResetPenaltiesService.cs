using Cronos;
using GNS.Services.Interfaces;

namespace GNS.BackgroundServices
{
    public class MontlyResetPenaltiesService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly CronExpression _cronExpression;

        public MontlyResetPenaltiesService(IServiceScopeFactory scopeFactory)
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
            while (!token.IsCancellationRequested)
            {
                var now = DateTime.UtcNow;
                var nextExecution = _cronExpression.GetNextOccurrence(now);

                if (nextExecution is null)
                {
                    return;
                }
                if (token.IsCancellationRequested)
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
                    // нужно логгирование
                }
                try
                {
                    await SetZeroPenalties(token);

                }
                catch(Exception e)
                {
                    //Нужно логгирование
                }

                await ScheduleNextExcequtionAsync(token);


            }


        }
        private async Task SetZeroPenalties(CancellationToken token = default)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var employeeService = scope.ServiceProvider.GetRequiredService<IEmployeeService>();

                await employeeService.SetZeroPenaltiesAsync(token);
            }
            catch
            {

            }
        }
        public override async Task StopAsync(CancellationToken token)
        {
            await base.StopAsync(token);
        }
    }
}