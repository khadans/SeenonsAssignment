select
  s.id,
  s.size,
  s.container_product_id,
  s.discount_percentage,
  s.unit_price_pickup,
  s.unit_price_rent,
  s.unit_price_placement,
  co.type as container_type,
  co.name as container_name
from
  streamsize ss
inner join size s on ss.sizeid = s.id inner join providerpickuparea_container c on c.containerId = s.container_product_id
inner join container co on co.id = c.containerid
where
  c.providerpickupareaid = ANY(:providerPickupAreaIds) and ss.streamid = :streamId
