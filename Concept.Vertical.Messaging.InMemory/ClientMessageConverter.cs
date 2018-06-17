using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Concept.Vertical.Messaging.InMemory
{
  public class ClientMessageConverter : JsonConverter
  {
    private readonly Type _clientMsgType;

    public ClientMessageConverter()
    {
      _clientMsgType = typeof(ClientMessage);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      throw new NotImplementedException();
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
      var jObj = JObject.Load(reader);
      var clientMsg = new ClientMessage
      {
        ClientIds = jObj[nameof(ClientMessage.ClientIds)].Values<string>()?.ToList()?.AsReadOnly(),
        Payload = jObj.Property(nameof(ClientMessage.Payload)).Value,
        Type = jObj[nameof(ClientMessage.Type)].Value<string>()
      };
      return clientMsg;
    }

    public override bool CanWrite => false;

    public override bool CanConvert(Type objectType) => objectType == _clientMsgType;
  }
}
