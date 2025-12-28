namespace FinancialControl.API.Endpoints.Models;

public class WalletAssetCreateViewModel
{
    public Guid Wallet { get; set; }
    public Guid Asset { get; set; }
    public decimal Percentage { get; set; }
}
