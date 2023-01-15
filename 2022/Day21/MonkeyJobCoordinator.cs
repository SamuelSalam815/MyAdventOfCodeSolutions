using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Day21;

internal partial class MonkeyJobCoordinator
{
    private Dictionary<string, List<MonkeyMathJob>> jobDependencies;
    private HashSet<string> monkiesShouted;

    public long? RootMonkeyNumber { get; private set; }

    public MonkeyJobCoordinator()
    {
        jobDependencies = new();
        monkiesShouted = new();
    }

    public void TriggerMonkeyShout(string monkeyName, long numberShouted)
    {
        //Console.WriteLine($"{monkeyName} shouts {numberShouted}");
        monkiesShouted.Add(monkeyName);

        if(monkeyName.Equals("root"))
        {
            RootMonkeyNumber = numberShouted;
        }

        if(jobDependencies.TryGetValue(monkeyName, out List<MonkeyMathJob>? dependentJobs))
        {
            foreach(MonkeyMathJob dependentJob in dependentJobs)
            {
                dependentJob.SetArgument(monkeyName, numberShouted);
            }
        }
    }

    public void RegisterMonkeyMathJob(MonkeyMathJob job)
    {
        AddJobDependency(job, job.ArgumentName1);
        AddJobDependency(job, job.ArgumentName2);
    }

    private void AddJobDependency(MonkeyMathJob job, string dependency)
    {
        if(jobDependencies.TryGetValue(dependency, out List<MonkeyMathJob>? dependentJobs))
        {
            dependentJobs.Add(job);
        }
        else
        {
            jobDependencies.Add(dependency, new());
            jobDependencies[dependency].Add(job);
        }
    }
}