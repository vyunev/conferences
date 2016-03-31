using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using Xunit;
using MyShuttle.Data;

namespace MyShuttle.IntegrationTests
{
  public class MyShuttleContextTests
  {
	[Fact]
	public async Task Db_CreatedSuccessfully()
	{
	  var optionsBuilder = new DbContextOptionsBuilder();
	  optionsBuilder.UseSqlServer(@"Data Source=.;Initial Catalog=MyShuttle;Integrated Security=True");

	  var context = new MyShuttleContext(optionsBuilder.Options);
	  var databaseDeleted = await context.Database.EnsureDeletedAsync();
	  Assert.True(databaseDeleted);

	  var databaseCreated = await context.Database.EnsureCreatedAsync();
	  Assert.True(databaseCreated);
	}
  }
}
