var gulp = require('gulp');
var msbuild = require('gulp-msbuild');
var shell = require('gulp-shell');
var argv = require('yargs').argv;
var del = require('del');

var configuration = argv.configuration || "Release";
var environment = argv.environment;
var connectionString = argv.connectionString;

function targets() {
    return {
        targets: Array.prototype.slice.call(arguments),
        configuration: configuration,
        properties: {
            GenerateFullPaths: true
        },
        verbosity: "quiet",
        toolsVersion: 14.0,
        errorOnFail: true,
        stdout: true
    };
}

function nuget(args) {
    return "tools\\NuGet.exe " + args + " -Verbosity quiet";
}

function fixie(args) {
    return "packages\\Fixie.1.0.0.18\\lib\\net45\\Fixie.Console.exe " + args;
}

gulp.task('default', ["build"]);

gulp.task('init', shell.task(nuget("restore")));

gulp.task("clean", ["init"], function() {
    return gulp.src("*.sln")
        .pipe(msbuild(targets("Clean")));
});

gulp.task("build", ["init"], function() {
    return gulp.src("*.sln")
        .pipe(msbuild(targets("Build")));
});

gulp.task("test", ["build"], function() {
    return gulp.src("*.Tests/bin/" + configuration + "/*.Tests.dll")
        .pipe(shell(fixie("<%= file.path %>")));
});

gulp.task("package:clean", function () {
    return del("packages/*.nupkg");
});

gulp.task("package", ["test", "package:clean"], function () {
    var exportedPackages = [
        "Common.*/*.csproj",
        "*.Published/*.csproj"
    ];
    return gulp.src(exportedPackages)
        .pipe(shell(nuget("pack <%= file.path %> -OutputDirectory packages -Properties Configuration=" + configuration)));
});

gulp.task("publish", ["package"], function() {
    gulp.src("packages/*.nupkg")
        .pipe(shell(nuget("push <%= file.path %> tHg1r8r3T5hCn4M")));
});

gulp.task("deploy", ["test"], function () {
    var deployment = targets("Build");
    deployment.properties = {
        DeployOnBuild: true,
        PublishProfile: environment
    }
    return gulp.src("*.sln")
        .pipe(msbuild(deployment));
});