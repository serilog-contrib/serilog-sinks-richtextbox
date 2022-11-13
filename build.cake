#addin "nuget:?package=Cake.MinVer&version=2.0.0"
#addin "nuget:?package=Cake.Args&version=3.0.0"

var target       = ArgumentOrDefault<string>("target") ?? "pack";
var buildVersion = MinVer(s => s.WithTagPrefix("v").WithDefaultPreReleasePhase("preview"));

Task("clean")
    .Does(() =>
{
    CleanDirectories("./artifact/**");
    CleanDirectories("./**/^{bin,obj}");
});

Task("restore")
    .IsDependentOn("clean")
    .Does(() =>
{
    DotNetRestore("./serilog-sinks-richtextbox.sln", new DotNetRestoreSettings
    {
        LockedMode = true,
    });
});

Task("build")
    .IsDependentOn("restore")
    .DoesForEach(new[] { "Debug", "Release" }, (configuration) =>
{
    DotNetBuild("./serilog-sinks-richtextbox.sln", new DotNetBuildSettings
    {
        Configuration = configuration,
        NoRestore = true,
        NoIncremental = false,
        MSBuildSettings = new DotNetMSBuildSettings
        {
            Version = buildVersion.Version,
            AssemblyVersion = buildVersion.AssemblyVersion,
            FileVersion = buildVersion.FileVersion,
            ContinuousIntegrationBuild = BuildSystem.IsLocalBuild,
        },
    });
});

Task("test")
    .IsDependentOn("build")
    .Does(() =>
{
    var settings = new DotNetTestSettings
    {
        Configuration = "Release",
        NoRestore = true,
        NoBuild = true,
    };

    var projectFiles = GetFiles("./test/**/*.csproj");
    foreach (var file in projectFiles)
    {
        DotNetTest(file.FullPath, settings);
    }
});

Task("pack")
    .IsDependentOn("test")
    .Does(() =>
{
    var releaseNotes = $"https://github.com/serilog-contrib/serilog-sinks-richtextbox/releases/tag/v{buildVersion.Version}";

    DotNetPack("./src/Serilog.Sinks.RichTextBox.Wpf/Serilog.Sinks.RichTextBox.Wpf.csproj", new DotNetPackSettings
    {
        Configuration = "Release",
        NoRestore = true,
        NoBuild = true,
        OutputDirectory = "./artifact/nuget",
        MSBuildSettings = new DotNetMSBuildSettings
        {
            Version = buildVersion.Version,
            PackageReleaseNotes = releaseNotes,
        },
    });
});

Task("push")
    .IsDependentOn("pack")
    .Does(context =>
{
    var url =  context.EnvironmentVariable("NUGET_URL");
    if (string.IsNullOrWhiteSpace(url))
    {
        context.Information("No NuGet URL specified. Skipping publishing of NuGet packages");
        return;
    }

    var apiKey =  context.EnvironmentVariable("NUGET_API_KEY");
    if (string.IsNullOrWhiteSpace(apiKey))
    {
        context.Information("No NuGet API key specified. Skipping publishing of NuGet packages");
        return;
    }

    var nugetPushSettings = new DotNetNuGetPushSettings
    {
        Source = url,
        ApiKey = apiKey,
    };

    foreach (var nugetPackageFile in GetFiles("./artifact/nuget/*.nupkg"))
    {
        DotNetNuGetPush(nugetPackageFile.FullPath, nugetPushSettings);
    }
});

RunTarget(target);
