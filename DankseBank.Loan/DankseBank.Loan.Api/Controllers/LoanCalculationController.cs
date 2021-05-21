// © Copyright [2021] DankseBank. All rights reserved.

namespace DankseBank.Loan.Api.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Domain.Model;
    using Domain.Services;
    using Microsoft.AspNetCore.Mvc;
    using Serilog;

    [ApiController]
    public class LoanCalculationController : ControllerBase
    {
        private readonly ILoanCalculationService _calculationService;
        private readonly ILogger _logger;

        public LoanCalculationController(
            ILoanCalculationService calculationService,
            ILogger logger)
        {
            _calculationService = calculationService;
            _logger = logger;
        }

        [HttpGet]
        [Route("GenerateLoanReport")]
        public async Task<ActionResult> GenerateLoanReport([FromBody] CalculationLoanInputData calculationData)
        {
            double annualInterestRate = 0.05;

            try
            {
                var amountAdministrationFee = await _calculationService.GetAdministrationFee(calculationData.LoanAmount)
                    .ConfigureAwait(false);

                var report = await _calculationService.GenerateReport(
                    calculationData.LoanAmount,
                    annualInterestRate,
                    calculationData.DurationOfLoanMonthly,
                    amountAdministrationFee);
                return Ok(report);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return BadRequest("problem with generate report");
            }
        }
    }
}