using RtgsGlobal.TechTest.Api.Models;
using RtgsGlobal.TechTest.Api.Models.DTOs;

namespace RtgsGlobal.TechTest.Api.Services;
public interface IAccountProvider
{
	void Deposit(string accountIdentifier, decimal amount);
	AccountBalance GetBalance(string accountIdentifier);
	void Transfer(LoanTransferDto transfer);
	void Withdraw(string accountIdentifier, decimal amount);
}
