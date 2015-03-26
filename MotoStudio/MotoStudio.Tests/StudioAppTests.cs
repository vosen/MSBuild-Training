using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MotoStudio.Model;
using MotoStudio.Test;
using NUnit.Framework;

namespace MotoStudio.Tests
{
    public class StudioAppTests
    {
        private static string NullPathLoad()
        {
            return null;
        }

        private static Func<Stream> LoadFromString(string text)
        {
            return () => new MemoryStream(System.Text.Encoding.Unicode.GetBytes(text));
        }

        private static bool FailOnError(string s1, string s2)
        {
            Assert.Fail();
            return false;
        }

        private static bool CancelOnError(string s1, string s2)
        {
            return false;
        }

        [Test]
        public void LoadInvalidProject()
        {
            using (var temp = Utils.LoadResourceDirectory("Data"))
            {
                var app = StudioApp.Create(() => temp.FilePath("Invalid.msbuild"), CancelOnError);
                Assert.IsNull(app);
            }
        }

        [Test]
        public void OnConfirmedErrorProjectIsOpenedAgain()
        {
            int count = 0;
            using (var temp = Utils.LoadResourceDirectory("Data"))
            {
                var app = StudioApp.Create(() => temp.FilePath("Invalid.msbuild"), (s1, s2) =>
                {
                    if (count++ == 0)
                        return true;
                    if (count == 1)
                        Assert.Pass();
                    return false;
                });
                Assert.IsNull(app);
            }
        }

        [Test]
        public void EmptyProjectHasNoItems()
        {
            using (var temp = Utils.LoadResourceDirectory("Data"))
            {
                var app = StudioApp.Create(() => temp.FilePath("Empty.msbuild"), FailOnError);
                Assert.IsNotNull(app);
                Assert.AreEqual(0, app.Project.Items.Count);
            }
        }

        [Test]
        public void ProjectHasName()
        {
            using (var temp = Utils.LoadResourceDirectory("Data"))
            {
                var app = StudioApp.Create(() => temp.FilePath("Empty.msbuild"), FailOnError);
                Assert.IsNotNull(app);
                Assert.AreEqual("Empty", app.Project.Name);
            }
        }

        [Test]
        public void FoldersHaveNames()
        {
            using (var temp = Utils.LoadResourceDirectory("Data"))
            {
                var app = StudioApp.Create(() => temp.FilePath("Folder.msbuild"), FailOnError);
                Assert.IsNotNull(app);
                Assert.AreEqual(2, app.Project.Items.Count);
                Assert.DoesNotThrow(() => app.Project.Items.Single(i => i.Name == "src1"));
                Assert.DoesNotThrow(() => app.Project.Items.Single(i => i.Name == "src2"));
            }
        }

        [Test]
        public void OnlyFoldersAndItemsAreLoaded()
        {
            using (var temp = Utils.LoadResourceDirectory("Data"))
            {
                var app = StudioApp.Create(() => temp.FilePath("Simple.msbuild"), FailOnError);
                Assert.IsNotNull(app);
                Assert.AreEqual(2, app.Project.Items.Count);
                Assert.DoesNotThrow(() => app.Project.Items.Single(i => i.Name == "folder"));
                Assert.DoesNotThrow(() => app.Project.Items.Single(i => i.Name == "src.cs"));
            }
        }

        [Test]
        public void SubItemsAreLoaded()
        {
            using (var temp = Utils.LoadResourceDirectory("Data"))
            {
                var app = StudioApp.Create(() => temp.FilePath("Subitem.msbuild"), FailOnError);
                Assert.IsNotNull(app);
                Assert.AreEqual(1, app.Project.Items.Count);
                ITreeItem folder = null;
                Assert.DoesNotThrow(() => folder = app.Project.Items.Single(i => i.Name == "folder"));
                Assert.IsFalse(folder.IsFile);
                Assert.AreEqual(1, folder.Items.Count);
                Assert.AreEqual("src.cs", folder.Items[0].Name);
                Assert.True(folder.Items[0].IsFile);
            }
        }

        [Test]
        public void NestedSubItemsAreLoaded()
        {
            using (var temp = Utils.LoadResourceDirectory("Data"))
            {
                var app = StudioApp.Create(() => temp.FilePath("Subitem-nested.msbuild"), FailOnError);
                Assert.IsNotNull(app);
                Assert.AreEqual(1, app.Project.Items.Count);
                ITreeItem folder = null;
                Assert.DoesNotThrow(() => folder = app.Project.Items.Single(i => i.Name == "folder"));
                Assert.IsFalse(folder.IsFile);
                Assert.AreEqual(1, folder.Items.Count);
                Assert.AreEqual("subfolder", folder.Items[0].Name);
                Assert.False(folder.Items[0].IsFile);
                Assert.AreEqual("src.cs", folder.Items[0].Items[0].Name);
                Assert.True(folder.Items[0].Items[0].IsFile);
            }
        }

        [Test]
        public void ProjectCanBuild()
        {
            using (var temp = Utils.LoadResourceDirectory("Data"))
            {
                var app = StudioApp.Create(() => temp.FilePath("Targets-only.msbuild"), FailOnError);
                var result = app.Project.Build();
                Assert.True(result.Success);
                StringAssert.Contains("Target \"Build\" executed", result.Output);
            }
        }

        [Test]
        public void ProjectCanClean()
        {
            using (var temp = Utils.LoadResourceDirectory("Data"))
            {
                var app = StudioApp.Create(() => temp.FilePath("Targets-only.msbuild"), FailOnError);
                var result = app.Project.Clean();
                Assert.True(result.Success);
                StringAssert.Contains("Target \"Clean\" executed", result.Output);
            }
        }
    }
}
