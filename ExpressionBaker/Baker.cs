using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionBaker
{
	public sealed class Baker<TDelegate>
	{
		public Baker(string expression)
		{
			if (string.IsNullOrWhiteSpace(expression))
			{
				throw new ArgumentException("An expressions must be given.", nameof(expression));
			}

			this.Expression = expression;
		}

		public Expression<TDelegate> Bake()
		{
			var name = "ExpressionHolder";
			var code = 
$@"using System;
using System.Linq.Expressions;
public static class {name}
{{
	public static readonly Expression<{this.GetDelegateType()}> func = {this.Expression};
}}";
			var tree = SyntaxFactory.ParseCompilationUnit(code).SyntaxTree;

			var options = new CSharpCompilationOptions(
				outputKind: OutputKind.DynamicallyLinkedLibrary,
				optimizationLevel: OptimizationLevel.Release);
			var compilation = CSharpCompilation.Create($"{Guid.NewGuid().ToString("N")}.dll",
				options: options,
				syntaxTrees: new[] { tree },
				references: new[]
				{
					MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
					MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location),
					MetadataReference.CreateFromFile(typeof(Expression).Assembly.Location),
				});
			var diagnostics = compilation.GetDiagnostics();

			using (var assemblyStream = new MemoryStream())
			{
				var results = compilation.Emit(assemblyStream);
				var assembly = Assembly.Load(assemblyStream.ToArray());
				return assembly.GetType(name)
					.GetField("func").GetValue(null) as Expression<TDelegate>;
			}
		}

		private string GetDelegateType()
		{
			var type = typeof(TDelegate);
			var name = type.Name.Split('`')[0];
			return $"{name}<{string.Join(",", (from argument in type.GetGenericArguments() select argument.Name))}>";
		}

		public string Expression { get; }
	}
}