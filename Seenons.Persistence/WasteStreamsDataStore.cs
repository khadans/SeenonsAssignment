using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Npgsql;
using Seenons.Adapters.Persistence;
using Seenons.Persistence.Utils;
using Seenons.Persistence.WasteStreams.Models;
using Seenons.Ports;
using Seenons.WasteStreams;

namespace Seenons.Persistence
{
    public class WasteStreamsDataStore : IWasteStreamsDataStore
    {
        private readonly IDbSettings _settings;

        public WasteStreamsDataStore(IDbSettings settings)
        {
            _settings = settings;
        }

        public async Task<IEnumerable<WasteStream>> GetWasteStreamsAsync(string postalCode)
        {
            var timeSlotsRows = await GetLogisticalProviderTimeSlotsForPostalCode(postalCode);

            return await GetWasteStreamsWithPickupTimeSlots(timeSlotsRows);
        }

        public async Task<IEnumerable<WasteStream>> GetWasteStreamsAsync(string postalCode, ushort[] days)
        {
            var timeSlotsRows = await GetLogisticalProviderTimeSlotsForPostalCodeAndDays(postalCode, days);

            return await GetWasteStreamsWithPickupTimeSlots(timeSlotsRows);
        }

        private async Task<IEnumerable<WasteStream>> GetWasteStreamsWithPickupTimeSlots(IEnumerable<TimeSlotRow> timeSlotRows)
        {
            var timeSlots = GetProviderPickupAreaTimeSlots(timeSlotRows).ToArray();

            var providerPickupAreaIds = timeSlots.Select(t => t.ProviderPickupAreaId).Distinct().ToArray();

            var wasteStreamRows = await GetWasteStreamsRows(providerPickupAreaIds);

            var wasteStreamsDetails = await MakeWasteStreamsWithPickupTimeSlots(wasteStreamRows, timeSlots);

            return wasteStreamsDetails;
        }

        private async Task<IEnumerable<WasteStream>> MakeWasteStreamsWithPickupTimeSlots(
            IEnumerable<WasteStreamRow> wasteStreamRows,
            IEnumerable<ProviderPickupAreaTimeSlots> providerTimeSlots
        )
        {
            var wasteStreams = new List<WasteStream>();

            var wasteStreamGroups = wasteStreamRows
               .GroupBy(w => new { w.name, w.type, w.stream_product_id });

            foreach (var wasteStream in wasteStreamGroups)
            {
                var wasteStreamId = wasteStream.Key.stream_product_id;

                var providerPickupAreas = wasteStream.ToArray().Select(r => r.providerpickupareaid)
                                                     .ToArray();

                var sizesRows = await GetWasteStreamSizes(wasteStreamId, providerPickupAreas);

                var sizes = sizesRows.Select(
                    s => new WasteStreamSize
                    {
                        Id = s.id,
                        Size = s.size,
                        UnitPricePickup = s.unit_price_pickup,
                        DiscountPercentage = s.discount_percentage,
                        UnitPricePlacement = s.unit_price_placement,
                        UnitPriceRent = s.unit_price_rent,
                        Container = new Container()
                        {
                            Id = s.container_product_id,
                            Type = s.container_type,
                            Name = s.container_name
                        }
                    }
                );

                var wasteStreamDetails = new WasteStream
                {
                    StreamProductId = wasteStream.Key.stream_product_id,
                    Name = wasteStream.Key.name,
                    Type = wasteStream.Key.type,
                    Sizes = sizes,
                    ProviderPickupAreaTimeSlots = wasteStream
                                                              .ToArray()
                                                              .SelectMany(
                                                                   r => providerTimeSlots
                                                                       .Where(t => t.ProviderPickupAreaId == r.providerpickupareaid)
                                                                       .ToArray()
                                                               )
                                                              .ToArray()
                };

                wasteStreams.Add(wasteStreamDetails);
            }

            return wasteStreams;
        }

        private async Task<IEnumerable<WasteStreamRow>> GetWasteStreamsRows(int[] providerPickupAreaIds)
        {
            await using var connection = new NpgsqlConnection(_settings.DbConnectionString);

            return await connection.QueryAsync<WasteStreamRow>(sql: EmbeddedResource.Get("GetWasteStreams.sql"), param: new { ids = providerPickupAreaIds.ToArray() });
        }

        private async Task<IEnumerable<WasteStreamSizeRow>> GetWasteStreamSizes(int streamId, int[] providerPickupAreaIds)
        {
            await using var connection = new NpgsqlConnection(_settings.DbConnectionString);

            return await connection.QueryAsync<WasteStreamSizeRow>(sql: EmbeddedResource.Get("GetWasteStreamSizes.sql"), param: new { streamId, providerPickupAreaIds });
        }

        private IEnumerable<ProviderPickupAreaTimeSlots> GetProviderPickupAreaTimeSlots(IEnumerable<TimeSlotRow> timeSlotsRows)
        {
            var providerPickupAreaGroups = timeSlotsRows.GroupBy(r => new { ProviderPickupAreaId = r.providerpickupareaid, LogisticalProviderName = r.logisticalprovidername });

            foreach (var providerGroup in providerPickupAreaGroups)
            {
                var providerPickupAreaId = providerGroup.Key.ProviderPickupAreaId;
                var logisticalProviderName = providerGroup.Key.LogisticalProviderName;

                var dayGroups = providerGroup
                               .ToArray()
                               .GroupBy(r => new { Day = r.day, WeekRecurrence = r.weekrecurrence })
                               .Select(
                                    d => new ProviderPickupAreaDay
                                    {
                                        Day = d.Key.Day,
                                        WeekRecurrence = d.Key.WeekRecurrence,
                                        PickupTimes = d.Select(t => t.pickupstart)
                                                            .ToArray()
                                    }
                                );

                yield return new ProviderPickupAreaTimeSlots
                {
                    LogisticalProviderName = logisticalProviderName,
                    ProviderPickupAreaId = providerPickupAreaId,
                    Days = dayGroups.ToArray()
                };
            }
        }

        private async Task<IEnumerable<TimeSlotRow>> GetLogisticalProviderTimeSlotsForPostalCode(string postalCode)
        {
            int postalCodeNumber = int.Parse(postalCode);

            await using var connection = new NpgsqlConnection(_settings.DbConnectionString);

            return await connection.QueryAsync<TimeSlotRow>(sql: EmbeddedResource.Get("GetTimeSlotsForPostalCode.sql"), param: new { postalCode = postalCodeNumber });
        }

        private async Task<IEnumerable<TimeSlotRow>> GetLogisticalProviderTimeSlotsForPostalCodeAndDays(string postalCode, ushort[] days)
        {
            int postalCodeNumber = int.Parse(postalCode);

            await using var connection = new NpgsqlConnection(_settings.DbConnectionString);

            return await connection.QueryAsync<TimeSlotRow>(sql: EmbeddedResource.Get("GetTimeSlotsForPostalCodeAndDays.sql"), param: new { postalCode = postalCodeNumber, days = days.Select(d => (short)d).ToArray() });
        }
    }
}
