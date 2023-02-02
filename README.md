# RTGS.global .NET Tech Test

Thank you for taking the time to do the [RTGS.global](https://rtgs.global) dotnet tech test. At RTGS.global we have a strong focus on __TDD__ and we'd expect the tech test to be completed with this in mind. We have implemented the following scenarios:

1. If a deposit of `1000` is made to `account A` the balance should be `1000`
2. If deposits of `1000` and `2000` are made to `account A` then the balance should be `3000`
3. If `1000` is transferred from `account A` to `account B` then `account A`'s balance should be `-1000` and `account B`'s balance should be `1000`

## Prerequisite
Download [.NET 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) and an IDE that can run the project.

## Step 1

In this first step please spend no more than __30 minutes__ addressing the issues below. This step is for you to become familiar with the code base before bringing it to our pairing session in [Step 3](#step-3---pairing).

Please review the code base and add separate commits for each point below.

 - Can you identify any code smells?
 - Return appropriate error when trying to use an account that doesn't exist.
 - You should not be able to deposit a negative amount.
 - You should not be able to transfer money to the same account.

## Step 2 - Submit

Zip the solution up (including the git directory), upload to cloud storage and share a link where we can access it.

## Step 3 - Pairing

Once we have reviewed your solution, we will arrange a call to extend your solution with another scenario.
