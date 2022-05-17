using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GitCommands;
using GitExtUtils;
using GitUI.CommandsDialogs;
using GitUI.UserControls;
using JetBrains.Annotations;

namespace GitUI.HelperDialogs
{
    public partial class FormMergeProcess : FormProcess
    {
        [Obsolete("For VS designer and translation test only. Do not remove.")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private protected FormMergeProcess()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
            : base()
        {
            InitializeComponent();
        }

        public FormMergeProcess(GitUICommands commands, string process, ArgumentString arguments, string workingDirectory, string input, bool useDialogSettings)
            : base(commands: null, process, arguments, workingDirectory, input, useDialogSettings)
        {
            InitializeComponent();
            ResetAndRetry.Visible = false;
        }

        public static new bool ShowDialog([CanBeNull] IWin32Window owner, string process, ArgumentString arguments, string workingDirectory, string input, bool useDialogSettings)
        {
            Debug.Assert(owner is not null, "Progress window must be owned by another window! This is a bug, please correct and send a pull request with a fix.");

            using (var formMergeProcess = new FormMergeProcess(commands: null, process, arguments, workingDirectory, input, useDialogSettings))
            {
                formMergeProcess.ShowDialog(owner);
                return !formMergeProcess.ErrorOccurred();
            }
        }

        private bool _isReceivingLocalChanges = false;
        private List<string> _localChangedFiles = new List<string>();
        protected override void DataReceived(object sender, TextEventArgs e)
        {
            if (!_isReceivingLocalChanges &&
                (e.Text.Contains("error: Your local changes to the following files would be overwritten by merge") ||
                e.Text.Contains("error: The following untracked working tree files would be overwritten by merge")))
            {
                _isReceivingLocalChanges = true;
            }
            else if (_isReceivingLocalChanges &&
                (e.Text.Contains("Please commit your changes or stash them before you merge") ||
                e.Text.Contains("Please move or remove them before you merge")))
            {
                _isReceivingLocalChanges = false;
            }
            else if (_isReceivingLocalChanges)
            {
                // Removing /t and /n
                var fileName = new string(e.Text.Skip(1).Take(e.Text.Length - 2).ToArray());
                _localChangedFiles.Add(fileName);
            }

            if (e.Text.Contains("Aborting") && _localChangedFiles.Count > 0)
            {
                ResetAndRetry.Visible = true;
            }
        }

        private void ResetAndRetry_Click(object sender, EventArgs e)
        {
            if (_localChangedFiles.Count == 0)
            {
                return;
            }

            var fullPathResolver = new FullPathResolver(() => WorkingDirectory);
            var gitUICommands = new GitUICommands(WorkingDirectory);
            var files = gitUICommands.Module.GetWorkTreeFiles()
                .Where(item => _localChangedFiles.Contains(item.Name));

            FormResetChanges.ActionEnum resetType = FormResetChanges.ShowResetDialog(this, files.Any(item => !item.IsNew), files.Any(item => item.IsNew));
            if (resetType == FormResetChanges.ActionEnum.Cancel)
            {
                return;
            }

            ResetAndRetry.Visible = false;

            var deleteNewFiles = files.Any(item => item.IsNew) && (resetType == FormResetChanges.ActionEnum.ResetAndDelete);
            var filesToReset = new List<string>();
            var filesInUse = new List<string>();
            var output = new StringBuilder();
            foreach (var file in files)
            {
                if (file.IsNew)
                {
                    if (deleteNewFiles)
                    {
                        try
                        {
                            string path = fullPathResolver.Resolve(file.Name);
                            if (File.Exists(path))
                            {
                                File.Delete(path);
                            }
                            else if (Directory.Exists(path))
                            {
                                Directory.Delete(path, recursive: true);
                            }
                        }
                        catch (IOException)
                        {
                            filesInUse.Add(file.Name);
                        }
                        catch (UnauthorizedAccessException)
                        {
                        }
                    }
                }
                else
                {
                    filesToReset.Add(file.Name);
                }
            }

            output.Append(gitUICommands.Module.ResetFiles(filesToReset));

            if (filesInUse.Count > 0)
            {
                MessageBox.Show(this, "The following files are currently in use and will not be reset:" + Environment.NewLine + "\u2022 " + string.Join(Environment.NewLine + "\u2022 ", filesInUse), "Files In Use", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            if (!string.IsNullOrEmpty(output.ToString()))
            {
                AppendOutput(output.ToString());
            }

            if (filesInUse.Count == 0)
            {
                Retry();
            }
        }
    }
}
