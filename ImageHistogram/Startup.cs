using ImageHistogram.Configuration;
using ImageHistogram.Database;
using ImageHistogram.Evaluation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImageHistogram
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<PictureDbOptions>(Configuration.GetSection(nameof(PictureDbOptions)));

            services.AddSingleton<DatabaseInitializer>();
            services.AddSingleton<ImageStorage>();
            services.AddSingleton((serviceProvider) => serviceProvider.GetRequiredService<DatabaseInitializer>().Create());
            services.AddSingleton<FullScanFind>();
            services.AddSingleton<Histogram>();
            services.AddSingleton<IHistogramComparer, KullbackLeiblerDivergence>();
            services.AddSingleton<IHistogramComparer, MinkowskiDistance>((sp) => new MinkowskiDistance(2));
            ////services.AddSingleton<IHistogramComparer, Qfd>();
            services.AddSingleton<SimilarityAggregator>();

            services.AddRazorPages();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
