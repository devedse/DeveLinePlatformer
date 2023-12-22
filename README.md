# DeveLinePlatformer

[![Join the chat at https://gitter.im/DeveLinePlatformer/Lobby](https://badges.gitter.im/DeveLinePlatformer/Lobby.svg)](https://gitter.im/DeveLinePlatformer/Lobby?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

This is the new version of my maze generator, now made with .NET Core.

## Build status

| GitHubActions Builds |
|:--------------------:|
| [![GitHubActions Builds](https://github.com/devedse/DeveLinePlatformer/workflows/GitHubActionsBuilds/badge.svg)](https://github.com/devedse/DeveLinePlatformer/actions/workflows/githubactionsbuilds.yml) |

## DockerHub

| Docker Hub |
|:----------:|
| [![Docker pulls](https://img.shields.io/docker/v/devedse/DeveLinePlatformerweb)](https://hub.docker.com/r/devedse/DeveLinePlatformerweb/) |

## Code Coverage Status

| CodeCov |
|:-------:|
| [![codecov](https://codecov.io/gh/devedse/DeveLinePlatformer/branch/master/graph/badge.svg)](https://codecov.io/gh/devedse/DeveLinePlatformer) |

## Code Quality Status

| SonarQube |
|:---------:|
| [![Quality Gate](https://sonarcloud.io/api/project_badges/measure?project=DeveLinePlatformer&metric=alert_status)](https://sonarcloud.io/dashboard?id=DeveLinePlatformer) |

## Package

| NuGet |
|:-----:|
| [![NuGet](https://img.shields.io/nuget/v/DeveLinePlatformer.svg)](https://www.nuget.org/packages/DeveLinePlatformer/) |

## Deployment status

(If an image of a Maze is shown below, the deployment is working)

| Azure Web Deployment | Azure Docker Deployment |
|:--------------------:|:-----------------------:|
| [![Azure web deployment down :(](http://DeveLinePlatformerweb.azurewebsites.net/api/mazes/MazePath/192/64)](http://DeveLinePlatformerweb.azurewebsites.net/api/mazes/MazePath/192/64) | [![Docker deployment down :(](http://DeveLinePlatformerdocker.azurewebsites.net/api/mazes/MazePath/192/64)](http://DeveLinePlatformerdocker.azurewebsites.net/api/mazes/MazePath/192/64) |

## Build and Deployment details

### Travis

**Obsolete**

The Travis build will also run publish and then create a docker image which is automatically published to here:
https://hub.docker.com/r/devedse/DeveLinePlatformer/

Azure will then pick up the docker image and automatically deploy it using the Web App On Linux (preview) to:
http://DeveLinePlatformerdocker.azurewebsites.net/api/mazes/MazePath/512/512

Azure will also do a seperate deployment/build when a push to git has occured:
http://DeveLinePlatformerweb.azurewebsites.net/api/mazes/MazePath/512/512

### AppVeyor:

**Obsolete**

AppVeyor will create a number of build artefacts which are added as releases on Github so they can be downloaded:
* DeveLinePlatformer.7z (Build output as 7z)
* DeveLinePlatformer.zip (Build output as zip)
* DeveLinePlatformer.x.x.x.nupkg (Nuget package of library)
* DeveLinePlatformer.x.x.x.symbols.nupkg (Nuget package of symbols for library)

## Maze generator details

As of the latest version it is now also possible to generate mazes the size on 2^30 * 2^30 dynamically with a path.

Use the following url as an example:
http://DeveLinePlatformerdocker.azurewebsites.net/api/mazes/MazeDynamicPathSeedPart/1337/1073741824/1073741824/0/0/512/512
http://DeveLinePlatformerweb.azurewebsites.net/api/mazes/MazeDynamicPathSeedPart/1337/1073741824/1073741824/0/0/512/512
