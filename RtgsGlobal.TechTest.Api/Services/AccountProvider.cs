using RtgsGlobal.TechTest.Api.Models;
using RtgsGlobal.TechTest.Api.Models.DTOs;

namespace RtgsGlobal.TechTest.Api.Services;

/// <summary>
/// Provides the functions to interact with an Account
/// </summary>
public class AccountProvider : IAccountProvider
{
	private readonly IDictionary<string, AccountBalance> _accounts;

	public AccountProvider()
	{
		_accounts = new Dictionary<string, AccountBalance> { { "account-a", new AccountBalance() }, { "account-b", new AccountBalance() } };
	}

	public AccountBalance GetBalance(string accountIdentifier)
	{
		return _accounts[accountIdentifier];
	}

	public void Deposit(string accountIdentifier, decimal amount)
	{
		if(amount < 0)
		{
			throw new ArgumentOutOfRangeException();
		}

		AddTransaction(accountIdentifier, amount);
	}

	public void Transfer(LoanTransferDto transfer)
	{
		if(transfer.CreditorAccountIdentifier == transfer.DebtorAccountIdentifier)
		{
			throw new ArgumentException(nameof(transfer));
		}

		AddTransaction(transfer.DebtorAccountIdentifier, -transfer.Amount);
		AddTransaction(transfer.CreditorAccountIdentifier, transfer.Amount);
	}

	public void Withdraw(string accountIdentifier, decimal amount)
	{
		var currentBalance = GetBalance(accountIdentifier);

		if(currentBalance.Balance < amount)
		{
			throw new ArgumentException(nameof(amount));
		}

		AddTransaction(accountIdentifier, -1 * amount);
	}

	private void AddTransaction(string accountIdentifier, decimal amount)
	{
		AccountBalance accountBalance = _accounts[accountIdentifier];
		_accounts[accountIdentifier] =
			accountBalance with { Balance = accountBalance.Balance + amount };
	}
}
