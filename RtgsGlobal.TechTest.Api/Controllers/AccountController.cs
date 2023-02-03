using System.Security.Cryptography.Xml;
using Microsoft.AspNetCore.Mvc;
using RtgsGlobal.TechTest.Api.Models.DTOs;
using RtgsGlobal.TechTest.Api.Services;

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

	/// <summary>
	/// Deposit given amount to a given account
	/// </summary>
	/// <param name="accountIdentifier"></param>
	/// <param name="amount"></param>
	[HttpPost("{accountIdentifier}", Name = "Deposit")]
	public async Task<IActionResult> Deposit(string accountIdentifier, [FromBody] decimal amount)
	{
		try
		{
			_accountProvider.Deposit(accountIdentifier, amount);
			return Ok();
		}
		catch (KeyNotFoundException)
		{
			return NotFound();
		}
		catch (ArgumentOutOfRangeException)
		{
			return BadRequest("error: cannot deposit less than 0");
		}
	}

	/// <summary>
	/// Withdraw a given amount from a given account
	/// </summary>
	/// <param name="accountIdentifier"></param>
	/// <param name="amount"></param>
	[HttpPost("{accountIdentifier}/withdraw", Name = "Withdrawal")]
	public async Task<IActionResult> Withdraw(string accountIdentifier, [FromBody] decimal amount)
	{
		try
		{
			_accountProvider.Withdraw(accountIdentifier, amount);
			return Ok();
		}
		catch (KeyNotFoundException)
		{
			return NotFound();
		}
		catch (ArgumentException)
		{
			return BadRequest("error: cannot withdraw more than balance");
		}
	}

	/// <summary>
	/// Transfer a given value between two given accounts
	/// </summary>
	/// <param name="transfer"></param>
	[HttpPost("transfer", Name = "Transfer")]
	public async Task<IActionResult> Transfer(LoanTransferDto transfer)
	{
		try
		{
			_accountProvider.Transfer(transfer);
			return Ok();
		}
		catch (KeyNotFoundException)
		{
			return BadRequest("error: invalid account identifier");
		}
		catch (ArgumentException)
		{
			return BadRequest("error: cannot transfer to same account");
		}
	}

	/// <summary>
	/// Get the balance for a given account
	/// </summary>
	/// <param name="accountIdentifier"></param>
	/// <returns>Balance</returns>
	[HttpGet("{accountIdentifier}", Name = "GetBalance")]
	public async Task<IActionResult> Get(string accountIdentifier)
	{
		try
		{
			var balance = _accountProvider.GetBalance(accountIdentifier);
			return Ok(balance);
		}
		catch (KeyNotFoundException)
		{
			return NotFound();
		}
	}
}
