using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProtobufDemo.Data.Api;
using ProtobufDemo.Data.EF;
using ProtobufDemo.Data.EF.Manager;
using ProtobufDemo.Manager;
using System;
using System.IO.Compression;
using System.Linq;
using WebApiContrib.Core.Formatter.Protobuf;

namespace ProtobufDemo.Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            TypeModelHelper.AddTypeModels(ProtobufOutputFormatter.Model);

            services
                .AddScoped(_ => new DemoContext(Configuration.GetConnectionString("EntityFramework")))
                .AddScoped(_ => new Func<DemoContext>(() => _.GetService<DemoContext>()))
                .AddScoped<IOrderManager, OrderManager>()
                .AddResponseCompression(options =>
                {
                    options.Providers.Add<GzipCompressionProvider>();
                    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/x-protobuf" });
                })
                .AddMvc(options => options.OutputFormatters.Add(new ProtobufOutputFormatter(new ProtobufFormatterOptions())))
                .AddJsonOptions(options => options.SerializerSettings.TypeNameHandling = TypeNameHandling.All);

            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Optimal;
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            app.UseResponseCompression();
            app.UseMvc();
        }
    }
}
