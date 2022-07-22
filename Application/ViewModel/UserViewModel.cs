namespace PCP.Application.ViewModel;

public class UserViewModel
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public long RoleId { get; set; }
    public string KeycloakId { get; set; } = null!;
    public string Email { get; set; } = null!;
    public int SubsribeToEmail { get; set; }
}