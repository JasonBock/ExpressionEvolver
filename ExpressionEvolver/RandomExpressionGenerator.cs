using System;
using System.Globalization;
using System.Linq.Expressions;
using Spackle;
using Spackle.Extensions;

namespace ExpressionEvolver
{
	public enum Operation
	{
		Add,
		Subtract,
		Multiply,
		Divide,
		Power
	}

	public sealed class RandomExpressionGenerator
	{
		public RandomExpressionGenerator(int maximumOperationCount, double injectConstantProbabilityValue,
			double constantLimit, ParameterExpression parameter, SecureRandom random)
			: base()
		{
			parameter.CheckParameterForNull("parameter");
			random.CheckParameterForNull("random");

			this.InjectConstantProbabilityValue = injectConstantProbabilityValue;
			this.ConstantLimit = constantLimit;
			this.Body = parameter;
			this.Parameter = parameter;
			this.Random = random;
			this.GenerateBody(maximumOperationCount);
		}

		private void GenerateBody(int maximumOperationCount)
		{
			for (var i = 0; i < maximumOperationCount; i++)
			{
				this.GetRandomOperation(
					(Operation)this.Random.Next((int)Operation.Power + 1));
			}
		}

		private ConstantExpression GetConstant()
		{
			var value = this.Random.NextDouble() * this.ConstantLimit;
			var constant = value * (this.Random.NextBoolean() ? 1d : -1d);
			return Expression.Constant(constant);
		}

		private void GetRandomOperation(Operation @operator)
		{
			var isLeftConstant = this.Random.NextDouble() < this.InjectConstantProbabilityValue;
			var isRightConstant = this.Random.NextDouble() < this.InjectConstantProbabilityValue;
			var isLeftBody = true;
			var isRightBody = true;

			if (!isLeftConstant && !isRightConstant)
			{
				isLeftBody = this.Random.NextDouble() < 0.5;
				isRightBody = !isLeftBody;
			}
			else if (isLeftConstant && isRightConstant)
			{
				isLeftConstant = this.Random.NextDouble() < this.InjectConstantProbabilityValue;
				isRightConstant = !isLeftConstant;
			}
			else if (@operator == Operation.Divide && !isLeftConstant && !isRightConstant)
			{
				isLeftConstant = this.Random.NextBoolean();
				isRightConstant = !isLeftConstant;
			}

			this.Body = RandomExpressionGenerator.GetExpressionFunction(@operator)(
				isLeftConstant ? this.GetConstant() : (isLeftBody ? this.Body : this.Parameter),
				isRightConstant ? this.GetConstant() : (isRightBody ? this.Body : this.Parameter));
		}

		private static Func<Expression, Expression, Expression> GetExpressionFunction(Operation @operator)
		{
			Func<Expression, Expression, Expression> selectedOperation = null;

			switch (@operator)
			{
				case Operation.Add:
					selectedOperation = Expression.Add;
					break;
				case Operation.Subtract:
					selectedOperation = Expression.Subtract;
					break;
				case Operation.Multiply:
					selectedOperation = Expression.Multiply;
					break;
				case Operation.Divide:
					selectedOperation = Expression.Divide;
					break;
				case Operation.Power:
					selectedOperation = Expression.Power;
					break;
				default:
					throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture,
						"Unexpected operator type: {0}", @operator));
			}
			return selectedOperation;
		}

		public Expression Body { get; private set; }
		private double ConstantLimit { get; set; }
		private double InjectConstantProbabilityValue { get; set; }
		public Expression Parameter { get; private set; }
		private SecureRandom Random { get; set; }
	}
}
