namespace ChatBot.DTO;

public class Customer
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    
    // Конфиденциальные данные
    public string Password { get; set; }
    public string CardNumber { get; set; }
    
    // Системные поля
    public string Role { get; set; }
    public bool IsBlocked { get; set; }
    public DateTime CreatedAt { get; set; }
}