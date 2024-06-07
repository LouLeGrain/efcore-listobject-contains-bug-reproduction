# efcore-listobject-contains-bug-reproduction
This is a test project using EF Core 8.0.6 to demonstrate a bug that hapens when filtering a dbSet with `List<object>.Contains()`

It's the same problem related in [issue #20624](https://github.com/dotnet/efcore/issues/20624)

In a nutshell this is the problematic code : 
```csharp
List<object> objList = new List<object>() {
  new MyRelationClass { Id = 1, Name = "Relation 1" },
  new MyRelationClass { Id = 2, Name = "Relation 2" }
};

dbContext.MyClasses
    .Include(m => m.MyRelationClass)
    .Where(e => objList.Contains(e.MyRelationClass))
    .ToList();
/*
 Unhandled exception. System.InvalidOperationException: 
 Translation of 'EF.Property<int?>((MyRelationClass)ProjectionBindingExpression: EmptyProjectionMember, "Id")' failed. 
 Either the query source is not an entity type, or the specified property does not exist the entity type.
*/
```
