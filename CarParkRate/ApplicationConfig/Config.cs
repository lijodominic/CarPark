using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace AppConfig
{
    public enum ConnectionStringKey { 
        CoreSqlWrite, CoreSqlRead, 
        SummarySqlWrite, SummarySqlRead,
        SerializedSqlWrite, SerializedSqlRead,
    }

    public class Config : IConfig
    {
        private readonly IConfigurationRoot _configuration;
        private readonly AWSOptions _awsOptions;
        private readonly Deployment _deployment;
        private readonly Dictionary<ConnectionStringKey, string> _connectionStrings;

   
        public Config(Deployment deployment, bool setAwsCredentials = true)
        {
            var builder = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.base.json", optional: false)
                    .AddJsonFile($"appsettings.base.{deployment.ToString()}.json", optional: true)
                    .AddJsonFile($"appsettings.json", optional: true)
                    //.AddJsonFile($"appsettings.{deployment.ToString().ToLowerInvariant()}.json", optional: true)
                    .AddEnvironmentVariables()
                ;
            _deployment = deployment;
            _configuration = builder.Build();

            _connectionStrings = new Dictionary<ConnectionStringKey, string>
            {
                {
                    ConnectionStringKey.CoreSqlWrite,
                    GetConfig(new[] {"ConnectionStrings", ConnectionStringKey.CoreSqlWrite.ToString()})
                },
                {
                    ConnectionStringKey.CoreSqlRead,
                    GetConfig(new[] {"ConnectionStrings", ConnectionStringKey.CoreSqlRead.ToString()})
                },

                {
                    ConnectionStringKey.SummarySqlWrite,
                    GetConfig(new[] {"ConnectionStrings", ConnectionStringKey.SummarySqlWrite.ToString()})
                },
                {
                    ConnectionStringKey.SummarySqlRead,
                    GetConfig(new[] {"ConnectionStrings", ConnectionStringKey.SummarySqlRead.ToString()})
                },

                {
                    ConnectionStringKey.SerializedSqlWrite,
                    GetConfig(new[] {"ConnectionStrings", ConnectionStringKey.SerializedSqlWrite.ToString()})
                },
                {
                    ConnectionStringKey.SerializedSqlRead,
                    GetConfig(new[] {"ConnectionStrings", ConnectionStringKey.SerializedSqlRead.ToString()})
                },
            };

            // will be enabled once it is set on AWS
            if (setAwsCredentials)
            {
                var awsOptions = _configuration.GetAWSOptions();
                awsOptions.Credentials = new EnvironmentVariablesAWSCredentials();
                _awsOptions = awsOptions;
            }

        }

        public AWSOptions GetAwsOptions() => _awsOptions;

        public Deployment Deployment => _deployment;

        public string GetConnectionString(ConnectionStringKey key)
        {
            var connectionString = _connectionStrings.GetValueOrDefault(key);
            return connectionString;
        }

        public List<string> DevAuthorizedIps =>
            _configuration.GetSection("App:DevAuthorizedIps").Get<string[]>().ToList();

        public string GetS3Bucket(string bucket)
        {
            var deployment = Deployment.ToString().ToLower();
            var bucketName = GetConfig(new[] {"S3", bucket});
            var baseDomain = GetConfig(new[] {"S3", "BaseDomain"});

            return $"{deployment}.{bucketName}.{baseDomain}";
        }

        public string GetS3FullUrlPath(string bucket)
        {
            var baseUrl = GetConfig(new[] {"S3", "S3Url"});
            return $"{baseUrl}{GetS3Bucket(bucket)}";
        }

       

        public bool IsEnvironmentProduction()
        {
            return _deployment == Deployment.Production;
        }

        public string GetDevPrefix()
        {
            var devPrefix = GetConfig(new[] {"DevPrefix"});
            devPrefix = !string.IsNullOrEmpty(devPrefix) ? $"{devPrefix}-" : "";
            if (IsEnvironmentProduction())
                devPrefix = "";
            return devPrefix;
        }

        public string GetSNSArn(string topic, string overrideDeployment = null, bool withBase = true)
        {
            var devPrefix = GetDevPrefix();
            var baseArn = withBase ? $"{GetConfig(new[] {"SNS", "BaseArn"})}:" : "";
            var deployment = !string.IsNullOrEmpty(overrideDeployment)
                ? overrideDeployment.ToLower()
                : Deployment.ToString().ToLower();
            var topicName = GetConfig(new[] {"SNS", topic});

            return $"{baseArn}{devPrefix}{deployment}-{topicName}";
        }

        public string SesReceivingEmailDomain
        {
            get
            {
                var devPrefix = GetDevPrefix();
                var domain = GetConfig(new[] {"SES", "ReceivingEmailDomain"});
                return $"{devPrefix}{domain}";
            }
        }


        public string GetSQSArn(string queue, bool isDead = false, string overrideDeployment = null)
        {
            var queueName = GetSQSUrl(queue, isDead, overrideDeployment, false);
            var baseArn = $"{GetConfig(new[] {"SQS", "BaseArn"})}:";

            return $"{baseArn}{queueName}";
        }

        public string GetSQSUrl(string queue, bool isDead = false, string overrideDeployment = null,
            bool withBase = true)
        {
            var devPrefix = GetDevPrefix();
            var baseUrl = withBase ? $"{GetConfig(new[] {"SQS", "BaseUrl"})}/" : "";
            var deployment = !string.IsNullOrEmpty(overrideDeployment)
                ? overrideDeployment.ToLower()
                : Deployment.ToString().ToLower();
            var queueName = GetConfig(new[] {"SQS", queue});
            var suffix = isDead ? "-dead" : "";

            return $"{baseUrl}{devPrefix}{deployment}-{queueName}{suffix}";
        }

        public string GetCloudWatchSchedulerName(string name, string overrideDeployment = null)
        {
            var devPrefix = GetDevPrefix();
            var deployment = !string.IsNullOrEmpty(overrideDeployment)
                ? overrideDeployment.ToLower()
                : Deployment.ToString().ToLower();
            var cloudWatchName = GetConfig(new[] {"CloudWatch", name});

            return $"{devPrefix}{deployment}-{cloudWatchName}";
        }

        public string AwsAccountId => GetConfig(new[] {"AwsAccountId"});
        public string ParserApiUrl => GetConfig(new[] {"Parser", "ApiUrl"});
        public string ParserApiToken => GetConfig(new[] {"Parser", "ApiToken"});
        public string ParserApiCallbackKey => GetConfig(new[] {"Parser", "ApiCallbackKey"});
        public string ParserAppHandlerUrl => GetConfig(new[] {"Parser", "AppHandlerUrl"});
        public string ParserPhantomLiteUrl => GetConfig(new[] {"Parser", "PhantomLiteUrl"});
        public string AppDomain => GetConfig(new[] {"App", "Domain"});
        public string AppBrand => GetConfig(new[] {"App", "Brand"});
        public string AppWebsiteUrl => GetConfig(new[] {"App", "WebsiteUrl"});
        public int AppDefaultPageSize => Convert.ToInt32(GetConfig(new[] {"App", "DefaultPageSize"}));
        public string CatUIUrl => GetConfig(new[] { "CatUI", "CatUIUrl" });
        public string ResetPassword => GetConfig(new[] { "CatUI", "ResetPassword" });
        public string CatUIOnHoIdUrl => GetConfig(new[] { "CatUI", "CatUIOnHoIdUrl" });
        public string IpStackKey => GetConfig(new[] { "IpStack", "IpStackKey" });
        public string IpStackURL => GetConfig(new[] { "IpStack", "IpStackURL" });
        public string IdentityServerUrl => GetConfig(new[] {"IdentityServer", "Url"});
        public string IdentityServerClientId => GetConfig(new[] {"IdentityServer", "ClientId"});
        public string IdentityServerClientSecret => GetConfig(new[] {"IdentityServer", "ClientSecret"});
        public int IdentityServerRemoteTimeoutSeconds => Convert.ToInt32(GetConfig(new[] {"IdentityServer", "RemoteTimeoutSeconds"}));
        public bool EmailEnabled => Convert.ToBoolean(GetConfig(new[] {"Email", "Enabled"}));
        public string EmailOverride => GetConfig(new[] {"Email", "Override"});
        public string EmailFromName => GetConfig(new[] {"Email", "From", "Name"});
        public string EmailFromAddress => GetConfig(new[] {"Email", "From", "Address"});
        public string EmailAppOutbox => GetConfig(new[] {"Email", "AppOutbox"});
        public bool ExceptionlessEnabled => Convert.ToBoolean(GetConfig(new[] {"Exceptionless", "Enabled"}));
        public string ExceptionlessApiKey => GetConfig(new[] {"Exceptionless", "ApiKey"});
        public string ExceptionlessWorkerApiKey => GetConfig(new[] {"Exceptionless", "WorkerApiKey"});
        public string WorkerGroup => GetConfig(new[] { "Worker", "Group" });
        public int DisplayRowCountLimit => Convert.ToInt32(GetConfig(new[] { "DisplayRowCountLimit" }));
        public bool EnablePriceBenchMark => Convert.ToBoolean(GetConfig(new[] { "EnablePriceBenchMark" }));
        public string GetConfig(string[] args) => _configuration[$"{string.Join(":", args)}"];


        //// new keys
        ///
        public string SecretKey
        {
            get
            {
                return "SecretKeyForToken";
            }
        }
        public string GetGoogleMapBaseUrl()
        {
            var baseUrl = GetConfig(new[] { "GoogleMaps", "BaseUrl" });
            return baseUrl;
        }
        public string GetGoogleMapGeoCodeUrl()
        {
            var geoCodeUrl = GetConfig(new[] { "GoogleMaps", "GeoCodeUrl" });
            return geoCodeUrl;
        }
        public string GetGoogleMapApiBrowserKey()
        {
            var apiBrowserKey = GetConfig(new[] { "GoogleMaps", "ApiBrowserKey" });
            return apiBrowserKey;
        }
        public string GetGoogleMapApiServerKey()
        {
            var apiServerKey = GetConfig(new[] { "GoogleMaps", "ApiServerKey" });
            return apiServerKey;
        }
        public string GetGoogleMapApiClientId()
        {
            var apiClientId = GetConfig(new[] { "GoogleMaps", "ApiClientId" });
            return apiClientId;
        }

        public string GetGoogleMapApiPrivateKey()
        {
            var apiPrivateKey = GetConfig(new[] { "GoogleMaps", "ApiPrivateKey" });
            return apiPrivateKey;
        }
        public string GetGooglePlacesKey()
        {
            var apiPlacesKey = GetConfig(new[] { "GoogleMaps", "ApiPlacesKey" });
            return apiPlacesKey;
        }
    }




}
