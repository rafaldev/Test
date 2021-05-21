// © Copyright [2021] DankseBank. All rights reserved

namespace DankseBank.Loan.Domain.Services
{
    using System.Threading.Tasks;
    using Model;

    public class LoanCalculationService : ILoanCalculationService
    {
        public async Task<double> GetMonthlyPayment(
            double loanAmount,
            double annualInterestRate,
            int durationOfLoanMonthly,
            double amountAdministrationFee)
        {
            var amountMonthlyWithoutTaxes = loanAmount / durationOfLoanMonthly;
            var amountInterestRate = await GetAmountInterestRate(annualInterestRate, amountMonthlyWithoutTaxes);
            var amountOfNotUnderstandableTax = await GetAmountOfNotUnderstandableTax(amountMonthlyWithoutTaxes);
            var amountAdministrationFeeMonthly = amountAdministrationFee / durationOfLoanMonthly;

            var monthlyPayment = amountMonthlyWithoutTaxes
                                 + amountInterestRate
                                 + amountOfNotUnderstandableTax
                                 + amountAdministrationFeeMonthly;

            return await Task.FromResult(monthlyPayment);
        }

        public async Task<double> GetAmountOfNotUnderstandableTax(double loanAmount)
        {
            return await Task.FromResult(loanAmount / 100 * 21.277);
        }

        public async Task<double> GetAmountInterestRate(double annualInterestRate, double loanAmount)
        {
            return await Task.FromResult(loanAmount * annualInterestRate);
        }

        public async Task<double> GetYearlyPayment(
            double loanAmount,
            double annualInterestRate,
            int durationOfLoanMonthly,
            double amountAdministrationFee)
        {
            return await GetMonthlyPayment(loanAmount, annualInterestRate, durationOfLoanMonthly, amountAdministrationFee) *
                   durationOfLoanMonthly;
        }

        public async Task<LoanReport> GenerateReport(
            double loanAmount,
            double annualInterestRate,
            int durationOfLoanMonthly,
            double amountAdministrationFee)
        {
            return new LoanReport()
            {
                MonthlyPayment = await GetMonthlyPayment(
                    loanAmount,
                    annualInterestRate,
                    durationOfLoanMonthly,
                    amountAdministrationFee),
                YearlyPayment = await GetYearlyPayment(
                    loanAmount,
                    annualInterestRate,
                    durationOfLoanMonthly,
                    amountAdministrationFee)
            };
        }

        public async Task<double> GetAdministrationFee(
            double loanAmount)
        {
            if (loanAmount / 100 < 10000)
            {
                return await Task.FromResult(loanAmount / 100);
            }

            return await Task.FromResult(10000);
        }
    }
}