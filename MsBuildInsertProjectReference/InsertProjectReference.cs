// -----------------------------------------------------------------------
// <copyright file="InsertProjectReference.cs" company="Ace Olszowka">
//  Copyright (c) Ace Olszowka 2019. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MsBuildInsertProjectReference
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;

    internal static class InsertProjectReference
    {
        private static XNamespace msbuildNS = "http://schemas.microsoft.com/developer/msbuild/2003";

        internal static bool Execute(string projFile, IEnumerable<string> projectsToInsert, bool saveChanges = false)
        {
            // Load up the project file
            XDocument projXml = XDocument.Load(projFile);

            // Determine Where to Insert Project References
            XElement projectReferenceItemGroup = _LocateOrCreateProjectReferenceItemGroup(projXml);

            foreach (string projectToInsert in projectsToInsert)
            {
                // Load up the ProjectInformation for the Reference to Insert
                ProjectInformation projectInformation = ProjectInformation.Parse(projectToInsert);

                // Create a Project Reference Element
                XElement projectReferenceFragment = _GenerateProjectReferenceFragment(projFile, projectInformation);

                // Now Insert it
                projectReferenceItemGroup.Add(projectReferenceFragment);
            }

            // See if there were any changes to this file
            bool hasChanges = projXml.ToString() != XDocument.Load(projFile).ToString();

            if (hasChanges && saveChanges)
            {
                projXml.Save(projFile);
            }

            return hasChanges;
        }

        /// <summary>
        /// Locate or Create an ItemGroup Specifically for ProjectReferences
        /// </summary>
        /// <param name="projXml">An MSBuild Style Project File.</param>
        /// <returns>The ItemGroup Element for ProjectReference Elements.</returns>
        /// <remarks>
        /// This attempts to mimic what Visual Studio would do.
        /// </remarks>
        private static XElement _LocateOrCreateProjectReferenceItemGroup(XDocument projXml)
        {
            XElement result;

            // First see if any ProjectReference Items Exist; if so add them to that ItemGroup
            XElement existingItemGroup = projXml.Descendants(msbuildNS + "ProjectReference").FirstOrDefault()?.Parent;

            if (existingItemGroup != null)
            {
                result = existingItemGroup;
            }
            else
            {
                // We're going to create an ItemGroup attached right after the Reference ItemGroup
                XElement existingReferenceItemGroup = projXml.Descendants(msbuildNS + "Reference").LastOrDefault().Parent;
                XElement newItemGroup = new XElement(msbuildNS + "ItemGroup");
                existingReferenceItemGroup.AddAfterSelf(newItemGroup);
                result = newItemGroup;
            }

            return result;
        }

        /// <summary>
        /// Generates a well-formed ProjectReference element
        /// </summary>
        /// <param name="projFile">The path to the project file that will contain this element.</param>
        /// <param name="reference">A <see cref="ProjectInformation"/> for the Project to add a reference to.</param>
        /// <returns>A well-formed ProjectReference Element.</returns>
        /// <remarks>
        /// See https://docs.microsoft.com/en-us/visualstudio/msbuild/common-msbuild-project-items?view=vs-2017
        /// for a description of what constitutes a well-formed ProjectReference Element.
        /// </remarks>
        private static XElement _GenerateProjectReferenceFragment(string projFile, ProjectInformation reference)
        {
            XElement projectReference =
                new XElement(
                    msbuildNS + "ProjectReference",
                    new XAttribute("Include", PathUtilities.GetRelativePath(projFile, reference.Path)),
                    new XElement(msbuildNS + "Project", reference.ProjectGuid),
                    new XElement(msbuildNS + "Name", Path.GetFileNameWithoutExtension(reference.Path)));

            return projectReference;
        }
    }
}
