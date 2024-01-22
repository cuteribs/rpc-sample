using Microsoft.AspNetCore.Mvc;
using Google.Protobuf.WellKnownTypes;
using RpcServer.Contracts;

namespace RpcServer;

public static class WebApplicationExtensions
{
	public static WebApplication MapPostServiceContracts(this WebApplication app)
	{
		app.MapPost(
			"/Echo/Ping",
			(
				[FromBody] PingRequest request,
				[FromServices] Echo.EchoBase service,
				HttpContext context
			) => service.Ping(request, context.ToCallContext())
		);

		app.MapPost(
			"/Echo/Hello",
			(
				[FromBody] Empty request,
				[FromServices] Echo.EchoBase service,
				HttpContext context
			) => service.Hello(request, context.ToCallContext())
		);

		app.MapPost(
			"/Hello/Hello",
			(
				[FromBody] HelloRequest request,
				[FromServices] Hello.HelloBase service,
				HttpContext context
			) => service.Hello(request, context.ToCallContext())
		);

		return app;
	}

	public static HttpServerCallContext ToCallContext(this HttpContext context)
		=> new(context);
}