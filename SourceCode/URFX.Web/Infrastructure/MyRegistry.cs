using FluentScheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using URFX.Web.Controllers;
using URFX.Web.Scheduler;

namespace URFX.Web.Infrastructure
{
   
    public class MyRegistry : Registry
    {
        public MyRegistry()
        {
            // Schedule an ITask to run at an interval
            Schedule<JobTask>().AndThen<ClientNotifyTask>().ToRunNow().AndEvery(1).Minutes();

            
        }
    }
}