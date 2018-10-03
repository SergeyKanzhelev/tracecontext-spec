using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenCensus.Collector.AspNetCore;
using OpenCensus.Collector.Dependencies;
using OpenCensus.Trace;
using OpenCensus.Trace.Propagation;
using OpenCensus.Trace.Sampler;
using System.Net.Http;

namespace TestApp.AspNetCore._2._0
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
            services.AddMvc();
            services.AddSingleton<HttpClient>();

            services.AddSingleton<ITracer>(Tracing.Tracer);
            services.AddSingleton<ISampler>(Samplers.AlwaysSample);
            services.AddSingleton<RequestsCollectorOptions>(new RequestsCollectorOptions());
            services.AddSingleton<RequestsCollector>();
            services.AddSingleton<DependenciesCollectorOptions>(new DependenciesCollectorOptions());
            services.AddSingleton<DependenciesCollector>();
            services.AddSingleton<IPropagationComponent>(new DefaultPropagationComponent());

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            var collector = app.ApplicationServices.GetService<RequestsCollector>();
            var depCollector = app.ApplicationServices.GetService<DependenciesCollector>();
        }
    }
}
