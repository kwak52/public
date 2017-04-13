using System;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using System.IO;
using System.Diagnostics;

namespace Dsu.Driver.UI.Paix
{
    public partial class FormPathPlanner
    {
        private string Path2Custom(string path) => $"{Path.GetFileName(path)} @ {Path.GetDirectoryName(path)}";

        private void SaveToXmlFile(string file)
        {
            gridViewAxes.PostEditor();
            gridViewPosition.PostEditor();

            XElement poses = AuditPoses.PosesToXml(Poses);
            poses.Save(file);
            _activeFileName = file;
            //toolStripStatusLabel1.Text = file;
            labelFilePath.Text = Path2Custom(file);
        }

        private void ReadFromXmlFile(string file)
        {
            Poses = AuditPoses.LoadFromXml(file);
            _activeFileName = file;
            //toolStripStatusLabel1.Text = file;
            labelFilePath.Text = Path2Custom(file);
        }

        private string _activeFileName;
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "Pose files(*.poses)|*.poses|All files(*.*)|*.*";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                SaveToXmlFile(saveFileDialog1.FileName);
        }


        private void saveToolStripMenuItem_Click(object sender, EventArgs e) => SaveToXmlFile(_activeFileName);

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Pose files(*.poses)|*.poses|All files(*.*)|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                ReadFromXmlFile(openFileDialog1.FileName);
        }


        private void ReadFromPaixFile(string file)
        {
            Poses = AuditPoses.LoadFromPaix(file);
            labelFilePath.Text = Path2Custom(file);
        }

        private void importPAIXNodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "PAIX Node files(*.node)|*.node|All files(*.*)|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                ReadFromPaixFile(openFileDialog1.FileName);
        }
        private void saveGroupsIntoFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var folderDialog = new FolderBrowserDialog())
            {
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    var targetFolder = folderDialog.SelectedPath;
                    var groupPoses =
                        from p in Poses
                        group p by p.Group into g
                        select new { Group = g.Key, Poses = g.Select(p => p) };

                    foreach (var gp in groupPoses)
                    {
                        var xml = AuditPoses.PosesToXml(gp.Poses);
                        var path = $"{Path.Combine(targetFolder, gp.Group)}.poses";
                        xml.Save(path);
                    }

                    Process.Start(targetFolder);
                }
            }
        }
    }
}
