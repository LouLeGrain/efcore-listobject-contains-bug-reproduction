using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

class MyClass
{
  [Key]
  public int Id { get; set; }
  public string Name { get; set; } = String.Empty;

  public MyRelationClass MyRelationClass { get; set; } = new();
  public int MyRelationClassId { get; set; }

  public override string ToString()
  {
    return $"Id: {this.Id} - Name : {this.Name}";
  }

}
class MyRelationClass
{
  [Key]
  public int Id { get; set; }
  public string Name { get; set; } = String.Empty;

  public override string ToString()
  {
    return $"Id: {this.Id} - Name : {this.Name}";
  }
}