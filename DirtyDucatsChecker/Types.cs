using System;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DirtyDucatsChecker
{
    // To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
    //
    //    using QuickType;
    //
    //    var welcome = Welcome.FromJson(jsonString);

    public partial class TennoZoneData
    {
        [JsonProperty("accessPacks")]
        public AccessPack[] AccessPacks { get; set; }

        [JsonProperty("sets")]
        public Set[] Sets { get; set; }

        [JsonProperty("parts")]
        public Part[] Parts { get; set; }

        [JsonProperty("relics")]
        public Relic[] Relics { get; set; }

        [JsonProperty("prices")]
        public Price[] Prices { get; set; }

        [JsonProperty("pricesUpdated")]
        public DateTimeOffset PricesUpdated { get; set; }

        [JsonProperty("drops")]
        public Drop[] Drops { get; set; }

        [JsonProperty("bounties")]
        public Bounty[] Bounties { get; set; }

        [JsonProperty("alert")]
        public Alert Alert { get; set; }
    }

    public partial class AccessPack
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("releaseDate")]
        public DateTimeOffset ReleaseDate { get; set; }

        [JsonProperty("sets")]
        public long[] Sets { get; set; }

        [JsonProperty("vaultDate", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? VaultDate { get; set; }

        [JsonProperty("unvaulting", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Unvaulting { get; set; }
    }

    public partial class Alert
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public partial class Bounty
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("isGhoul")]
        public bool IsGhoul { get; set; }

        [JsonProperty("rewards")]
        public BountyReward[] Rewards { get; set; }

        [JsonProperty("score")]
        public Score Score { get; set; }

        [JsonProperty("totalScore")]
        public double TotalScore { get; set; }
    }

    public partial class BountyReward
    {
        [JsonProperty("rotation")]
        public Rotation Rotation { get; set; }

        [JsonProperty("relics")]
        public PurpleRelic[] Relics { get; set; }
    }

    public partial class PurpleRelic
    {
        [JsonProperty("itemName")]
        public string ItemName { get; set; }

        [JsonProperty("chance")]
        public double Chance { get; set; }

        [JsonProperty("expectedReturn")]
        public double ExpectedReturn { get; set; }
    }

    public partial class Score
    {
        [JsonProperty("Lith")]
        public double? Lith { get; set; }

        [JsonProperty("Meso")]
        public double? Meso { get; set; }

        [JsonProperty("Neo")]
        public double? Neo { get; set; }

        [JsonProperty("Axi")]
        public double? Axi { get; set; }
    }

    public partial class Drop
    {
        [JsonProperty("missions")]
        public Mission[] Missions { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("rewards")]
        public DropReward[] Rewards { get; set; }

        [JsonProperty("score")]
        public Score Score { get; set; }

        [JsonProperty("totalScore")]
        public double TotalScore { get; set; }
    }

    public partial class Mission
    {
        [JsonProperty("planet")]
        public string Planet { get; set; }

        [JsonProperty("mission")]
        public string MissionMission { get; set; }
    }

    public partial class DropReward
    {
        [JsonProperty("rotation")]
        public Rotation? Rotation { get; set; }

        [JsonProperty("relics")]
        public FluffyRelic[] Relics { get; set; }
    }

    public partial class FluffyRelic
    {
        [JsonProperty("itemName")]
        public string ItemName { get; set; }

        [JsonProperty("chance")]
        public double Chance { get; set; }
    }

    public partial class Part
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("setId")]
        public long SetId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("ducats", NullValueHandling = NullValueHandling.Ignore)]
        public long? Ducats { get; set; }
    }

    public partial class Price
    {
        [JsonProperty("partId")]
        public long PartId { get; set; }

        [JsonProperty("priceInfo")]
        public PriceInfo PriceInfo { get; set; }
    }

    public partial class PriceInfo
    {
        [JsonProperty("average")]
        public double Average { get; set; }

        [JsonProperty("median")]
        public double Median { get; set; }
    }

    public partial class Relic
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("parts")]
        public RelicPart[] Parts { get; set; }

        [JsonProperty("isVaulted", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsVaulted { get; set; }
    }

    public partial class RelicPart
    {
        [JsonProperty("partId")]
        public long PartId { get; set; }

        [JsonProperty("rarity")]
        public long Rarity { get; set; }
    }

    public partial class Set
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public long? Type { get; set; }

        [JsonProperty("vaultDate", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? VaultDate { get; set; }

        [JsonProperty("founders", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Founders { get; set; }
    }

    public enum Rotation { A, B, C };

    public partial class TennoZoneData
    {
        public static TennoZoneData FromJson(string json) => JsonConvert.DeserializeObject<TennoZoneData>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this TennoZoneData self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
                RotationConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class RotationConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Rotation) || t == typeof(Rotation?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "A":
                    return Rotation.A;
                case "B":
                    return Rotation.B;
                case "C":
                    return Rotation.C;
            }
            throw new Exception("Cannot unmarshal type Rotation");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Rotation)untypedValue;
            switch (value)
            {
                case Rotation.A:
                    serializer.Serialize(writer, "A");
                    return;
                case Rotation.B:
                    serializer.Serialize(writer, "B");
                    return;
                case Rotation.C:
                    serializer.Serialize(writer, "C");
                    return;
            }
            throw new Exception("Cannot marshal type Rotation");
        }

        public static readonly RotationConverter Singleton = new RotationConverter();
    }
}
