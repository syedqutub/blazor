#addin "nuget:?package=Cake.Git&version=1.0.0"
#addin "nuget:?package=Cake.FileHelpers&version=4.0.0"

var target = Argument("target","Default");
var slnForlder = "./";
var configuration = Argument("configuration","Release");
var webApiFolder = "./Client";
var publishFolder = Argument("publish",$"{webApiFolder}/bin/Publish");

Task("Clean")
    .Does(() =>
{
});

Task("Restore")
    .Does(() =>
{
    DotNetCoreRestore(slnForlder);
});

Task("Compile")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .Does(() =>
{
    DotNetCoreBuild(slnForlder,new DotNetCoreBuildSettings
    {
        NoRestore = true,
        Configuration = configuration
    });
});

Task("Test")
    .IsDependentOn("Compile")
    .Does(() =>
{
    // uncomment if you want to enable tests.
    // DotNetCoreTest(slnForlder,new DotNetCoreTestSettings
    // {
    //     NoRestore = true,
    //     NoBuild = true,
    //     Configuration = configuration
    // });
});

Task("Publish")
    .IsDependentOn("Test")
    .Does(() =>
{
    DotNetCorePublish(webApiFolder, new DotNetCorePublishSettings
    {
        NoRestore = true,
        NoBuild = true,
        Configuration = configuration,
        OutputDirectory = publishFolder
    });

    ReplaceTextInFiles($"{publishFolder}/wwwroot/index.html",@"<base href=""/"" />",@"<base href=""/blazor/"" />");
    
    // writes text to file. create a new file if does not exists.
    FileWriteText($"{publishFolder}/wwwroot/.nojekyll","");
});

Task("Deploy")
    .IsDependentOn("Publish")
    .Does(() =>
{
    
});

Task("Default")
    .IsDependentOn("Deploy")
    .Does(() =>
{
    
});

RunTarget(target);
