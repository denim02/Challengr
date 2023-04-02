using Notes.Models;
using Notes.Services;
using Plugin.NFC;
using System.Text;

namespace Notes.Views;

public partial class ChallengeView : ContentPage
{
    public ChallengeView()
    {
        InitializeComponent();

        BaseClient client = new BaseClient();
        ChallengeServices challengeService = new ChallengeServices(client);

        challengeService.GetChallenge("challenge_1").ContinueWith((task) =>
        {
            var challenge = task.Result;
            this.BindingContext = challenge;
        });
    }

    public const string ALERT_TITLE = "NFC";
    public const string MIME_TYPE = "text/plain";
    private PublishServices publishServices = new PublishServices(new BaseClient());

    private bool _challengeStarted = false;

    // NFC Code

    NFCNdefTypeFormat _type;
    bool _makeReadOnly = false;
    bool _eventsAlreadySubscribed = false;
    bool _isDeviceiOS = false;

    public bool MessageIsReceived
    {
        get => _messsageReceived;
        set
        {
            _messsageReceived = value;
            OnPropertyChanged(nameof(MessageIsReceived));
        }
    }

    public bool DeviceIsListening
    {
        get => _deviceIsListening;
        set
        {
            _deviceIsListening = value;
            OnPropertyChanged(nameof(DeviceIsListening));
        }
    }
    private bool _deviceIsListening;
    private bool _messsageReceived;

    private bool _nfcIsEnabled;
    public bool NfcIsEnabled
    {
        get => _nfcIsEnabled;
        set
        {
            _nfcIsEnabled = value;
            OnPropertyChanged(nameof(NfcIsEnabled));
            OnPropertyChanged(nameof(NfcIsDisabled));
        }
    }

    public bool NfcIsDisabled => !NfcIsEnabled;

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // In order to support Mifare Classic 1K tags (read/write), you must set legacy mode to true.
        CrossNFC.Legacy = false;

