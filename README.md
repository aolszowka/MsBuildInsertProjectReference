# MsBuildInsertProjectReference
Mass Insertion of ProjectReferences into MsBuild Based Projects

## Background
In a large source tree it is sometimes necessary to insert (multiple) ProjectReference ([Microsoft Docs: Common MSBuild project items](https://docs.microsoft.com/en-us/visualstudio/msbuild/common-msbuild-project-items?view=vs-2017)) into a significant number of MsBuild Projects.

The Microsoft Solution is to open each of these projects in Visual Studio and use the Add Reference Dialog to add them manually. However this gets really old after the 5th project or so (especially when you have 2,000 more projects to go).

## When To Use This Tool
This tool is used when you want to insert any number of ProjectReferences into any number of Projects.

## Hacking
This tool is rough around the edges and was written to be quick and dirty; there is plenty of room for improvements to improve its reusability.

## Contributing
Pull requests and bug reports are welcomed so long as they are MIT Licensed.

## License
This tool is MIT Licensed.