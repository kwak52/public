﻿using System;
using System.Collections.Generic;
using System.Text;
using DotNetSiemensPLCToolBoxLibrary.Projectfiles;

namespace DotNetSiemensPLCToolBoxLibrary.DataTypes.Projectfolders
{
    internal interface IProjectFolder
    {
        string Name { get; set; }
        Project Project { get; set; }
        ProjectFolder Parent { get; set; }
        List<ProjectFolder> SubItems { get; set; }

        string ProjectAndStructuredFolderName { get; }
    }
}
