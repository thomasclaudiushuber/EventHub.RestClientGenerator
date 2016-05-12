using System;
using ThomasClaudiusHuber.Azure.EventHub.RestClientGenerator.EventHubLogic;
using ThomasClaudiusHuber.Azure.EventHub.RestClientGenerator.UwpTextTemplate;

namespace ThomasClaudiusHuber.Azure.EventHub.RestClientGenerator.ViewModel
{
  public class MainViewModel : ViewModelBase
  {
    private string _connectionString;
    private ConnectionDetails _connectionDetails;
    private string _publisherName;
    private string _tokenLifeTime;
    private string _sharedAccessSignature;
    private string _eventHubRestUri;
    private string _uwpEventHubClient;

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

    private void GenerateOutput()
    {
      if (string.IsNullOrWhiteSpace(ConnectionString))
      {
        ConnectionDetails = null;
        return;
      }

      ConnectionDetails details;
      if (ConnectionDetailsExtractor.TryExtract(ConnectionString, out details))
      {
        ConnectionDetails = details;
        int minutes;
        if (int.TryParse(TokenLifeTime, out minutes))
        {
          var generator = new RestUriAndSharedAccessSignatureGenerator(details,PublisherName, TimeSpan.FromMinutes(minutes));
          SharedAccessSignature = generator.GenerateSharedAccessSignature();
          EventHubRestUri = generator.GetEventHubRestUri();

          UwpEventHubClient = new EventHubClientTemplate(SharedAccessSignature, EventHubRestUri).TransformText();
        }
      }

    }
  }
}
