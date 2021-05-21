// © Copyright [2021] DankseBank. All rights reserved.

namespace DankseBank.Loan.Domain.Services
{
    using System.Threading.Tasks;
    using Model;

    public interface ILoanCalculationService
    {
        Task<double> GetMonthlyPayment(
            double loanAmount,
            double annualInterestRate,
            int durationOfLoanMonthly,
            double amountAdministrationFee);

        Task<double> GetAmountOfNotUnderstandableTax(double loanAmount);

        Task<double> GetAmountInterestRate(double annualInterestRate, double loanAmount);

        Task<double> GetYearlyPayment(
            double loanAmount,
            double annualInterestRate,
            int durationOfLoanMonthly,
            double amountAdministrationFee);

        Task<LoanReport> GenerateReport(
            double loanAmount,
            double annualInterestRate,
            int durationOfLoanMonthly,
            double amountAdministrationFee);

        Task<double> GetAdministrationFee(
            double loanAmount);
    }
}