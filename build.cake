var target = Argument("target", "Default");
var solutionFiles = GetFiles("ParticleApp.sln");
var outputDirectory = "Build";
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
	if(System.IO.Directory.Exists(outputDirectory))
	{
		System.IO.Directory.Delete(outputDirectory, true);
	}
	
	System.IO.Directory.CreateDirectory(outputDirectory);
});

Task("UpdateAssemblyVersion")
	.Does(() =>
{
	var fixedVersionString = buildVersion.Replace("-beta-", ".").Replace("-alpha-",".");
	
	if(fixedVersionString.Split('.').Length == 3){
		fixedVersionString += ".0";
	}
	
	CreateAssemblyInfo("Particle-Win8\\Particle-Win8.Windows\\Properties\\AssemblyVersion.cs", new AssemblyInfoSettings
	{
		Version = fixedVersionString,
		FileVersion = fixedVersionString
	});
    Information("Update Package Manifest");
    
    var content = System.IO.File.ReadAllText("Particle-Win8\\Particle-Win8.Windows\\Package.appxmanifest");
    content = XmlPoke(content, "ns:Package/ns:Identity/@Version", fixedVersionString,new XmlPokeSettings {
        Namespaces = new Dictionary<string, string> {
            { /* Prefix */ "ns", /* URI */ "http://schemas.microsoft.com/appx/2010/manifest" }
        }
    });
    System.IO.File.WriteAllText("Particle-Win8\\Particle-Win8.Windows\\Package.appxmanifest", content);
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
		MSBuild(file, settings => settings
			.WithProperty("OutputPath", String.Format("..\\{0}\\", outputDirectory))
			.SetPlatformTarget(PlatformTarget.x86)
			.SetConfiguration("Release"));
	}
});


Task("Default")
	.IsDependentOn("Build");

RunTarget(target);