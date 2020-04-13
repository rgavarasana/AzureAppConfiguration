using System;
using Azure.Core;
using Azure.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.Hosting;

namespace Ravi.Learn.Azure.AppConfiguration
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureAppConfiguration((builderContext, configBuilder) => {
                        
                        var configRoot = configBuilder.Build();
                        var isDevelopment = builderContext.HostingEnvironment.IsDevelopment();
                        //configBuilder.AddAzureAppConfiguration(configRoot["ConnectionStrings:AppConfig"]);
                        configBuilder.AddAzureAppConfiguration(options =>
                        {

                            //options.Connect(new Uri(configRoot["ConnectionStrings:AppConfig"]), new DefaultAzureCredential())
                            //    .ConfigureKeyVault(kv =>
                            //    {
                            //        kv.SetCredential(new DefaultAzureCredential());
                            //    });

                            //var defaultAzureCredentialOptions = new DefaultAzureCredentialOptions();


                            //defaultAzureCredentialOptions.ExcludeEnvironmentCredential = true;
                            // defaultAzureCredentialOptions.ExcludeManagedIdentityCredential = true;
                            // defaultAzureCredentialOptions.SharedTokenCacheTenantId = "f5dfe06f-ce39-486a-9341-534321ac9eed";// "f1e27560 -725a-4134-a985-0a66fe1b80ab";
                            // defaultAzureCredentialOptions.SharedTokenCacheUsername = @"live.com#gavarasana@outlook.com";
                            //defaultAzureCredentialOptions.SharedTokenCacheUsername = "RGavarasana@evolenthealth.com";

                            options.Connect(new Uri(configRoot["ConnectionStrings:AppConfig"]),
                                    isDevelopment ? new DefaultAzureCredential() : (TokenCredential)new ManagedIdentityCredential())
                                    .ConfigureKeyVault(kvOptions =>
                                    {
                                        kvOptions.SetCredential(isDevelopment ? new DefaultAzureCredential() : (TokenCredential)new ManagedIdentityCredential());
                                    });
                        });
                        var kvOptions = new AzureKeyVaultConfigurationOptions(configRoot["ConnectionStrings:KeyVaultConfig"])
                        {
                            ReloadInterval = new TimeSpan(0, 15, 0)
                        };
                        configBuilder.AddAzureKeyVault(kvOptions);


                    });
                    
                    webBuilder.UseStartup<Startup>();
                });
    }
}
