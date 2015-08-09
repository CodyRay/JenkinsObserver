namespace JenkinsObserver.Test
{
    public static class ConstJson
    {
        public const string NODEJS_SERVER_NORMAL =

        #region Json

 @"{
  ""assignedLabels"" : [
    {
    }
  ],
  ""mode"" : ""NORMAL"",
  ""nodeDescription"" : ""the master Jenkins node"",
  ""nodeName"" : """",
  ""numExecutors"" : 1,
  ""description"" : null,
  ""jobs"" : [
    {
      ""name"" : ""libuv-julien"",
      ""url"" : ""http://jenkins.nodejs.org/job/libuv-julien/"",
      ""color"" : ""red""
    },
    {
      ""name"" : ""libuv-julien-windows"",
      ""url"" : ""http://jenkins.nodejs.org/job/libuv-julien-windows/"",
      ""color"" : ""red""
    },
    {
      ""name"" : ""libuv-master"",
      ""url"" : ""http://jenkins.nodejs.org/job/libuv-master/"",
      ""color"" : ""yellow""
    }
  ],
  ""overallLoad"" : {
  },
  ""primaryView"" : {
    ""name"" : ""All"",
    ""url"" : ""http://jenkins.nodejs.org/""
  },
  ""quietingDown"" : false,
  ""slaveAgentPort"" : 0,
  ""unlabeledLoad"" : {
  },
  ""useCrumbs"" : false,
  ""useSecurity"" : true,
  ""views"" : [
    {
      ""name"" : ""All"",
      ""url"" : ""http://jenkins.nodejs.org/""
    },
    {
      ""name"" : ""libuv"",
      ""url"" : ""http://jenkins.nodejs.org/view/libuv/""
    },
    {
      ""name"" : ""node"",
      ""url"" : ""http://jenkins.nodejs.org/view/node/""
    }
  ]
}";

        #endregion Json

        public const string NODEJS_JOB1_NORMAL =

        #region Json

 @"{
  ""actions"" : [
    {
    },
    {
    },
    {
    }
  ],
  ""description"" : """",
  ""displayName"" : ""libuv-julien"",
  ""displayNameOrNull"" : null,
  ""name"" : ""libuv-julien"",
  ""url"" : ""http://jenkins.nodejs.org/job/libuv-julien/"",
  ""buildable"" : true,
  ""builds"" : [
    {
      ""number"" : 256,
      ""url"" : ""http://jenkins.nodejs.org/job/libuv-julien/256/""
    },
    {
      ""number"" : 255,
      ""url"" : ""http://jenkins.nodejs.org/job/libuv-julien/255/""
    },
    {
      ""number"" : 254,
      ""url"" : ""http://jenkins.nodejs.org/job/libuv-julien/254/""
    },
    {
      ""number"" : 253,
      ""url"" : ""http://jenkins.nodejs.org/job/libuv-julien/253/""
    }
  ],
  ""color"" : ""red"",
  ""firstBuild"" : {
    ""number"" : 1,
    ""url"" : ""http://jenkins.nodejs.org/job/libuv-julien/1/""
  },
  ""healthReport"" : [
    {
      ""description"" : ""Build stability: All recent builds failed."",
      ""iconClassName"" : ""icon-health-00to19"",
      ""iconUrl"" : ""health-00to19.png"",
      ""score"" : 0
    },
    {
      ""description"" : ""Test Result: 0 tests failing out of a total of 758 tests."",
      ""iconClassName"" : ""icon-health-80plus"",
      ""iconUrl"" : ""health-80plus.png"",
      ""score"" : 100
    }
  ],
  ""inQueue"" : false,
  ""keepDependencies"" : false,
  ""lastBuild"" : {
    ""number"" : 256,
    ""url"" : ""http://jenkins.nodejs.org/job/libuv-julien/256/""
  },
  ""lastCompletedBuild"" : {
    ""number"" : 256,
    ""url"" : ""http://jenkins.nodejs.org/job/libuv-julien/256/""
  },
  ""lastFailedBuild"" : {
    ""number"" : 256,
    ""url"" : ""http://jenkins.nodejs.org/job/libuv-julien/256/""
  },
  ""lastStableBuild"" : null,
  ""lastSuccessfulBuild"" : {
    ""number"" : 75,
    ""url"" : ""http://jenkins.nodejs.org/job/libuv-julien/75/""
  },
  ""lastUnstableBuild"" : {
    ""number"" : 75,
    ""url"" : ""http://jenkins.nodejs.org/job/libuv-julien/75/""
  },
  ""lastUnsuccessfulBuild"" : {
    ""number"" : 256,
    ""url"" : ""http://jenkins.nodejs.org/job/libuv-julien/256/""
  },
  ""nextBuildNumber"" : 257,
  ""property"" : [
    {
    },
    {
    }
  ],
  ""queueItem"" : null,
  ""concurrentBuild"" : false,
  ""downstreamProjects"" : [

  ],
  ""scm"" : {
  },
  ""upstreamProjects"" : [

  ],
  ""activeConfigurations"" : [
    {
      ""name"" : ""label=osx"",
      ""url"" : ""http://jenkins.nodejs.org/job/libuv-julien/label=osx/"",
      ""color"" : ""blue""
    },
    {
      ""name"" : ""label=smartos"",
      ""url"" : ""http://jenkins.nodejs.org/job/libuv-julien/label=smartos/"",
      ""color"" : ""red""
    },
    {
      ""name"" : ""label=ubuntu-12.04"",
      ""url"" : ""http://jenkins.nodejs.org/job/libuv-julien/label=ubuntu-12.04/"",
      ""color"" : ""blue""
    },
    {
      ""name"" : ""label=ubuntu-14.04"",
      ""url"" : ""http://jenkins.nodejs.org/job/libuv-julien/label=ubuntu-14.04/"",
      ""color"" : ""blue""
    }
  ]
}";

        #endregion Json

        public const string NODEJS_JOB2_NORMAL =

        #region Json

 @"{
  ""actions"" : [
    {
    },
    {
    },
    {
    }
  ],
  ""description"" : """",
  ""displayName"" : ""libuv-julien-windows"",
  ""displayNameOrNull"" : null,
  ""name"" : ""libuv-julien-windows"",
  ""url"" : ""http://jenkins.nodejs.org/job/libuv-julien-windows/"",
  ""buildable"" : true,
  ""builds"" : [
    {
      ""number"" : 255,
      ""url"" : ""http://jenkins.nodejs.org/job/libuv-julien-windows/255/""
    },
    {
      ""number"" : 254,
      ""url"" : ""http://jenkins.nodejs.org/job/libuv-julien-windows/254/""
    },
    {
      ""number"" : 253,
      ""url"" : ""http://jenkins.nodejs.org/job/libuv-julien-windows/253/""
    }
  ],
  ""color"" : ""red"",
  ""firstBuild"" : {
    ""number"" : 82,
    ""url"" : ""http://jenkins.nodejs.org/job/libuv-julien-windows/82/""
  },
  ""healthReport"" : [
    {
      ""description"" : ""Build stability: All recent builds failed."",
      ""iconClassName"" : ""icon-health-00to19"",
      ""iconUrl"" : ""health-00to19.png"",
      ""score"" : 0
    },
    {
      ""description"" : ""Test Result: 0 tests in total."",
      ""iconClassName"" : ""icon-health-80plus"",
      ""iconUrl"" : ""health-80plus.png"",
      ""score"" : 100
    }
  ],
  ""inQueue"" : false,
  ""keepDependencies"" : false,
  ""lastBuild"" : {
    ""number"" : 255,
    ""url"" : ""http://jenkins.nodejs.org/job/libuv-julien-windows/255/""
  },
  ""lastCompletedBuild"" : {
    ""number"" : 255,
    ""url"" : ""http://jenkins.nodejs.org/job/libuv-julien-windows/255/""
  },
  ""lastFailedBuild"" : {
    ""number"" : 255,
    ""url"" : ""http://jenkins.nodejs.org/job/libuv-julien-windows/255/""
  },
  ""lastStableBuild"" : {
    ""number"" : 82,
    ""url"" : ""http://jenkins.nodejs.org/job/libuv-julien-windows/82/""
  },
  ""lastSuccessfulBuild"" : {
    ""number"" : 131,
    ""url"" : ""http://jenkins.nodejs.org/job/libuv-julien-windows/131/""
  },
  ""lastUnstableBuild"" : {
    ""number"" : 131,
    ""url"" : ""http://jenkins.nodejs.org/job/libuv-julien-windows/131/""
  },
  ""lastUnsuccessfulBuild"" : {
    ""number"" : 255,
    ""url"" : ""http://jenkins.nodejs.org/job/libuv-julien-windows/255/""
  },
  ""nextBuildNumber"" : 256,
  ""property"" : [
    {
    },
    {
    }
  ],
  ""queueItem"" : null,
  ""concurrentBuild"" : false,
  ""downstreamProjects"" : [

  ],
  ""scm"" : {
  },
  ""upstreamProjects"" : [

  ],
  ""activeConfigurations"" : [
    {
      ""name"" : ""DESTCPU=ia32,label=windows"",
      ""url"" : ""http://jenkins.nodejs.org/job/libuv-julien-windows/DESTCPU=ia32,label=windows/"",
      ""color"" : ""red""
    },
    {
      ""name"" : ""DESTCPU=x64,label=windows"",
      ""url"" : ""http://jenkins.nodejs.org/job/libuv-julien-windows/DESTCPU=x64,label=windows/"",
      ""color"" : ""red""
    }
  ]
}";

        #endregion Json

        public const string NODEJS_JOB3_NORMAL =

        #region Json

 @"{
  ""actions"" : [
    {
    },
    {
    },
    {
    }
  ],
  ""description"" : """",
  ""displayName"" : ""libuv-master"",
  ""displayNameOrNull"" : null,
  ""name"" : ""libuv-master"",
  ""url"" : ""http://jenkins.nodejs.org/job/libuv-master/"",
  ""buildable"" : true,
  ""builds"" : [
    {
      ""number"" : 1173,
      ""url"" : ""http://jenkins.nodejs.org/job/libuv-master/1173/""
    },
    {
      ""number"" : 1172,
      ""url"" : ""http://jenkins.nodejs.org/job/libuv-master/1172/""
    },
    {
      ""number"" : 1171,
      ""url"" : ""http://jenkins.nodejs.org/job/libuv-master/1171/""
    }
  ],
  ""color"" : ""yellow"",
  ""firstBuild"" : {
    ""number"" : 1,
    ""url"" : ""http://jenkins.nodejs.org/job/libuv-master/1/""
  },
  ""healthReport"" : [
    {
      ""description"" : ""Test Result: 3 tests failing out of a total of 1,026 tests."",
      ""iconClassName"" : ""icon-health-80plus"",
      ""iconUrl"" : ""health-80plus.png"",
      ""score"" : 100
    },
    {
      ""description"" : ""Build stability: No recent builds failed."",
      ""iconClassName"" : ""icon-health-80plus"",
      ""iconUrl"" : ""health-80plus.png"",
      ""score"" : 100
    }
  ],
  ""inQueue"" : false,
  ""keepDependencies"" : false,
  ""lastBuild"" : {
    ""number"" : 1173,
    ""url"" : ""http://jenkins.nodejs.org/job/libuv-master/1173/""
  },
  ""lastCompletedBuild"" : {
    ""number"" : 1173,
    ""url"" : ""http://jenkins.nodejs.org/job/libuv-master/1173/""
  },
  ""lastFailedBuild"" : {
    ""number"" : 1150,
    ""url"" : ""http://jenkins.nodejs.org/job/libuv-master/1150/""
  },
  ""lastStableBuild"" : {
    ""number"" : 2,
    ""url"" : ""http://jenkins.nodejs.org/job/libuv-master/2/""
  },
  ""lastSuccessfulBuild"" : {
    ""number"" : 1173,
    ""url"" : ""http://jenkins.nodejs.org/job/libuv-master/1173/""
  },
  ""lastUnstableBuild"" : {
    ""number"" : 1173,
    ""url"" : ""http://jenkins.nodejs.org/job/libuv-master/1173/""
  },
  ""lastUnsuccessfulBuild"" : {
    ""number"" : 1173,
    ""url"" : ""http://jenkins.nodejs.org/job/libuv-master/1173/""
  },
  ""nextBuildNumber"" : 1174,
  ""property"" : [
    {
    },
    {
    }
  ],
  ""queueItem"" : null,
  ""concurrentBuild"" : false,
  ""downstreamProjects"" : [

  ],
  ""scm"" : {
  },
  ""upstreamProjects"" : [

  ],
  ""activeConfigurations"" : [
    {
      ""name"" : ""label=smartos"",
      ""url"" : ""http://jenkins.nodejs.org/job/libuv-master/label=smartos/"",
      ""color"" : ""yellow""
    },
    {
      ""name"" : ""label=osx-build"",
      ""url"" : ""http://jenkins.nodejs.org/job/libuv-master/label=osx-build/"",
      ""color"" : ""blue""
    },
    {
      ""name"" : ""label=ubuntu-12.04"",
      ""url"" : ""http://jenkins.nodejs.org/job/libuv-master/label=ubuntu-12.04/"",
      ""color"" : ""blue""
    },
    {
      ""name"" : ""label=ubuntu-14.04"",
      ""url"" : ""http://jenkins.nodejs.org/job/libuv-master/label=ubuntu-14.04/"",
      ""color"" : ""blue""
    }
  ]
}";

        #endregion Json
    }
}