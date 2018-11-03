﻿using System.Linq.Expressions;
using DapperExtensions;

namespace Dapper.Storage.Linq
{
	public class BinaryToken : Token
	{
		public new BinaryExpression Expression =>
			(BinaryExpression)base.Expression;

		public override string Name =>
			Token.Create(Expression.Left).Name;

		public override object Value =>
			Token.Create(Expression.Right).Value;

		private IPredicate CreateFieldPredicate(IToken left, IToken right) => null;

		public BinaryToken(BinaryExpression expression)
			:base(expression)
		{
		}
	}
}