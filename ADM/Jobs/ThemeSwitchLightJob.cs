using System.Threading.Tasks;
using ADM.Helpers;
using Quartz;

namespace ADM.Jobs
{
    public class ThemeSwitchLightJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            ThemeSwitchHelper.Switch(ThemeSwitchHelper.Mode.Light);
            return Task.CompletedTask;
        }
    }
}