using Microsoft.AspNetCore.Mvc;

namespace RtgsGlobal.TechTest.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
	private readonly IAccountProvider _accountProvider;

	public AccountController(IAccountProvider accountProvider)
	{
		_accountProvider = accountProvider;
	}

	[HttpPost("{accountIdentifier}", Name = "Deposit")]
	public IActionResult Deposit(string accountIdentifier, [FromBody] float amount)
	{
		_accountProvider.Deposit(accountIdentifier, amount);
		return Ok();
	}

	[HttpPost("{accountIdentifier}/withdraw", Name = "Withdrawal")]
	public IActionResult Withdraw(string accountIdentifier, [FromBody] float amount)
	{
		_accountProvider.Withdraw(accountIdentifier, amount);
		return Ok();
	}

	[HttpPost("transfer", Name = "Transfer")]
	public IActionResult Transfer(MyTransferDto transfer)
	{
		_accountProvider.Transfer(transfer);
		return Accepted();
	}

	[HttpGet("{accountIdentifier}", Name = "GetBalance")]
	public MyBalance Get(string accountIdentifier) => _accountProvider.GetBalance(accountIdentifier);
}

public class MyTransferDto
{
	public MyTransferDto(string debtorAccountIdentifier, string creditorAccountIdentifier, float amount)
	{
		DebtorAccountIdentifier = debtorAccountIdentifier;
		CreditorAccountIdentifier = creditorAccountIdentifier;
		Amount = amount;
	}

	public string DebtorAccountIdentifier { get; set; }
	public string CreditorAccountIdentifier { get; set; }
	public float Amount { get; set; }
}
