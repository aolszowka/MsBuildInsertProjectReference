// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Ace Olszowka">
//  Copyright (c) Ace Olszowka 2019. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MsBuildInsertProjectReference
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    class Program
    {
        static void Main(string[] args)
        {
            string targetPath = @"S:\TimsSVN\8x\Trunk\Synergy";
            string[] projectsToInsert = new string[] { @"S:\TimsSVN\8x\Trunk\Synergy\Schemas\Repository.synproj" };

            IEnumerable<string> synnetProjects = Directory.EnumerateFiles(targetPath, "*.DblNet.synproj", SearchOption.AllDirectories);

            Parallel.ForEach(synnetProjects, synnetProject =>
            {
                InsertProjectReference.Execute(synnetProject, projectsToInsert, true);
            }
            );
        }
    }
}
