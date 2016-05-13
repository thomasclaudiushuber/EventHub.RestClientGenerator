using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using ThomasClaudiusHuber.Azure.EventHub.RestClientGenerator.EventHubLogic;
using ThomasClaudiusHuber.Azure.EventHub.RestClientGenerator.UwpTextTemplate;

namespace ThomasClaudiusHuber.Azure.EventHub.RestClientGenerator.ViewModel
{
  public class MainViewModel : ViewModelBase, INotifyDataErrorInfo
  {
    private Dictionary<string, List<string>> _errorsByPropertyName;
    private string _connectionString;
    private ConnectionDetails _connectionDetails;
    private string _publisherName;
    private string _tokenLifeTime;
    private string _sharedAccessSignature;
    private string _eventHubRestUri;
    private string _uwpEventHubClient;
    private TokenLifeTimeUnit _tokenLifeTimeUnit;

    public MainViewModel()
    {
      _errorsByPropertyName = new Dictionary<string, List<string>>();
      TokenLifeTimeUnits = Enum.GetValues(typeof(TokenLifeTimeUnit)).Cast<TokenLifeTimeUnit>().ToList();
      TokenLifeTimeUnit = TokenLifeTimeUnit.Days;
      TokenLifeTime = "365";
    }

    public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

    public string ConnectionString
    {
      get { return _connectionString; }
      set
      {
        _connectionString = value;
        OnPropertyChanged();
        GenerateOutput();
      }
    }

    public ConnectionDetails ConnectionDetails
    {
      get { return _connectionDetails; }
      private set
      {
        _connectionDetails = value;
        OnPropertyChanged();
      }
    }

    public string PublisherName
    {
      get { return _publisherName; }
      set
      {
        _publisherName = value;
        OnPropertyChanged();
        GenerateOutput();
      }
    }

    public string TokenLifeTime
    {
      get { return _tokenLifeTime; }
      set
      {
        _tokenLifeTime = value;
        OnPropertyChanged();
        GenerateOutput();
      }
    }

    public IEnumerable<TokenLifeTimeUnit> TokenLifeTimeUnits { get; }

    public TokenLifeTimeUnit TokenLifeTimeUnit
    {
      get { return _tokenLifeTimeUnit; }
      set
      {
        _tokenLifeTimeUnit = value;
        OnPropertyChanged();
        GenerateOutput();
      }
    }

    public string SharedAccessSignature
    {
      get { return _sharedAccessSignature; }
      set
      {
        _sharedAccessSignature = value;
        OnPropertyChanged();
      }
    }

    public string EventHubRestUri
    {
      get { return _eventHubRestUri; }
      set
      {
        _eventHubRestUri = value;
        OnPropertyChanged();
      }
    }

    public string UwpEventHubClient
    {
      get { return _uwpEventHubClient; }
      set
      {
        _uwpEventHubClient = value;
        OnPropertyChanged();
      }
    }

    public bool HasErrors
    {
      get
      {
        return _errorsByPropertyName.Any();
      }
    }

    private void GenerateOutput()
    {
      
      ConnectionDetails details = null;

      if (string.IsNullOrWhiteSpace(ConnectionString))
      {
        ConnectionDetails = null;
        _connectionString = "";
        OnPropertyChanged(nameof(ConnectionString));
      }
      else
      {
        RemoveAnySpacesFromTheConnectionString();

        if (!ConnectionDetailsExtractor.TryExtract(ConnectionString, out details))
        {
          _errorsByPropertyName[nameof(ConnectionString)] = new List<string> { "Invalid event hub connection string. Please copy it from the Azure Portal." };
          OnErrorsChanged(nameof(ConnectionString));
        }
        else
        {
          ConnectionDetails = details;
          if(_errorsByPropertyName.ContainsKey(nameof(ConnectionString)))
          {
            _errorsByPropertyName.Remove(nameof(ConnectionString));
            OnErrorsChanged(nameof(ConnectionString));
          }
        }
      }

      int tokenLifeTimeInt;
      if (!int.TryParse(TokenLifeTime, out tokenLifeTimeInt))
      {
        _errorsByPropertyName[nameof(TokenLifeTime)] = new List<string> { "Invalid Token Lifetime. Please specify an integer" };
        OnErrorsChanged(nameof(TokenLifeTime));
      }
      else
      {
        if (_errorsByPropertyName.ContainsKey(nameof(TokenLifeTime)))
        {
          _errorsByPropertyName.Remove(nameof(TokenLifeTime));
          OnErrorsChanged(nameof(TokenLifeTime));
        }
      }

      // TODO: Put this in a try-catch to find out too big values
      TimeSpan tokenLifeTime = GetTokenLifeTime(tokenLifeTimeInt);

      if (!_errorsByPropertyName.Any()
        && !string.IsNullOrWhiteSpace(ConnectionString)
        && !string.IsNullOrWhiteSpace(PublisherName))
      {
        var generator = new RestUriAndSharedAccessSignatureGenerator(details, PublisherName, tokenLifeTime);
        SharedAccessSignature = generator.GenerateSharedAccessSignature();
        EventHubRestUri = generator.GetEventHubRestUri();

        UwpEventHubClient = new EventHubClientTemplate(SharedAccessSignature, EventHubRestUri).TransformText();
      }
      else
      {
        SharedAccessSignature = null;
        EventHubRestUri = null;
        UwpEventHubClient = null;
      }
    }

    private void RemoveAnySpacesFromTheConnectionString()
    {
      _connectionString = ConnectionString.Trim().Replace(" ", "");
      OnPropertyChanged(nameof(ConnectionString));
    }

    private TimeSpan GetTokenLifeTime(int tokenLifeTimeInt)
    {
      TimeSpan tokenLifeTime;
      switch (TokenLifeTimeUnit)
      {
        case TokenLifeTimeUnit.Minutes:
          tokenLifeTime = TimeSpan.FromMinutes(tokenLifeTimeInt);
          break;
        case TokenLifeTimeUnit.Hours:
          tokenLifeTime = TimeSpan.FromHours(tokenLifeTimeInt);
          break;
        case TokenLifeTimeUnit.Days:
          tokenLifeTime = TimeSpan.FromDays(tokenLifeTimeInt);
          break;
        default:
          throw new Exception($"Type {TokenLifeTimeUnit} is not supported yet");
      }
      return tokenLifeTime;
    }

    public IEnumerable GetErrors(string propertyName)
    {
      return _errorsByPropertyName.ContainsKey(propertyName)
        ? _errorsByPropertyName[propertyName]
        : null;
    }

    private void OnErrorsChanged(string propertyName)
    {
      ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
    }
  }
  public enum TokenLifeTimeUnit
  {
    Minutes,
    Hours,
    Days
  }
}