using StreamingHub.Providers;
using StreamingHub.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace StreamingHub
{
	public class Startup
	{
		const string corsPolicy = "_corsPolicy";

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddGrpc();

			services.AddTransient<IStreamingSessionService, StreamingSessionService>();
			services.AddSingleton<IStreamingSessionProvider, StreamingSessionProvider>();

			services.AddCors(options =>
			{
				options.AddPolicy(name: corsPolicy,
								  policy =>
								  {
									  policy.AllowAnyOrigin()
											.AllowAnyHeader()
											.AllowAnyMethod();
								  });
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseRouting();
			app.UseCors(corsPolicy);

			app.UseGrpcWeb();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapGrpcService<StreamingService>().EnableGrpcWeb();
				endpoints.MapGet("/",
					async context =>
					{
						await context.Response.WriteAsync(
							"Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
					});
			});
		}
	}
}