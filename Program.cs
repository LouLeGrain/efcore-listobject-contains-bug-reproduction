using System.ComponentModel.DataAnnotations;
using System.Net;
using Microsoft.EntityFrameworkCore;

// Creating DbContext
using var dbContext = new TestDbContext();

dbContext.Database.EnsureDeleted();
dbContext.Database.EnsureCreated();

// Filling the database
MyRelationClass relation1 = new MyRelationClass { Name = "Relation 1" };
MyRelationClass relation2 = new MyRelationClass { Name = "Relation 2" };
MyRelationClass relation3 = new MyRelationClass { Name = "Relation 3" };

dbContext.MyRelationClasses.AddRange(relation1, relation2, relation3);

dbContext.MyClasses.Add(new MyClass { Name = "Main 1", MyRelationClass = relation1 });
dbContext.MyClasses.Add(new MyClass { Name = "Main 2", MyRelationClass = relation2 });

dbContext.SaveChanges();

// Then my test case here

List<object> objList = new List<object>() {
  new MyRelationClass { Id = 1, Name = "Relation 1" },
  new MyRelationClass { Id = 2, Name = "Relation 2" }
};

List<MyRelationClass> typedList = new List<MyRelationClass>() {
  new MyRelationClass { Id = 2, Name = "Relation 2" }
};


/*
 * This works fine
 */
List<MyClass> result = dbContext.MyClasses
  .Include(m => m.MyRelationClass)
  .Where(e => typedList.Contains(e.MyRelationClass))
  .ToList();

foreach (MyClass item in result)
{
  Console.WriteLine(item);
}


/*
 * This throws : 
 * Unhandled exception. System.InvalidOperationException: 
 * Translation of 'EF.Property<int?>((MyRelationClass)ProjectionBindingExpression: EmptyProjectionMember, "Id")' failed. 
 * Either the query source is not an entity type, or the specified property does not exist on the entity type.
 */
try {

  List<MyClass> fail = dbContext.MyClasses
    .Include(m => m.MyRelationClass)
    .Where(e => objList.Contains(e.MyRelationClass))
    .ToList();

} catch (System.InvalidOperationException ex) {
  Console.WriteLine(ex);
  dbContext.Database.EnsureDeleted();
}


class TestDbContext : DbContext
{
  public DbSet<MyClass> MyClasses => Set<MyClass>();
  public DbSet<MyRelationClass> MyRelationClasses => Set<MyRelationClass>();

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    optionsBuilder.UseSqlite("DataSource=test.db");
  }
}