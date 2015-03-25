# Introduction

This repo is a dumping ground for materials pertaining to an internal MSBuild training.
It's unlikely to be very interesting to you if you didn't attend the talk.

# Agenda
1. Introduction, github location
2. MSBuild
  1. Versions
  2. Tools and documentation
    * MSDN
    * Command line
    * MSBuild Explorer
    * Debugging
3. Properties
  1. Imports & evaluation order
  2. Properties dictionary
  3. Special properties
    * Globals (immutable)
    * Enviromental
  4. Conditions
4. Items
  1. Grouping
  2. Metadata
  3. Scalar-vector duality
  4. Vectors again
5. Targets & Tasks
  1. Extending targets
  2. Authoring tasks
6. Demo
7. Extra
  1. Differences between AfterBuild target and custom tasks
  2. True order of MSBuild evaluation
  3. Item transforms

# Links

## Documentation
* [MSBuild reference](https://msdn.microsoft.com/en-us/library/0k6kkbsd.aspx)
* [MSBuild file schema](https://msdn.microsoft.com/en-us/library/5dy88c2e.aspx)

## Tooling
* [MSBuild Exlorer](http://www.msbuildexplorer.com/)
* Enabling MSBuild debugging: [explanation](http://blogs.msdn.com/b/visualstudio/archive/2010/07/06/debugging-msbuild-script-with-visual-studio.aspx), [alternative explanation](http://stackoverflow.com/a/28155244)

## Custom tasks
* [MSBuild Community Tasks](https://github.com/loresoft/msbuildtasks)
* [MSBuild Extension Pack](http://www.msbuildextensionpack.com/)

## Extra
* [MSBuild property functions](http://blogs.msdn.com/b/visualstudio/archive/2010/04/02/msbuild-property-functions.aspx)
* [Side effects of defining Properties inside Targets (paragraph "Subtle Effects of the Evaluation Order")](https://msdn.microsoft.com/en-us/library/dd997067.aspx)
* [Convert .sln to .msbuild](http://stackoverflow.com/a/3888083)