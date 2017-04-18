using System;
using Microsoft.EntityFrameworkCore;

namespace StockExchange.Models
{
	public class StockContext : DbContext
	{
		public StockContext(DbContextOptions<StockContext> options)
			: base(options)
		{ }

		public DbSet<Stock> Stocks { get; set; }
	}

	public class Stock
	{
		public Stock()
		{
		}
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string ShortName { get; set; }
		public double Rate { get; set; }
	}
}
