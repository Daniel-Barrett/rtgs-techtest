using RtgsGlobal.TechTest.Api.Controllers;

namespace RtgsGlobal.TechTest.Api;

public interface IAccountProvider
{
	MyBalance GetBalance(string accountIdentifier);
	void Deposit(string accountIdentifier, float amount);
	void Transfer(MyTransferDto transfer);
	void Withdraw(string accountIdentifier, float amount);
}

public class AccountProvider : IAccountProvider
{
	private readonly IDictionary<string, MyBalance> _accounts;

	public AccountProvider()
	{
		_accounts = new Dictionary<string, MyBalance> { { "account-a", new MyBalance() }, { "account-b", new MyBalance() } };
	}

	public MyBalance GetBalance(string accountIdentifier) => _accounts[accountIdentifier];

	public void Deposit(string accountIdentifier, float amount) => AddTransaction(accountIdentifier, amount);

	public void Transfer(MyTransferDto transfer)
	{
		AddTransaction(transfer.DebtorAccountIdentifier, -transfer.Amount);
		AddTransaction(transfer.CreditorAccountIdentifier, transfer.Amount);
	}

	public void Withdraw(string accountIdentifier, float amount) => AddTransaction(accountIdentifier, -1 * amount);

	private void AddTransaction(string accountIdentifier, float amount)
	{
		MyBalance accountBalance = _accounts[accountIdentifier];
		_accounts[accountIdentifier] =
			accountBalance with { Balance = accountBalance.Balance + amount };
	}
}

public record MyBalance(float Balance = 0);
