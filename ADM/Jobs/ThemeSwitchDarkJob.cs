using System.Threading.Tasks;
using ADM.Helpers;
using Quartz;

namespace ADM.Jobs
{
    public class ThemeSwitchDarkJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            ThemeSwitchHelper.Switch(ThemeSwitchHelper.Mode.Dark);
        }
    }
}