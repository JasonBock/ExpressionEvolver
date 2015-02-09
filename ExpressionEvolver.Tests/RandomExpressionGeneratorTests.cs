using System;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Spackle;

namespace ExpressionEvolver.Tests
{
	[TestClass]
	public sealed class RandomExpressionGeneratorTests
	{
		[TestMethod, ExpectedException(typeof(NotSupportedException))]
		public void CreateForUnsupportedOperation()
		{
			RandomExpressionGeneratorTests.TestGenerator(_ =>
			{
				_.Next((int)Operation.Power + 1).Returns(5);
			}, null);
		}

		private static void TestGenerator(Action<SecureRandom> setupRandom, string body)
		{
			const int maximumOperationCount = 1;
			const double injectConstantProbabilityValue = 0.5;
			var parameter = Expression.Parameter(typeof(double), "a");

			var random = Substitute.For<SecureRandom>();
			setupRandom(random);

			var generator = new RandomExpressionGenerator(
				maximumOperationCount, injectConstantProbabilityValue,
				100d, parameter, random);

			Assert.AreEqual(body, generator.Body.ToString());
		}

		[TestMethod]
		public void CreateExpressionWhenBothSidesAreConstantsNegativeConstantAndParameter()
		{
			RandomExpressionGeneratorTests.TestGenerator(_ =>
			{
				_.NextBoolean().Returns(false);
				_.NextDouble().Returns(0.3, 0.3, 0.3, 0.3);
			}, "(-30 + a)");
		}

		[TestMethod]
		public void CreateAddOfNegativeConstantAndParameter()
		{
			RandomExpressionGeneratorTests.TestGenerator(_ =>
			{
				_.NextBoolean().Returns(false);
				_.NextDouble().Returns(0.3, 0.7, 0.3);
			}, "(-30 + a)");
		}

		[TestMethod]
		public void CreateAddOfPositiveConstantAndParameter()
		{
			RandomExpressionGeneratorTests.TestGenerator(_ =>
			{
				_.NextBoolean().Returns(true);
				_.NextDouble().Returns(0.3, 0.7, 0.3);
			}, "(30 + a)");
		}

		[TestMethod]
		public void CreateAddOfParameterAndNegativeConstant()
		{
			RandomExpressionGeneratorTests.TestGenerator(_ =>
			{
				_.NextBoolean().Returns(false);
				_.NextDouble().Returns(0.7, 0.3, 0.3);
			}, "(a + -30)");
		}

		[TestMethod]
		public void CreateAddOfParameterAndPositiveConstant()
		{
			RandomExpressionGeneratorTests.TestGenerator(_ =>
			{
				_.NextBoolean().Returns(true);
				_.NextDouble().Returns(0.7, 0.3, 0.3);
			}, "(a + 30)");
		}

		[TestMethod]
		public void CreateSubtractOfNegativeConstantAndParameter()
		{
			RandomExpressionGeneratorTests.TestGenerator(_ =>
			{
				_.Next((int)Operation.Power + 1).Returns(1);
				_.NextBoolean().Returns(false);
				_.NextDouble().Returns(0.3, 0.7, 0.3);
			}, "(-30 - a)");
		}

		[TestMethod]
		public void CreateSubtractOfPositiveConstantAndParameter()
		{
			RandomExpressionGeneratorTests.TestGenerator(_ =>
			{
				_.Next((int)Operation.Power + 1).Returns(1);
				_.NextBoolean().Returns(true);
				_.NextDouble().Returns(0.3, 0.7, 0.3);
			}, "(30 - a)");
		}

		[TestMethod]
		public void CreateSubtractOfParameterAndNegativeConstant()
		{
			RandomExpressionGeneratorTests.TestGenerator(_ =>
			{
				_.Next((int)Operation.Power + 1).Returns(1);
				_.NextBoolean().Returns(false);
				_.NextDouble().Returns(0.7, 0.3, 0.3);
			}, "(a - -30)");
		}

		[TestMethod]
		public void CreateSubtractOfParameterAndPositiveConstant()
		{
			RandomExpressionGeneratorTests.TestGenerator(_ =>
			{
				_.Next((int)Operation.Power + 1).Returns(1);
				_.NextBoolean().Returns(true);
				_.NextDouble().Returns(0.7, 0.3, 0.3);
			}, "(a - 30)");
		}

