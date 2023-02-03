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
		catch (Exception)
		{
			return BadRequest("error: invalid account identifier");
		}
	}

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
		catch (Exception)
		{
			return BadRequest("error: invalid account identifier");
		}
	}

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
			return NotFound();
		}
		catch (Exception)
		{
			return BadRequest("error: invalid account identifier");
		}
	}

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
