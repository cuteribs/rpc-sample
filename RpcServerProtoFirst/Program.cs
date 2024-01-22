using Microsoft.AspNetCore.Http.Json;
using RpcServer.Contracts;
using RpcServer.Services;

namespace RpcServer;

public class Program
{
	public static void Main(string[] args)
	{
		StartWebServer(args);
	}

	static void StartWebServer(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		var services = builder.Services;

		// enable code first gRPC services
		services.AddGrpc();

		// optional - enable service reflection
		services.AddGrpcReflection();

		services.AddScoped<Echo.EchoBase, EchoService>();
		services.AddScoped<Hello.HelloBase, HelloService>();

		services.Configure<JsonOptions>(o => o.SerializerOptions.PropertyNamingPolicy = null);
		services.AddEndpointsApiExplorer()
			.AddSwaggerGen();

		var app = builder.Build();

		app.UseSwagger()
			.UseSwaggerUI();

		// map gRPC services
		app.MapGrpcService<EchoService>();
		app.MapGrpcService<HelloService>();


		// optional - map gRPC service reflection
		app.MapGrpcReflectionService();

		// map service contracts as Web API endpoints (auto-generated with T4 text template)
		app.MapPostServiceContracts();

		app.MapGet("/", () => "Hello World!");

		app.Run();
	}
}