		[TestMethod]
		public void CreateMultiplyOfNegativeConstantAndParameter()
		{
			RandomExpressionGeneratorTests.TestGenerator(_ =>
			{
				_.Next((int)Operation.Power + 1).Returns(2);
				_.NextBoolean().Returns(false);
				_.NextDouble().Returns(0.3, 0.7, 0.3);
			}, "(-30 * a)");
		}

		[TestMethod]
		public void CreateMultiplyOfPositiveConstantAndParameter()
		{
			RandomExpressionGeneratorTests.TestGenerator(_ =>
			{
				_.Next((int)Operation.Power + 1).Returns(2);
				_.NextBoolean().Returns(true);
				_.NextDouble().Returns(0.3, 0.7, 0.3);
			}, "(30 * a)");
		}

		[TestMethod]
		public void CreateMultiplyOfParameterAndNegativeConstant()
		{
			RandomExpressionGeneratorTests.TestGenerator(_ =>
			{
				_.Next((int)Operation.Power + 1).Returns(2);
				_.NextBoolean().Returns(false);
				_.NextDouble().Returns(0.7, 0.3, 0.3);
			}, "(a * -30)");
		}

		[TestMethod]
		public void CreateMultiplyOfParameterAndPositiveConstant()
		{
			RandomExpressionGeneratorTests.TestGenerator(_ =>
			{
				_.Next((int)Operation.Power + 1).Returns(2);
				_.NextBoolean().Returns(true);
				_.NextDouble().Returns(0.7, 0.3, 0.3);
			}, "(a * 30)");
		}

		[TestMethod]
		public void CreateDivideOfNegativeConstantAndParameter()
		{
			RandomExpressionGeneratorTests.TestGenerator(_ =>
			{
				_.Next((int)Operation.Power + 1).Returns(3);
				_.NextBoolean().Returns(false);
				_.NextDouble().Returns(0.3, 0.7, 0.3);
			}, "(-30 / a)");
		}

		[TestMethod]
		public void CreateDivideOfPositiveConstantAndParameter()
		{
			RandomExpressionGeneratorTests.TestGenerator(_ =>
			{
				_.Next((int)Operation.Power + 1).Returns(3);
				_.NextBoolean().Returns(true);
				_.NextDouble().Returns(0.3, 0.7, 0.3);
			}, "(30 / a)");
		}

		[TestMethod]
		public void CreateDivideOfParameterAndNegativeConstant()
		{
			RandomExpressionGeneratorTests.TestGenerator(_ =>
			{
				_.Next((int)Operation.Power + 1).Returns(3);
				_.NextBoolean().Returns(false);
				_.NextDouble().Returns(0.7, 0.3, 0.3);
			}, "(a / -30)");
		}

		[TestMethod]
		public void CreateDivideOfParameterAndPositiveConstant()
		{
			RandomExpressionGeneratorTests.TestGenerator(_ =>
			{
				_.Next((int)Operation.Power + 1).Returns(3);
				_.NextBoolean().Returns(true);
				_.NextDouble().Returns(0.7, 0.3, 0.3);
			}, "(a / 30)");
		}

		[TestMethod]
		public void CreateSquareRootOfParameter()
		{
			RandomExpressionGeneratorTests.TestGenerator(_ =>
			{
				_.Next((int)Operation.Power + 1).Returns(4);
				_.NextBoolean().Returns(true);
				_.NextDouble().Returns(0.7, 0.3, 0.005);
			}, "(a ^ 0.5)");
		}

		[TestMethod]
		public void CreateParameterOnlyBody()
		{
			const int maximumOperationCount = 0;
			const double injectConstantProbabilityValue = 0d;
			var parameter = Expression.Parameter(typeof(double), "a");
			using (var random = new SecureRandom())
			{
				var expressionGenerator = new RandomExpressionGenerator(
					maximumOperationCount, injectConstantProbabilityValue,
					100d, parameter, random);

				Assert.AreEqual("a", expressionGenerator.Body.ToString());
			}
		}

		[TestMethod, ExpectedException(typeof(ArgumentNullException))]
		public void CreateWithNullParameter()
		{
			using (var random = new SecureRandom())
			{
				new RandomExpressionGenerator(1, 0.5, 100d,
					null, random);
			}
		}

		[TestMethod, ExpectedException(typeof(ArgumentNullException))]
		public void CreateWithNullRandom()
		{
			new RandomExpressionGenerator(1, 0.5, 100d,
				Expression.Parameter(typeof(double), "a"), null);
		}
	}
}
