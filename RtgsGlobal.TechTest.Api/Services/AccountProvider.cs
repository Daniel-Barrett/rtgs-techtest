using RtgsGlobal.TechTest.Api.Models;
using RtgsGlobal.TechTest.Api.Models.DTOs;

namespace RtgsGlobal.TechTest.Api.Services;

public class AccountProvider : IAccountProvider
{
	private readonly IDictionary<string, AccountBalance> _accounts;

	public AccountProvider()
	{
		_accounts = new Dictionary<string, AccountBalance> { { "account-a", new AccountBalance() }, { "account-b", new AccountBalance() } };
	}

	public AccountBalance GetBalance(string accountIdentifier)
	{
		//ThrowIfAccountDoesNotExist(accountIdentifier);
		return _accounts[accountIdentifier];
	}

	public void Deposit(string accountIdentifier, decimal amount)
	{
		ThrowIfAccountDoesNotExist(accountIdentifier);
		AddTransaction(accountIdentifier, amount);
	}

	public void Transfer(LoanTransferDto transfer)
	{
		ThrowIfAccountDoesNotExist(transfer.DebtorAccountIdentifier);
		ThrowIfAccountDoesNotExist(transfer.CreditorAccountIdentifier);

		AddTransaction(transfer.DebtorAccountIdentifier, -transfer.Amount);
		AddTransaction(transfer.CreditorAccountIdentifier, transfer.Amount);
	}

	public void Withdraw(string accountIdentifier, decimal amount)
	{
		ThrowIfAccountDoesNotExist(accountIdentifier);
		AddTransaction(accountIdentifier, -1 * amount);
	}

	private void AddTransaction(string accountIdentifier, decimal amount)
	{
		AccountBalance accountBalance = _accounts[accountIdentifier];
		_accounts[accountIdentifier] =
			accountBalance with { Balance = accountBalance.Balance + amount };
	}

	private void ThrowIfAccountDoesNotExist(string accountIdentifier)
	{
		if (!_accounts.ContainsKey(accountIdentifier))
		{
			throw new KeyNotFoundException(nameof(accountIdentifier));
		}
	}
}
