using System;

namespace NetCoreTemplate.Api.Users.Dtos
{
  public class UserDto
  {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Lastname { get; set; }
    public int Age { get; set; }
  }
}