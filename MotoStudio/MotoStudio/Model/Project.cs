using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Execution;
using MsProject = Microsoft.Build.Evaluation.Project;
using Microsoft.Build.Framework;

namespace MotoStudio.Model
{
    public class Project : ITreeItem
    {
        private readonly MsProject proj;

        public MsProject MsProject { get { return proj; } }
        public string Name { get { return Path.GetFileNameWithoutExtension(proj.FullPath); } }
        public IList<ITreeItem> Items { get; private set; }

        public Project(MsProject proj)
        {
            this.proj = proj;
            Items = new List<ITreeItem>();
            foreach (ProjectItem item in proj.Items.Where(CanLoadItem))
            {
                string[] segments = item.EvaluatedInclude.Split(new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);
                AddSubitem(this, item.ItemType == "Compile", segments);
            }
        }

        private static void AddSubitem(ITreeItem item, bool isFile, string[] segments)
        {
            ITreeItem subRoot = item.Items.FirstOrDefault(i => segments[0] == i.Name);
            if (subRoot == null)
                item.Items.Add(new Item(item, segments, isFile));
            else
                AddSubitem(subRoot, isFile, segments.Skip(1).ToArray());
        }

        private static bool CanLoadItem(ProjectItem i)
        {
            switch (i.ItemType)
            {
                case "Compile":
                case "Folder":
                    return true;
            }
            return false;
        }

        private BuildResult BuildTarget(string target)
        {
            var logger = new StringLogger();
            var result = BuildManager.DefaultBuildManager.Build(
                new BuildParameters() { Loggers = new ILogger[] { logger } },
                new BuildRequestData(this.proj.CreateProjectInstance(), new string[] { target }));
            return new BuildResult(result.OverallResult == BuildResultCode.Success, logger.GetText());
        }

        public BuildResult Build()
        {
            return BuildTarget("Build");
        }

        public BuildResult Clean()
        {
            return BuildTarget("Clean");
        }

        public bool IsFolder
        {
            get { return false; }
        }


        public ITreeItem Parent
        {
            get { return null; }
        }
    }
}
