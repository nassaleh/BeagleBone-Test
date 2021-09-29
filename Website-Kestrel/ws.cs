using System;
using System.IO;
using System.Text;
using BeagleBone;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Internal;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ThreadUtils;

namespace Web
{
    public class Startup
    {
        public static string GetTable()
        {
            var sb = new StringBuilder();

            sb.Append(@"<html>
                        <head>
                        <style>
                        table, th, td {
                        border: 1px solid black;
                        border-collapse: collapse;
                        }
                        </style>
                        </head>
                        <body>
                        <table class=""table"">");

            DBHelper db = new DBHelper();

            foreach (var pin in db.GetRecords())
            {
                sb.AppendLine($"<tr>" +
                    $"<td>ID: {pin.Id}</td>" +
                    $"<td>Pin: {pin.Gpio}</td>" +
                    $"<td>Value: {pin.PinValue}</td>" +
                    $"<td>Time: {pin.Timestamp}</td>" +
                    $"</tr>");
            }
            sb.Append(@"</table>
                        </body>
                        </html>");
            return sb.ToString();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseEndpointRouting();
            app.UseEndpoint();

            app.UseMvc(endpoints =>
            {
                endpoints.MapGet("/home/evt", async context =>
                {
                    await context.Response.WriteAsync(GetTable());
                });
            });

            app.Run(context => context.Response.WriteAsync($"Hello World{Environment.NewLine}" +
                  $".NET Core {Environment.Version}{Environment.NewLine}" +
                  $"Environment.OSVersion: {Environment.OSVersion}{Environment.NewLine}" +
                  $"Environment.Is64BitOperatingSystem: {Environment.Is64BitOperatingSystem}{Environment.NewLine}" +
                  $"Environment.Is64BitProcess: {Environment.Is64BitProcess}", Encoding.UTF8));
        }
    }

    public class wsWorker : Worker, IDisposable
    {
        private IWebHost host;
        public override void DoWork()
        {
            host = new WebHostBuilder()
           .UseKestrel()
           //.UseUrls("http://192.168.1.7:8069", "http://localhost:8069")
           .UseUrls("http://192.168.1.111:8069", "http://localhost:8069")
           .UseContentRoot(Directory.GetCurrentDirectory())
           .UseStartup<Startup>()
           .Build();

            host.Run();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && host != null)
            {
                host.Dispose();
                host = null;
            }
        }

        ~wsWorker() => Dispose(false);
    }
}