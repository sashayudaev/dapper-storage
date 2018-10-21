﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using Dapper.Storage.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Dapper.Storage.Tests
{
	[TestClass]
	public class StorageResourceTests
	{
		[TestMethod]
		public void Query_WithoutTransaction_ShouldNotHaveTransaction()
		{
			var mock = new Mock<ILifetimeScope>();
			mock.Setup(lf => lf.BeginLifetimeScope())
				.Returns(mock.Object);

			var context = mock.Object;
			var resource = new StorageResource(context);

			Assert.IsFalse(resource.HasTransaction);
		}

		[TestMethod]
		public void Query_InTransaction_ShouldHaveTransaction()
		{
			var mock = new Mock<ILifetimeScope>();
			mock.Setup(lf => lf.BeginLifetimeScope())
				.Returns(mock.Object);

			var context = mock.Object;
			var resource = new StorageResource(context);

			bool hasTransaction;
			using (var transaction = resource.Begin())
			{
				hasTransaction = resource.HasTransaction;
			}

			Assert.IsTrue(hasTransaction);
		}

		[TestMethod]
		public void MultipleTransaction_EndTransaction_NoTransactionLevel()
		{
			var mock = new Mock<ILifetimeScope>();
			mock.Setup(lf => lf.BeginLifetimeScope())
				.Returns(mock.Object);

			var context = mock.Object;
			var resource = new StorageResource(context);

			var first = resource.Begin();
			var second = resource.Begin();
			var third = resource.Begin();

			var threeLevels = resource.TransactionLevel;
			third.Dispose();
			var twoLevels = resource.TransactionLevel;
			second.Dispose();
			var oneLevel = resource.TransactionLevel;
			first.Dispose();
			var noLevels = resource.TransactionLevel;

			Assert.AreEqual(threeLevels, 3);
			Assert.AreEqual(twoLevels, 2);
			Assert.AreEqual(oneLevel, 1);
			Assert.AreEqual(noLevels, 0);
		}
	}
}