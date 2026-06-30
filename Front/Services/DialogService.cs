namespace Front.Services;

public enum DialogType
{
    Info,
    Success,
    Warning,
    Error
}

public class DialogService
{
    public event Action? OnChange;

    public bool IsVisible { get; private set; }
    public bool IsConfirm { get; private set; }
    public string Title { get; private set; } = "";
    public string Message { get; private set; } = "";
    public DialogType Type { get; private set; } = DialogType.Info;
    public string ConfirmText { get; private set; } = "Confirmer";
    public string CancelText { get; private set; } = "Annuler";
    public string OkText { get; private set; } = "OK";

    private TaskCompletionSource<bool>? _tcs;

    public Task ShowSuccessAsync(string message) =>
        ShowAlertAsync(message, "Succès", DialogType.Success);

    public Task ShowErrorAsync(string message) =>
        ShowAlertAsync(message, "Erreur", DialogType.Error);

    public Task ShowWarningAsync(string message) =>
        ShowAlertAsync(message, "Attention", DialogType.Warning);

    public Task ShowAlertAsync(string message, string title = "Information", DialogType type = DialogType.Info)
    {
        IsConfirm = false;
        IsVisible = true;
        Title = title;
        Message = message;
        Type = type;
        OkText = "OK";
        _tcs = new TaskCompletionSource<bool>();
        Notify();
        return _tcs.Task;
    }

    public Task<bool> ShowConfirmAsync(
        string message,
        string title = "Confirmation",
        string confirmText = "Confirmer",
        string cancelText = "Annuler")
    {
        IsConfirm = true;
        IsVisible = true;
        Title = title;
        Message = message;
        Type = DialogType.Warning;
        ConfirmText = confirmText;
        CancelText = cancelText;
        _tcs = new TaskCompletionSource<bool>();
        Notify();
        return _tcs.Task;
    }

    public void Close(bool result = true)
    {
        if (!IsVisible) return;

        IsVisible = false;
        _tcs?.TrySetResult(result);
        _tcs = null;
        Notify();
    }

    private void Notify() => OnChange?.Invoke();
}
