using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace CSharpHelpers.Services
{
    abstract public class BaseService
    {
        #region Variables and constants
        public Version? AppVersion { get; }
        internal IConfigurationRoot config;
        #endregion

        #region Constructors 
        public BaseService()
        {
            AppVersion = Assembly.GetExecutingAssembly().GetName().Version;
            
             config = new ConfigurationBuilder()
                .AddUserSecrets<BaseService>()
                .Build();
        }
        #endregion
    }
}