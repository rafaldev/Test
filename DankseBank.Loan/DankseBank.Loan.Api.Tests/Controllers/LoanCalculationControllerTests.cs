namespace DankseBank.Loan.Api.Tests.Controllers
{
    using DankseBank.Loan.Api.Controllers;
    using DankseBank.Loan.Domain.Services;
    using Moq;
    using Serilog;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.Model;
    using Microsoft.AspNetCore.Mvc;
    using Shouldly;
    using Xunit;

    public class LoanCalculationControllerTests
    {
        private readonly LoanCalculationController _loanCalculationController;

        public LoanCalculationControllerTests()
        {
            var loanCalculationServiceMock = new LoanCalculationService();
            var loggerMock = new Mock<ILogger>();

            _loanCalculationController = new LoanCalculationController(
                loanCalculationServiceMock,
                loggerMock.Object);
        }

        [Theory]
        [MemberData(nameof(LoanData))]
        public async Task GenerateLoanReport_Should_Return_Correct_Report(double loanAmount, int durationOfLoanMonthly)
        {
            // Arrange
            CalculationLoanInputData calculationData = new CalculationLoanInputData()
            {
                DurationOfLoanMonthly = durationOfLoanMonthly,
                LoanAmount = loanAmount
            };

            // Act
            var result = await _loanCalculationController.GenerateLoanReport(
                calculationData);

            var okResult = result as OkObjectResult;
            var loanReport =  (LoanReport) okResult.Value;
            // Assert
            okResult.StatusCode.ShouldBe(200);
            loanReport.YearlyPayment.ShouldBe(636385.00000000012);
            loanReport.MonthlyPayment.ShouldBe(5303.2083333333339);
        }

        public static IEnumerable<object[]> LoanData =>
            new List<object[]>
            {
                new object[] { 500000, 120 }
            };
    }
}