var target = Argument("target", "Default");
var solutionFiles = GetFiles("ParticleApp.Win8.sln");
var projectFolders = GetDirectories("Particle-Win8\\Particle-Win8.Windows");
var globalPackageDirectory = MakeAbsolute(Directory("AppPackages"));
var outputDirectory = Directory("Build");
var buildVersion = "0.4.3.2";

Task("AppVeyorUpdate")
	.Does(() =>
{
	if(AppVeyor.IsRunningOnAppVeyor)
	{
		Information("Building on AppVeyor");
		buildVersion = AppVeyor.Environment.Build.Version;
		Information("Build Version is {0}", buildVersion);
	}
	else
	{
		Information("Not building on AppVeyor");
	}
});

Task("CleanUp")
	.Does(()=>
{
    CleanDirectory(outputDirectory);
    CleanDirectory(globalPackageDirectory);
    
    foreach(var folder in projectFolders)
    {
        Information("Cleaning up {0}\\AppPackages", folder);
        CleanDirectory(folder + "\\AppPackages");
    }
});

Task("UpdateAssemblyVersion")
	.Does(() =>
{
	var fixedVersionString = buildVersion.Replace("-beta-", ".").Replace("-alpha-",".");
	
	if(fixedVersionString.Split('.').Length == 3){
		fixedVersionString += ".0";
	}
    
    foreach(var folder in projectFolders)
    {
        Information("Updating Version info in {0}", folder);
        var assemblyVersionFile = folder + "\\Properties\\AssemblyVersion.cs";
        CreateAssemblyInfo(assemblyVersionFile, new AssemblyInfoSettings
        {
            Version = fixedVersionString,
            FileVersion = fixedVersionString
        });
        var appxmanifestFile = folder + "\\Package.appxmanifest";
        var content = System.IO.File.ReadAllText(appxmanifestFile);
        XmlPoke(content, "ns:Package/ns:Identity/@Version", fixedVersionString,new XmlPokeSettings {
            Namespaces = new Dictionary<string, string> {
                { /* Prefix */ "ns", /* URI */ "http://schemas.microsoft.com/appx/2010/manifest" }
            }
        });
        System.IO.File.WriteAllText(appxmanifestFile, content);
    }
});

Task("Build")
	.IsDependentOn("CleanUp")
	.IsDependentOn("AppVeyorUpdate")
	.IsDependentOn("UpdateAssemblyVersion")
	.Does(()=>
{
	foreach(var file in solutionFiles)
	{
		Information("Restoring {0}", file);
		NuGetRestore(file);	
		Information("Building {0}", file);
        var settings = new MSBuildSettings();
        settings.WithProperty("OutputPath", System.IO.Path.GetFullPath(outputDirectory))
            .SetConfiguration("Release")
            .SetPlatformTarget(PlatformTarget.x86);
		MSBuild(file, settings);
        settings.SetPlatformTarget(PlatformTarget.x64);
        MSBuild(file, settings);
        settings.SetPlatformTarget(PlatformTarget.ARM);
        MSBuild(file, settings);
       
	}
    
    foreach(var folder in projectFolders)
    {
		var source = folder + "\\AppPackages";
		var destName = System.IO.Path.GetFileName(folder.ToString());
		Zip(source, System.IO.Path.Combine(globalPackageDirectory.ToString(), destName + ".zip"));
    }
});


Task("Default")
	.IsDependentOn("Build");

RunTarget(target);