        if (CrossNFC.IsSupported)
        {


            NfcIsEnabled = CrossNFC.Current.IsEnabled;


            await AutoStartAsync().ConfigureAwait(false);

        }
    }

    protected override bool OnBackButtonPressed()
    {
        Task.Run(() => StopListening());
        return base.OnBackButtonPressed();
    }

    async Task AutoStartAsync()
    {
        await Task.Delay(500);
        await StartListeningIfNotiOS();
    }

    /// <summary>
    /// Subscribe to the NFC events
    /// </summary>
    void SubscribeEvents()
    {
        if (_eventsAlreadySubscribed)
            UnsubscribeEvents();

        _eventsAlreadySubscribed = true;

        CrossNFC.Current.OnMessageReceived += Current_OnMessageReceived;
        CrossNFC.Current.OnTagDiscovered += Current_OnTagDiscovered;
        CrossNFC.Current.OnTagListeningStatusChanged += Current_OnTagListeningStatusChanged;
    }

    /// <summary>
    /// Unsubscribe from the NFC events
    /// </summary>
    void UnsubscribeEvents()
    {
        CrossNFC.Current.OnMessageReceived -= Current_OnMessageReceived;
        CrossNFC.Current.OnTagDiscovered -= Current_OnTagDiscovered;
        CrossNFC.Current.OnTagListeningStatusChanged -= Current_OnTagListeningStatusChanged;

        _eventsAlreadySubscribed = false;
    }

    /// <summary>
    /// Event raised when Listener Status has changed
    /// </summary>
    /// <param name="isListening"></param>
    void Current_OnTagListeningStatusChanged(bool isListening) => DeviceIsListening = isListening;

    /// <summary>
    /// Event raised when a NDEF message is received
    /// </summary>
    /// <param name="tagInfo">Received <see cref="ITagInfo"/></param>
    async void Current_OnMessageReceived(ITagInfo tagInfo)
    {
        if (tagInfo == null)
        {
            await ShowAlert("No tag found");
            return;
        }

        // Customized serial number
        var identifier = tagInfo.Identifier;
        var serialNumber = NFCUtils.ByteArrayToHexString(identifier, ":");
        var title = !string.IsNullOrWhiteSpace(serialNumber) ? $"Tag [{serialNumber}]" : "Tag Info";

        if (!tagInfo.IsSupported)
        {
            await ShowAlert("Unsupported tag (app)", title);
        }
        else if (tagInfo.IsEmpty)
        {
            await ShowAlert("Empty tag", title);
        }
        else
        {
            var first = tagInfo.Records[0];
            var message = first.Message;

            if (message == "2")
            {
                await publishServices.PublishEndTime();
                var times = await publishServices.ReadTimes();


                publishServices.SendCommandToDispenser();
                await ShowAlert($"You did it! Start time: {times[0]}, End time: {times[1]}, Total elapsed time: {(times[1] - times[0]).TotalSeconds.ToString("n2")}", "Congratulations");

            }

        }
    }

    async void Current_OnTagDiscovered(ITagInfo tagInfo, bool format)
    {
        try
        {
            NFCNdefRecord record = null;
            switch (_type)
            {
                case NFCNdefTypeFormat.WellKnown:
                    record = new NFCNdefRecord
                    {
                        TypeFormat = NFCNdefTypeFormat.WellKnown,
                        MimeType = MIME_TYPE,
                        Payload = NFCUtils.EncodeToByteArray("Plugin.NFC is awesome!"),
                        LanguageCode = "en"
                    };
                    break;
                case NFCNdefTypeFormat.Uri:
                    record = new NFCNdefRecord
                    {
                        TypeFormat = NFCNdefTypeFormat.Uri,
                        Payload = NFCUtils.EncodeToByteArray("https://github.com/franckbour/Plugin.NFC")
                    };
                    break;
                case NFCNdefTypeFormat.Mime:
                    record = new NFCNdefRecord
                    {
                        TypeFormat = NFCNdefTypeFormat.Mime,
                        MimeType = MIME_TYPE,
                        Payload = NFCUtils.EncodeToByteArray("Plugin.NFC is awesome!")
                    };
                    break;
                default:
                    break;
            }

            if (!format && record == null)
                throw new Exception("Record can't be null.");

            tagInfo.Records = new[] { record };

        }
        catch (Exception ex)
        {
            await ShowAlert(ex.Message);
        }
    }

    /// <summary>
    /// Returns the tag information from NDEF record
    /// </summary>
    /// <param name="record"><see cref="NFCNdefRecord"/></param>
    /// <returns>The tag information</returns>
    string GetMessage(NFCNdefRecord record)
    {
        var message = $"Message: {record.Message}";
        message += Environment.NewLine;
        message += $"RawMessage: {Encoding.UTF8.GetString(record.Payload)}";
        message += Environment.NewLine;
        message += $"Type: {record.TypeFormat}";

        if (!string.IsNullOrWhiteSpace(record.MimeType))
        {
            message += Environment.NewLine;
            message += $"MimeType: {record.MimeType}";
        }

        return message;
    }

    /// <summary>
    /// Display an alert
    /// </summary>
    /// <param name="message">Message to be displayed</param>
    /// <param name="title">Alert title</param>
    /// <returns>The task to be performed</returns>
    Task ShowAlert(string message, string title = null) => DisplayAlert(string.IsNullOrWhiteSpace(title) ? ALERT_TITLE : title, message, "OK");

    /// <summary>
    /// Task to start listening for NFC tags if the user's device platform is not iOS
    /// </summary>
    /// <returns>The task to be performed</returns>
    async Task StartListeningIfNotiOS()
    {
        if (_isDeviceiOS)
        {
            SubscribeEvents();
            return;
        }
        await BeginListening();
    }

    /// <summary>
    /// Task to safely start listening for NFC Tags
    /// </summary>
    /// <returns>The task to be performed</returns>
    async Task BeginListening()
    {
        try
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                SubscribeEvents();
                CrossNFC.Current.StartListening();
            });
        }
        catch (Exception ex)
        {
            await ShowAlert(ex.Message);
        }
    }

    /// <summary>
    /// Task to safely stop listening for NFC tags
    /// </summary>
    /// <returns>The task to be performed</returns>
    async Task StopListening()
    {
        try
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                CrossNFC.Current.StopListening();
                UnsubscribeEvents();
            });
        }
        catch (Exception ex)
        {
            await ShowAlert(ex.Message);
        }
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Launcher.OpenAsync("https://goo.gl/maps/smj55cY2HR7QeVAN8");
    }
}