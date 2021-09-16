using System;
using System.Collections.Generic;
using Amazon.Extensions.NETCore.Setup;

namespace AppConfig
{
    public enum Deployment { Development, Test, Demo, Production }

    public interface IConfig
    {
        Deployment Deployment { get; }
        string GetConnectionString(ConnectionStringKey key);
        string GetS3Bucket(string bucket);
        string GetSNSArn(string topic, string overrideDeployment = "", bool withBase = true);
        string GetSQSArn(string queueName, bool isDead = false, string overrideDeployment = "");
        string GetSQSUrl(string queueName, bool isDead = false, string overrideDeployment = "", bool withBase = true);
        string GetCloudWatchSchedulerName(string name, string overrideDeployment = null);
        AWSOptions GetAwsOptions();
        List<string> DevAuthorizedIps { get; }
        string ParserApiUrl { get; }
        string ParserApiToken { get; }
        string ParserApiCallbackKey { get; }
        string ParserAppHandlerUrl { get; }
        string ParserPhantomLiteUrl { get; }
        string AppDomain { get; }
        string AppBrand { get; }
        string AppWebsiteUrl { get; }
        int AppDefaultPageSize { get; }
        string IdentityServerUrl { get; }
        string IdentityServerClientId { get; }
        string IdentityServerClientSecret { get; }
        int IdentityServerRemoteTimeoutSeconds { get; }
        string SesReceivingEmailDomain { get; }
        bool EmailEnabled { get; }
        string EmailOverride { get; }
        string EmailFromName { get; }
        string EmailFromAddress { get; }
        string EmailAppOutbox { get; }
        bool ExceptionlessEnabled { get; }
        string ExceptionlessApiKey { get; }
        string WorkerGroup { get; }
        string GetS3FullUrlPath(string bucket);
        int DisplayRowCountLimit { get; }
        string AwsAccountId { get; }
        string ExceptionlessWorkerApiKey { get; }
        string CatUIUrl { get; }
        string CatUIOnHoIdUrl { get; }
        string IpStackKey { get; }
        string IpStackURL { get; }
        string ResetPassword { get; }
        

        // new ones
        string SecretKey { get; }

        // Google Map
        string GetGoogleMapBaseUrl();
        string GetGoogleMapGeoCodeUrl();
        string GetGoogleMapApiBrowserKey();
        string GetGoogleMapApiServerKey();
        string GetGoogleMapApiClientId();
        string GetGoogleMapApiPrivateKey();
        string GetGooglePlacesKey();
    }
}
