# Upgrading to VS2017
It was hoped that by applying the steps in [Make Extensions Compatible with Visual Studio 2017 and Visual Studio 2015](https://docs.microsoft.com/en-us/visualstudio/extensibility/how-to-roundtrip-vsixs), 
I can make a binary that works across both vs-2015 and vs-2017. However, it is found there are breaking changes in vs-2017, so branch VS2017 is created, the final result is each branch only works with respect to its coressponding vs version.
T
he assembly version is bumped to 3.0, and the vsix product version is bumped to 2.0.

# Introduction

The Guidance Automation Extensions (GAX) expands the capabilities of Visual Studio by running guidance packages, which automate key development tasks from within the Visual Studio environment. The Guidance Automation Toolkit (GAT) allows authoring of guidance packages. Please refer to the [original reference documentation on MSDN](https://msdn.microsoft.com/en-us/library/ff709808.aspx) for more information.

_Note: This is the open-sourced version of the GAX/GAT 2010 originally built by Microsoft patterns & practices. Microsoft no longer maintains this project and no future releases are planned._

# OpenGAX VSIX Manifest Configuration

Depending on the version of Visual Studio (and therefore OpenGAX) you are targeting, you will need to update your VSIX manifest file (typically "source.extension.vsixmanifest"). The most important parts of the manifest are shown below:

```
<Vsix>
  ...snip...

    <SupportedProducts>
      <VisualStudio Version="14.0">
        <Edition>Enterprise</Edition>
        ...

  ...snip...

  <Reference Id="OpenGAX-VS2015" MinVersion="1.0">
    <Name>Guidance Automation Extensions</Name>
  </Reference>

  ...snip...

  <Content>
    <VsPackage>|%CurrentProject%;PkgdefProjectOutputGroup|</VsPackage>
  </Content>
</Vsix>
```

### Supported Products

Depending on the version of Visual Studio and the edition(s) you want to enable, you must update the supported products in the VSIX manifest file. The `Version` attribute must match the Visual Studio version number (e.g. "14.0" for VS 2015) and the `Edition` attribute must refer to one or more valid SKU's (e.g. "Pro" or "Enterprise" for VS 2015).

### VSIX References

You must update the VSIX `Reference` element to the proper version of OpenGAX:

Visual Studio Version | VSIX ID | VSIX Version
--------------------- | ------- | ------------
VS 2012 | OpenGAX | 3.0
VS 2013 | OpenGAX-VS2013 | 1.0
VS 2015 | OpenGAX-VS2015 | 1.0

### Declare VsPackage

Because of a change in the way VSIX packages are registered in Visual Studio 2012 and above, you must explicitly mention the fact that there is a Visual Studio Package inside the VSIX by including it as a `Content` element in the VSIX manifest file. Otherwise, the VSIX will install just fine but appear not to work or simply doesn't show up anywhere inside Visual Studio. This typically means including the `<VsPackage>` element as shown above.

# Porting existing packages to the open source GAX

Guidance packages developed using the original GAX 2010 (this includes all software factories from p&p, like the Web Services Software Factory) will not run or build against the new open source GAX. While no source code changes have been introduced in the open source GAX -meaning it is currently source code compatible with GAX 2010- the new open source GAX assemblies are not signed by Microsoft and wonâ€™t have a strong name unless you update the projects to use your own key. This means you will have to make sure your guidance package source code is modified to not include any references to GAX 2010 assemblies.

You should make sure to perform the steps below.

### Update Assembly References

All assembly file references to GAX 2010 assemblies must be updated to now point to the DLLs for the Open Source GAX.

If your guidance package is referencing assemblies that were previously built against the GAX 2010 assemblies, you will need to recompile these assemblies using the new open source GAX ones.

### Update Public Key References

All references to the Microsoft public key token â€?1bf3856ad364e35â€?must be updated to point to â€œnullâ€?or a new public key token if you are signing the assemblies. One place where you will find this is in .vstemplate files containing a Wizard Extension because these reference a GAX 2010 assembly using its strong name, for example:

```
<WizardExtension>
    <Assembly>Microsoft.Practices.RecipeFramework.VisualStudio, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35</Assembly>
    <FullClassName>Microsoft.Practices.RecipeFramework.VisualStudio.Templates.UnfoldTemplate</FullClassName>
</WizardExtension>
```

This can simply be replaced by the following:

```
<WizardExtension>
    <Assembly>Microsoft.Practices.RecipeFramework.VisualStudio</Assembly>
    <FullClassName>Microsoft.Practices.RecipeFramework.VisualStudio.Templates.UnfoldTemplate</FullClassName>
</WizardExtension>
```

### Update VSIX References

In the VSIX manifest file, you must update the reference to GAX from the previous VSIX ID (`Microsoft.Practices.RecipeFramework.VisualStudio` for GAX and `Microsoft.Practices.RecipeFramework.MetaGuidancePackage` for GAT) to the new open source ID.

See the table above for the correct ID depending on the version of Visual Studio you are targeting.

### T4 Reference
The syntax: <#@ assembly name="[assembly strong name|assembly file path]" #> 

Auto included assembly:
Microsoft.VisualStudio.TextTemplating.1*.dll
System.dll
WindowsBase.dll

Senerio:
1. Runtime templating: by MS definition, means the preprocessed template, this directive has no effect, use project references.
2. Design time templating
The list of assemblies seen by the template is separate from the list of References in the application project.
3. Runtime templating but no preprocessed, same as 2, regardless of appdomain
