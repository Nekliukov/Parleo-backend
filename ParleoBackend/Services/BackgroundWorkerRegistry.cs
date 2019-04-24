using FluentScheduler;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ParleoBackend.Services
{
    public class BackgroundWorkerRegistry: Registry
    {
        public BackgroundWorkerRegistry(IServiceProvider serviceProvider)
        {
            Schedule(() => 
                serviceProvider
                .CreateScope()
                .ServiceProvider
                .GetRequiredService<ClearExpiredTokenJob>()
            )
                .ToRunNow()
                .AndEvery(5)
                .Minutes();
        }
    }
}
