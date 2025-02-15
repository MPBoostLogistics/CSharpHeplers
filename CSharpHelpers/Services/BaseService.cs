using System.Diagnostics;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace CSharpHelpers.Services
{
    abstract public class BaseService
    {
        #region Variables and constants
        public Version? AppVersion { get; }
        internal IConfigurationRoot? config;
        
        #endregion

        #region Constructors 
        public BaseService()
        {
            AppVersion = Assembly.GetExecutingAssembly().GetName().Version;
            BuildConfig();
        }

        public virtual void BuildConfig() 
        {
            if (FileService.GetProjectDirectory(out DirectoryInfo? projectDirectory)
                    && projectDirectory != null)
            {
                string? configFileName;
                if(IsRunningFromNUnit)
                    configFileName = "testConfig.json";
                else
                    configFileName = "config.json";

                if(!string.IsNullOrEmpty(configFileName)) 
                {
                    config = new ConfigurationBuilder()
                    .SetBasePath(projectDirectory.FullName)
                    .AddUserSecrets<BaseService>()
                    .AddJsonFile(configFileName, optional: false)
                    .Build();
                } 
                else 
                {
                    throw new Exception("Config json filename is null");
                }
            }
        }
        
        private static readonly bool IsRunningFromNUnit = 
            AppDomain.CurrentDomain
                .GetAssemblies()
                .Any(a => a != null &&
                    !string.IsNullOrEmpty(a.FullName) &&
                    a.FullName.ToLowerInvariant().StartsWith("nunit.framework"));
        
        
        #endregion
    }
}