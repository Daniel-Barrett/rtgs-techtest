namespace RtgsGlobal.TechTest.Api.Models.DTOs;

public record LoanTransferDto(string DebtorAccountIdentifier, string CreditorAccountIdentifier, decimal Amount);
