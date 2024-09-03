namespace Backend.Models;

public class Image
{
    public int Id { get; set; }

    public string FileName{ get; set; }

    public byte[] Picture { get; set; }

    public List<User> Users { get; set; }
}