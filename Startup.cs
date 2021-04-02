using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.RegularExpressions;
using System.IO;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace endpoints
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });
        }
        public string GetPlural(int count, string form1, string form2, string form3)
        {
            if((count == 1 || count % 10 == 1) && count != 11)
            {
                return form1;
            }
            else if ((count % 10 == 2 ||count % 10 == 3 ||count % 10 == 4) && (count > 14 || count < 11))
            {
                return form2;
            }
            else
            {
                return form3;
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
                endpoints.MapGet("/headers", async context =>
                {
                    foreach(var header in context.Request.Headers)
                    {
                        await context.Response.WriteAsync(header.ToString()+"\n");
                    }
                });
                endpoints.MapGet("/plural", async context =>
                {
                    string[] forms = context.Request.Query["forms"].ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    int num = Convert.ToInt32(context.Request.Query["number"]); 
                    string word = GetPlural(num ,forms[0],forms[1],forms[2]);
                    await context.Response.WriteAsync(num + " " + word + "\n");
                });

                endpoints.MapPost("/frequency", async context =>
                {
                    StreamReader reader = new StreamReader(context.Request.Body);
                    // await context.Response.WriteAsync(header.ToString()+"\n");
                    TextFrequensy text = new TextFrequensy(reader.ReadToEnd());
                    context.Response.Headers.Add("Content-Type","application/json");
                    context.Response.Headers.Add("Number-Of-Uniqu-Uords" , text.NumberOfUniquUords());
                    context.Response.Headers.Add("The-Most-Common-Word" , text.TheMostCommonWord());
                    await context.Response.WriteAsJsonAsync(text.GetFrequensy());
                });
            });

        }
    }
}
