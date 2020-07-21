using System;
using System.Collections.Specialized;
using System.Threading.Tasks;
using ADM.Helpers;
using ADM.Jobs;
using ADM.Properties;
using ADM.Views;
using Quartz;
using Quartz.Impl;

namespace ADM
{
    public class ThemeSwitchService
    {
        private IScheduler _scheduler;

        public ThemeSwitchService()
        {
            Start().GetAwaiter().GetResult();
        }

        private async Task Start()
        {
            try
            {
                ThemeSwitchHelper.Switch(ThemeSwitchHelper.Now());

                var props = new NameValueCollection
                {
                    { "quartz.serializer.type", "binary" }
                };
                var factory = new StdSchedulerFactory(props);
                _scheduler = await factory.GetScheduler();
                await _scheduler.Start();

                var startTime = Settings.Default.StartTime;
                
                var darkJob = JobBuilder.Create<ThemeSwitchDarkJob>()
                    .WithIdentity("Dark", "ThemeSwitcher")
                    .Build();
                var darkTrigger = TriggerBuilder.Create()
                    .WithIdentity("startTime", "ThemeSwitcher")
                    .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(startTime.Hour, startTime.Minute).WithMisfireHandlingInstructionFireAndProceed())
                    .Build();
                
                var endTime = Settings.Default.EndTime;
                
                var lightJob = JobBuilder.Create<ThemeSwitchLightJob>()
                    .WithIdentity("Light", "ThemeSwitcher")
                    .Build();
                var lightTrigger = TriggerBuilder.Create()
                    .WithIdentity("endTime", "ThemeSwitcher")
                    .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(endTime.Hour, endTime.Minute).WithMisfireHandlingInstructionFireAndProceed())
                    .Build();

                await _scheduler.ScheduleJob(darkJob, darkTrigger);
                await _scheduler.ScheduleJob(lightJob, lightTrigger);
            }
            catch (Exception e)
            {
                new ExceptionWindow("Could not initialize theme service: " + e.Message);
            }
        }

        public async Task Restart()
        {
            await Shutdown();
            Start().GetAwaiter().GetResult();
        }
        
        private async Task Shutdown()
        {
            try
            {
                await _scheduler.Shutdown();
            }
            catch (SchedulerException se)
            {
                Console.WriteLine(se);
            }
        }
    }
}