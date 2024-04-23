namespace WebAPI.Contracts.Payment;

public record CreatePaymentRequest(
    Amount Amount,
    RequestConfirmation Confirmation,
    string Description,
    bool Capture = true
);