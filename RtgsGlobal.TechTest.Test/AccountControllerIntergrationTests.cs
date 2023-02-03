using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using RtgsGlobal.TechTest.Api.Models;
using RtgsGlobal.TechTest.Api.Models.DTOs;
using RtgsGlobal.TechTest.Api.Services;
using Xunit;

namespace RtgsGlobal.TechTest.Test;

public class AccountControllerIntergrationTests : IClassFixture<WebApplicationFactory<Program>>
{
	private readonly HttpClient _client;

	public AccountControllerIntergrationTests(WebApplicationFactory<Program> fixture)
	{
		_client = fixture
			.WithWebHostBuilder(builder => builder.ConfigureServices(services =>
			{
				services.AddSingleton<IAccountProvider, AccountProvider>();
			}))
			.CreateDefaultClient();
	}

	[Fact]
	public async Task GivenAccountExistsWithNoTransactions_ThenGetBalanceShouldReturnZero()
	{
		var result = await _client.GetFromJsonAsync<AccountBalance>("/account/account-a");

		Assert.Equal(0, result.Balance);
	}

	[Fact]
	public async Task GivenAccountExists_WhenDepositIsAdded_ThenGetBalanceShouldReturnExpected()
	{
		await _client.PostAsJsonAsync("/account/account-a", "1000");
		var result = await _client.GetFromJsonAsync<AccountBalance>("/account/account-a");

		Assert.Equal(1000, result.Balance);
	}

	[Fact]
	public async Task GivenAccountExistsAndDepositIsAdded_WhenWithdrawalIsAdded_ThenGetBalanceShouldReturnExpected()
	{
		await _client.PostAsJsonAsync("/account/account-a", "1000");

		await _client.PostAsJsonAsync("/account/account-a/withdraw", "100");
		var result = await _client.GetFromJsonAsync<AccountBalance>("/account/account-a");

		Assert.Equal(900, result.Balance);
	}

	[Fact]
	public async Task GivenAccountExists_WhenMultipleDepositsAreAdded_ThenGetBalanceShouldReturnExpected()
	{
		await _client.PostAsJsonAsync("/account/account-a", "1000");
		await _client.PostAsJsonAsync("/account/account-a", "2000");
		var result = await _client.GetFromJsonAsync<AccountBalance>("/account/account-a");

		Assert.Equal(3000, result.Balance);
	}

	[Fact]
	public async Task GivenAccountExists_WhenTransferIsMade_ThenGetBalanceShouldReturnExpected()
	{
		await _client.PostAsJsonAsync("/account/transfer", new LoanTransferDto("account-a", "account-b", 1000));
		var accountA = await _client.GetFromJsonAsync<AccountBalance>("/account/account-a");
		var accountB = await _client.GetFromJsonAsync<AccountBalance>("/account/account-b");

		Assert.Equal(-1000, accountA.Balance);
		Assert.Equal(1000, accountB.Balance);
	}

	[Fact]
	public async Task GivenAccountDoesNotExist_ThenShouldReturnNotFound()
	{
		// arrange
		// act
		var result = await _client.GetAsync("/account/account-that-does-not-exist");

		// assert
		Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
	}

	[Fact]
	public async Task GivenAccountDoesNotExist_WhenDepositAttemptIsMade_ThenShouldReturnNotFound()
	{
		// arrange
		// act 
		var result = await _client.PostAsJsonAsync("/account/account-that-does-not-exist", "1000");

		// assert
		Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
	}

	[Fact]
	public async Task GivenAccountDoesNotExist_WhenWithdrawAttemptIsMade_ThenShouldReturnNotFound()
	{
		// arrange
		// act 
		var result = await _client.PostAsJsonAsync("/account/account-that-does-not-exist/withdraw", "1000");

		// assert
		Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
	}

	[Fact]
	public async Task GivenAccountDoesNotExist_WhenTransferAttemptIsMade_ThenShouldReturnBadRequestWithErrorMessae()
	{
		// arrange
		var dto = new LoanTransferDto("account-that-does-not-exist", "account-b", 1000);
		var errorMessage = "error: invalid account identifier";

		// act 
		var result = await _client.PostAsJsonAsync("/account/transfer", dto);

		// assert
		Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
		Assert.Equal(errorMessage, await result.Content.ReadAsStringAsync());
	}

	[Fact]
	public async Task GivenAccountExists_WhenNegativeDepositIsAttempted_ThenShouldReturnBadRequestWithErrorMessage()
	{
		// arrange
		var errorMessage = "error: cannot deposit less than 0";

		// act
		var result = await _client.PostAsJsonAsync("/account/account-a", "-1000");

		// assert
		Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
		Assert.Equal(errorMessage, await result.Content.ReadAsStringAsync());
	}

	[Fact]
	public async Task GivenAccountExists_WhenTransferToSameAccountIsAttempted_ThenShouldReturnBadRequestWithErrorMessage()
	{
		// arrange
		var errorMessage = "error: cannot transfer to same account";
		var dto = new LoanTransferDto("account-a", "account-a", 1000);

		// act
		var result = await _client.PostAsJsonAsync("/account/transfer", dto);

		// assert
		Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
		Assert.Equal(errorMessage, await result.Content.ReadAsStringAsync());
	}

	[Fact]	
	public async Task GivenAccountExists_WhenWithdrawingMoreThanBalance_ThenShouldReturnBadRequestWithErrorMessage()
	{
		// arrange
		var errorMessage = "error: cannot withdraw more than balance";

		// act
		await _client.PostAsJsonAsync("/account/account-a", "1000");
		var result = await _client.PostAsJsonAsync("/account/account-a/withdraw", "2000");

		// assert
		Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
		Assert.Equal(errorMessage, await result.Content.ReadAsStringAsync());

	}
}
