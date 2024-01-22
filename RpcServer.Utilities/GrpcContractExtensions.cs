using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace RpcServer.Utilities;

public static class GrpcContractExtensions
{
	private static readonly string ProtoTypeEmpty = ".google.protobuf.Empty";

	public static bool IsIn<T>(this T source, params T[] values)
		=> values.Contains(source);

	public static Type GetArgumentType(this Type source)
	{
		var type = source;

		if (source.IsIn(typeof(Task), typeof(ValueTask)))
		{
			type = typeof(void);
		}
		else if (source.IsGenericType && source.GetGenericTypeDefinition().IsIn(typeof(Task<>), typeof(ValueTask<>)))
		{
			type = source.GenericTypeArguments.FirstOrDefault();
		}

		return type;
	}

	public static string GetProtoType(this Type type)
	{
		var isRepeated = false;
		var argumentType = type.GetArgumentType();

		if (argumentType.IsGenericType && argumentType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
		{
			isRepeated = true;
			argumentType = argumentType.GenericTypeArguments.First();
		}

		var typeName = argumentType.Name;

		var protoType = typeName switch
		{
			"Int32" => "int32",
			"Int64" => "int64",
			"UInt32" => "int32",
			"UInt64" => "int64",
			"String" => "string",
			"Boolean" => "bool",
			"Single" => "float",
			"Double" => "double",
			"Decimal" => "decimal",
			"DateTime" => ".google.protobuf.Timestamp",
			"Byte[]" => "bytes",
			"Guid" => "string",
			"Void" => ProtoTypeEmpty,
			_ => typeName
		};

		return $"{(isRepeated ? "repeated " : string.Empty)}{protoType}";
	}

	public static IEnumerable<ProtoMessage> CreateProtoMessages(this Assembly assembly)
	{
		var messages = assembly.GetTypes()
			.Where(t => Attribute.IsDefined(t, typeof(DataContractAttribute)));

		return messages.Select(c => new ProtoMessage
		{
			Name = c.Name,
			IsEnum = c.IsEnum,
			Members = (c.IsEnum
				? c.GetEnumValues()?
					.Cast<object>()
					.Select(v => new ProtoField
					{
						Name = Enum.GetName(c, v),
						Type = string.Empty,
						Tag = Convert.ToInt32(v)
					})
				: c.GetProperties()?
					.Where(p => Attribute.IsDefined(p, typeof(DataMemberAttribute)))
					.Select(p =>
					{
						var attribute = p.GetCustomAttribute(typeof(DataMemberAttribute));
						var tag = attribute?.GetType()
							.GetProperty("Order")
							.GetValue(attribute);
						return new ProtoField
						{
							Name = p.Name,
							Type = p.PropertyType.GetProtoType(),
							Tag = Convert.ToInt32(tag)
						};
					})
				) ?? Enumerable.Empty<ProtoField>()
		});
	}

	public static IEnumerable<ProtoService> CreateProtoServices(this Assembly assembly, string baseInterface)
	{
		var services = assembly.GetTypes()
			.Where(t => t.IsInterface && t.GetInterface(baseInterface) != null);
		return services.Select(s => new ProtoService
		{
			TypeName = s.Name,
			Name = s.GetCustomAttribute<ServiceContractAttribute>()?.Name ?? s.Name,
			Operations = s.GetMethods(BindingFlags.Public | BindingFlags.Instance)
				.Where(m => m.GetCustomAttribute<OperationContractAttribute>() != null)
				.Select(o =>
				{
					var request = o.GetParameters().FirstOrDefault()?.ParameterType;

					if (request?.GetCustomAttribute(typeof(DataContractAttribute)) == null)
					{
						request = null;
					}

					return new ProtoOperation
					{
						Name = o.Name,
						Request = request?.Name ?? "Empty",
						Response = o.ReturnType?.Name,
						ProtoRequest = request?.GetProtoType() ?? ProtoTypeEmpty,
						ProtoResponse = o.ReturnType?.GetProtoType() ?? ProtoTypeEmpty
					};
				})
		});
	}
}

public class ProtoService
{
	public string TypeName { get; set; } = default!;
	public string Name { get; set; } = default!;
	public IEnumerable<ProtoOperation> Operations { get; set; } = default!;
}

public class ProtoOperation
{
	public string Name { get; set; } = default!;
	public string? Request { get; set; } = default!;
	public string? Response { get; set; } = default!;
	public string ProtoRequest { get; set; } = default!;
	public string ProtoResponse { get; set; } = default!;
}

public class ProtoMessage
{
	public string Name { get; set; } = default!;
	public bool IsEnum { get; set; }
	public IEnumerable<ProtoField> Members { get; set; } = default!;
}

public class ProtoField
{
	public string Name { get; set; } = default!;
	public string Type { get; set; } = string.Empty;
	public int Tag { get; set; }
}