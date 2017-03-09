using System.Collections.Generic;
using System.Collections.ObjectModel;
using Pihrtsoft.Snippets.CodeGeneration.Commands;

namespace Pihrtsoft.Snippets.CodeGeneration
{
    public class JobCollection : Collection<Job>
    {
        public void AddRange(IEnumerable<Job> jobs)
        {
            foreach (Job job in jobs)
                Add(job);
        }

        public bool IsEmpty
        {
            get { return Items.Count == 0; }
        }

        public void AddCommands(IEnumerable<Command> commands)
        {
            if (IsEmpty)
            {
                foreach (Command command in commands)
                    Add(new Job(command));
            }
            else
            {
                CartesianProduct(commands);
            }
        }

        private void CartesianProduct(IEnumerable<Command> commands)
        {
            using (IEnumerator<Command> en = commands.GetEnumerator())
            {
                if (en.MoveNext())
                {
                    Command first = en.Current;

                    var jobs = new List<Job>();

                    while (en.MoveNext())
                        jobs.AddRange(WithCommand(en.Current));

                    foreach (Job job in Items)
                        job.Commands.Add(first);

                    AddRange(jobs);
                }
            }
        }

        public void AddCommand(Command command)
        {
            if (IsEmpty)
            {
                Add(new Job(command));
            }
            else
            {
                AddRange(WithCommand(command));
            }
        }

        private List<Job> WithCommand(Command command)
        {
            var jobs = new List<Job>(Count);

            foreach (Job item in Items)
            {
                var job = new Job(item.Commands);
                job.Commands.Add(command);
                jobs.Add(job);
            }

            return jobs;
        }
    }
}
